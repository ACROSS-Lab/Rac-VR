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
  /*  [Header("Base GameObjects")]
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject WasteDisplayM;
    [SerializeField] private GameObject WasteCollectionI;
    [SerializeField] private GameObject HelpM;

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

    [SerializeField] private GameStateDisplay disDebug;

    // ADDED
    [SerializeField]
    private  DisplayManagement dm;
    private ModeConfig mc;
    private HelpManagement hm;


    // called when the current game state changes
    public event Action<GameState> OnGameStateChanged;

    // called when the game is restarted
    public event Action OnGameRestarted;

    private CoordinateConverter converter;
    private PolygonGenerator polyGen;
    private ConnectionParameter parameters;
    private WorldJSONInfo infoWorld;
    private GAMAGeometry gamaGeometry;
    private ConnectionClass classIndicators;

    private GameState currentState = GameState.MENU;

    private int villageId;

    public static GameManager Instance = null;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

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
         villageId = -1;
        initialPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        initialRotation = new Quaternion(player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w);
        
    }

    void FixedUpdate() {
        if(IsGameState(GameState.GAME)) {
            UpdatePlayerPosition();
        }

        if (classIndicators != null)
        {
            player.transform.SetLocalPositionAndRotation(initialPosition, initialRotation);
            UpdateClassIndicator();
            UpdateGameState(GameState.READY);
        }
       
    }


    // ############################################ GAMESTATE UPDATER ############################################
    public void UpdateGameState(GameState newState) {    
        
        switch(newState) {
            case GameState.MENU:
                Debug.Log("GameManager: UpdateGameState -> MENU");
                break;

            case GameState.WAITING:
                Debug.Log("GameManager: UpdateGameState -> WAITING");
                break;

            case GameState.LOADING_DATA:
                Debug.Log("GameManager: UpdateGameState -> LOADING_DATA");
                ConnectionManager.Instance.SendExecutableExpression("do init_player(\"" + ConnectionManager.Instance.GetConnectionId() + "\");");
                break;

            case GameState.GAME:
                
                Debug.Log("GameManager: UpdateGameState -> GAME");
                break;

            case GameState.IDLE:
                Debug.Log("GameManager: UpdateGameState -> IDLE");
                break;
            case GameState.READY:
                Debug.Log("GameManager: UpdateGameState -> READY");
                break;

            case GameState.END:
                Debug.Log("GameManager: UpdateGameState -> END");
                break;

            case GameState.CRASH:
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
        
        UpdateGameState(GameState.IDLE);
        Debug.Log("GameManager: Player parameters initialized");
    }

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
        ConnectionManager.Instance.SendExecutableExpression("do move_player_external("+ villageId + "," + p[0] + "," + p[1] + "," + 0 + ");");
        
    }

   
    private void UpdateClassIndicator() {
        Debug.Log("villageId: " + villageId + " " + classIndicators.solidwasteSoilClass[villageId] +" " + classIndicators.solidwasteCanalClass[villageId]);
        classIndicators.displaySolidClass(classIndicators.solidwasteSoilClass[villageId], classIndicators.solidwasteCanalClass[villageId]);
        Debug.Log("2 villageId: " + villageId);

        classIndicators.displayWaterClass(classIndicators.waterwasteClass[villageId]);
        classIndicators.displayProductionClass(classIndicators.productionClass[villageId]);
        classIndicators.displayWaterColor(classIndicators.waterwasteClass[villageId]);
        Debug.Log("3 villageId: " + villageId);

       classIndicators = null;
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
        Debug.Log(firstKey);
        switch (firstKey) {
            // handle general informations about the simulation
            case "precision":
                parameters = ConnectionParameter.CreateFromJSON(jsonObj.ToString());
                converter = new CoordinateConverter(parameters.precision, GamaCRSCoefX, GamaCRSCoefY, GamaCRSCoefY, GamaCRSOffsetX, GamaCRSOffsetY, 0.0f);
                Debug.Log("GameManager: Received simulation parameters");
                Debug.Log(jsonObj.ToString());
                // Init ground and player
                villageId = parameters.village_id;

                Timer.SetTimerDuration((float) parameters.exploration_duration);
                UpdateGameState(GameState.IDLE);

                break;

        
            case "solidwasteSoilClass":
                classIndicators = ConnectionClass.CreateFromJSON(jsonObj.ToString(), dm);
             
                 break;


            case "stopVR":
                UpdateGameState(GameState.IDLE);
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
        } 
    }

    // ############################################# UTILITY FUNCTIONS ########################################

    public bool IsGameState(GameState state) {
        return currentState == state;
    }  

    public GameState GetCurrentState() {
        return currentState;
    }

    public int GetVillageId() {
        return villageId;
    }

    public void StartGame() {
        UpdateGameState(GameState.GAME);
       
    }
    */
}

/*public enum GameState {
    // not connected to middleware
    MENU,
    // connected to middleware, waiting for authentication
    WAITING,
    // connected to middleware, authenticated, waiting for initial data from middleware
    LOADING_DATA,
    // connected to middleware, authenticated, initial data received, waiting for the next exploration phase
    IDLE,
    // connected to middleware, authenticated, initial data received,  waiting for the click on start
    READY,
    // connected to middleware, authenticated, initial data received, exploration phase
    GAME,   
    
    END,
    CRASH
}*/
