using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Base GameObjects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject Ground;
    [SerializeField] private List<GameObject> Agents;
    [SerializeField] private TMPro.TextMeshProUGUI infoText;

    // optional: rotation, Y-translation and Size scale to apply to the prefabs correspoding to the different species of agents
    [Header("Transformations applied to agents prefabs")]
    [SerializeField] private List<float> rotations = new List<float> { 90.0f, 90.0f, 0.0f };
    [SerializeField] private List<float> rotationsCoeff = new List<float> { 1, 1, 0.0f };
    [SerializeField] private List<float> YValues = new List<float> { -0.9f, -0.9f, 0.15f };
    [SerializeField] private List<float> Sizefactor = new List<float> { 0.3f, 0.3f, 1.0f }; 

    // optional: define a scale between GAMA and Unity for the location given
    [Header("Coordinate conversion parameters")]
    [SerializeField] private float GamaCRSCoefX = 1.0f;
    [SerializeField] private float GamaCRSCoefY = 1.0f;
    [SerializeField] private float GamaCRSOffsetX = 0.0f;
    [SerializeField] private float GamaCRSOffsetY = 0.0f;

    // Z offset and scale
    [SerializeField] private float GamaCRSOffsetZ = 180.0f;
    [SerializeField] private float GamaCRSCoefZ = 1.0f;

    //Y scale for the ground
    [SerializeField] private float groundY = 1.0f;

    //Y-offset to apply to the background geometries
    [SerializeField] private float offsetYBackgroundGeom = 0.1f;

    [Header("Simulation parameters")]
    [SerializeField] private float minSimulationDuration = 10.0f;
    [SerializeField] private bool geometriesExpected = false;
    [SerializeField] private bool groundExpected = false;
    [SerializeField] private bool playerParametersExpected = true;


    // called when the current game state changes
    public static event Action<GameState> OnGameStateChanged;

    // called when the game is restarted
    public static event Action OnGameRestarted;

    // called when the geometries are initialized
    public static event Action<GAMAGeometry> OnGeometriesInitialized;

    // called when the world data is received
    public static event Action<WorldJSONInfo> OnWorldDataReceived;

    private Timer timer;
    private List<Dictionary<int, GameObject>> agentMapList;

    private bool geometriesInitialized;
    private bool simulationParametersHandled;

    private bool handleGroundRequested;
    private bool handlePlayerRequested;
    private bool handleGeometriesRequested;

    private CoordinateConverter converter;
    private PolygonGenerator polyGen;
    private ConnectionParameter parameters;
    private WorldJSONInfo infoWorld;
    private GAMAGeometry gamaGeometry;

    private GameState currentState;

    public static GameManager Instance = null;

    // ############################################ UNITY FUNCTIONS ############################################
    void Awake() {
        Instance = this;
    }

    void OnEnable() {
        ConnectionManager.OnServerMessageReceived += HandleServerMessageReceived;
        ConnectionManager.OnConnectionAttempted += HandleConnectionAttempted;
        ConnectionManager.OnConnectionStateChange += HandleConnectionStateChange;
    }

    void OnDisable() {
        ConnectionManager.OnServerMessageReceived -= HandleServerMessageReceived;
        ConnectionManager.OnConnectionAttempted -= HandleConnectionAttempted;
        ConnectionManager.OnConnectionStateChange -= HandleConnectionStateChange;
    }

    void Start() {
        UpdateGameState(GameState.MENU);
        // InitAgentsList();
        // timer = GetComponent<Timer>();
        geometriesInitialized = false;
        simulationParametersHandled = false;
        handleGroundRequested = false;
        handlePlayerRequested = false;
        handleGeometriesRequested = false;
        // StartCoroutine(DelayedConnection());
    }

    // private IEnumerator DelayedConnection() {
    //     yield return new WaitForSeconds(0.2f);
    //     ConnectionManager.Instance.TryConnectionToServer();
    // }

    void FixedUpdate() {
        if(IsGameState(GameState.GAME)) {
            UpdatePlayerPosition();
            UpdateAgentsList();
        }
    }

    void LateUpdate() {
        if (handleGroundRequested && !simulationParametersHandled) {
            InitGroundParameters();
            handleGroundRequested = false;
        }

        if (handlePlayerRequested && !simulationParametersHandled) {
            handlePlayerRequested = false;
            InitPlayerParameters();
            
        }

        if (handleGeometriesRequested && !simulationParametersHandled) {
            InitGeometries();
            handleGeometriesRequested = false;
        }
    }

    // ############################################ GAMESTATE UPDATER ############################################
    public void UpdateGameState(GameState newState) {    
        
        switch(newState) {
            case GameState.MENU:
                RemoveInfoText();
                Debug.Log("GameManager: UpdateGameState -> MENU");
                break;

            case GameState.WAITING:
                Debug.Log("GameManager: UpdateGameState -> WAITING");
                RemoveInfoText();
                break;

            case GameState.LOADING_DATA:
                Debug.Log("GameManager: UpdateGameState -> LOADING_DATA");
                ConnectionManager.Instance.SendExecutableExpression("do init_player(\"" + ConnectionManager.Instance.GetConnectionId() + "\");");
                break;

            case GameState.GAME:
                Debug.Log("GameManager: UpdateGameState -> GAME");
                RemoveInfoText();
                timer.SetTimerRunning(true);
                break;

            case GameState.IDLE:
                Debug.Log("GameManager: UpdateGameState -> IDLE");
                timer.SetTimerRunning(false);
                break;

            case GameState.END:
                timer.SetTimerRunning(false);
                Debug.Log("GameManager: UpdateGameState -> END");
                break;

            case GameState.CRASH:
                // timer.Reset();
                Debug.Log("GameManager: UpdateGameState -> CRASH");
                break;

            default:
                Debug.Log("GameManager: UpdateGameState -> UNKNOWN");
                break;
        }
        
        currentState = newState;
        OnGameStateChanged?.Invoke(currentState);
    }

    

    // ############################# INITIALIZERS ####################################
    private void InitPlayerParameters() {
        Vector3 pos = converter.fromGAMACRS(parameters.position[0], parameters.position[1]);
        player.transform.position = pos;

        if (parameters.physics) {
            if (!player.TryGetComponent(out Rigidbody rigidBody)) {
                player.AddComponent<Rigidbody>();
            }
        } else {
            if (player.TryGetComponent(out Rigidbody rigidBody)) {
                Destroy(rigidBody);
            }
        }
        
        UpdateGameState(GameState.GAME);
        Debug.Log("GameManager: Player parameters initialized");
    }


    private void InitGroundParameters() {
        if (Ground == null) {
            Debug.LogError("GameManager: Ground not set");
            return;
        }
        Vector3 ls = converter.fromGAMACRS(parameters.world[0], parameters.world[1]);
        if (ls.z < 0)
            ls.z = -ls.z;
        if (ls.x < 0)
            ls.x = -ls.x;
        ls.y = groundY;
        Ground.transform.localScale = ls;

        Vector3 ps = converter.fromGAMACRS(parameters.world[0] / 2, parameters.world[1] / 2);
        ps.y = -groundY;

        Ground.transform.position = ps;
        Debug.Log("GameManager: Ground parameters initialized");
    }

    private void InitGeometries() {
        if (polyGen == null) {
            polyGen = PolygonGenerator.GetInstance();
            polyGen.Init(converter, offsetYBackgroundGeom);
        }
        polyGen.GeneratePolygons(gamaGeometry);
        geometriesInitialized = true;
        OnGeometriesInitialized?.Invoke(gamaGeometry);
        Debug.Log("GameManager: Geometries initialized");
    }

    // private void InitAgentsList() {
    //     agentMapList = new List<Dictionary<int, GameObject>>();
    //     foreach (GameObject i in Agents) {
    //         agentMapList.Add(new Dictionary<int, GameObject>());
    //     }
    //     Debug.Log("GameManager: Agents list initialized. " + Agents.Count + " species found");
    // }


    // ############################################ UPDATERS ############################################
    private void UpdatePlayerPosition() {
        Vector2 vF = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
        Vector2 vR = new Vector2(transform.forward.x, transform.forward.z);
        vF.Normalize();
        vR.Normalize();
        float c = vF.x * vR.x + vF.y * vR.y;
        float s = vF.x * vR.y - vF.y * vR.x;

        int angle = (int) (((s > 0) ? -1.0 : 1.0) * (180 / Math.PI) * Math.Acos(c) * parameters.precision);

        List<int> p = converter.toGAMACRS(Camera.main.transform.position);
        ConnectionManager.Instance.SendExecutableExpression("do move_player_external($id," + p[0] + "," + p[1] + "," + angle + ");");
    }

    private void UpdateAgentsList() {
        // if (infoWorld.position.Count == 2) {
        //     parameters.position = infoWorld.position;
        //     playerPositionUpdate = true; // Teleportation disabled
        // }

        foreach (Dictionary<int, GameObject> agentMap in agentMapList) {
            foreach (GameObject obj in agentMap.Values) {
                obj.SetActive(false);
            }
        }

        foreach (AgentInfo pi in infoWorld.agents) {
            int speciesIndex = pi.v[0];
            GameObject Agent = Agents[speciesIndex];
            int id = pi.v[1];
            GameObject obj = null;
            Dictionary<int, GameObject> agentMap = agentMapList[speciesIndex];

            if (!agentMap.ContainsKey(id)) {
                obj = Instantiate(Agent);
                float scale = Sizefactor[speciesIndex];
                obj.transform.localScale = new Vector3(scale, scale, scale);
                obj.SetActive(true);
                agentMap.Add(id, obj);
            } else {
                obj = agentMap[id];
            }


            Vector3 pos = converter.fromGAMACRS(pi.v[2], pi.v[3]);
            pos.y = YValues[speciesIndex];
            float rot = rotationsCoeff[speciesIndex] * (pi.v[4] / parameters.precision) + rotations[speciesIndex];
            obj.transform.SetPositionAndRotation(pos, Quaternion.AngleAxis(rot, Vector3.up));
            obj.SetActive(true);
        } 
        
        foreach (Dictionary<int, GameObject> agentMap in agentMapList) {
            List<int> ids = new List<int>(agentMap.Keys);
            foreach (int id in ids) {
                GameObject obj = agentMap[id];
                if (!obj.activeSelf) {
                    obj.transform.position = new Vector3(0, -100, 0);
                    agentMap.Remove(id);
                    GameObject.Destroy(obj);
                }
            }
        }
    }

    // ############################################# HANDLERS ########################################
    private void HandleConnectionStateChange(ConnectionState state) {
        // player has been added to the simulation by the middleware
        if (state == ConnectionState.AUTHENTICATED) {
            Debug.Log("GameManager: Player added to simulation, waiting for initial parameters");
            UpdateGameState(GameState.LOADING_DATA);
        }
    }

    private void HandleServerMessageReceived(JObject jsonObj) {
        string firstKey = jsonObj.Properties().Select(p => p.Name).FirstOrDefault();
        Debug.Log(jsonObj.ToString());
        switch (firstKey) {
            // handle general informations about the simulation
            case "precision":
                parameters = ConnectionParameter.CreateFromJSON(jsonObj.ToString());
                converter = new CoordinateConverter(parameters.precision, GamaCRSCoefX, GamaCRSCoefY, GamaCRSCoefY, GamaCRSOffsetX, GamaCRSOffsetY, GamaCRSOffsetZ);
                Debug.Log("GameManager: Received simulation parameters");
                // Init ground and player
                if (groundExpected) handleGroundRequested = true;
                if (playerParametersExpected) handlePlayerRequested = true;                
            break;

            // handle geometries sent by GAMA at the beginning of the simulation
            case "points":
                gamaGeometry = GAMAGeometry.CreateFromJSON(jsonObj.ToString());
                Debug.Log("GameManager: Received geometries data");
                if (geometriesExpected) handleGeometriesRequested = true;
            break;

            // handle agents while simulation is running
            case "agents":
                infoWorld = WorldJSONInfo.CreateFromJSON(jsonObj.ToString());                
                OnWorldDataReceived?.Invoke(infoWorld);
            break;

            default:
                Debug.LogError("GameManager: Received unknown message from middleware");
                break;
        }

    }

    private void HandleConnectionAttempted(bool success) {
        if (success) {
            if(IsGameState(GameState.MENU)) {
                Debug.Log("GameManager: Successfully connected to middleware");
                UpdateGameState(GameState.WAITING);
            }
        } else {
            // stay in MENU state
            DisplayInfoText("Unable to connect to middleware", Color.red);
        }
    }

    // ############################################# UTILITY FUNCTIONS ########################################
    public void DisplayInfoText(string text, Color color) {
        infoText.text = text;
        infoText.color = color;
    }

    public void RemoveInfoText() {
        DisplayInfoText("", new Color(0,0,0,0));
    }    

    public void RestartGame() {
        OnGameRestarted?.Invoke();        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsGameState(GameState state) {
        return currentState == state;
    }
    
    // public Timer GetTimer() {
    //     return timer;
    // }   

    public float GetMinSimulationDuration() {
        return minSimulationDuration;
    }

    public GameState GetCurrentState() {
        return currentState;
    }
}


// ############################################################
public enum GameState {
    // not connected to middleware
    MENU,
    // connected to middleware, waiting for authentication
    WAITING,
    // connected to middleware, authenticated, waiting for initial data from middleware
    LOADING_DATA,
    // connected to middleware, authenticated, initial data received, exploration phase
    GAME,
    // connected to middleware, authenticated, initial data received, waiting for the next exploration phase
    IDLE,
    END,
    CRASH
}