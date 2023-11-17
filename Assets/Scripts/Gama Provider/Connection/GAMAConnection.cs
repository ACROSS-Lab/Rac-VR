using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using TMPro;
using UnityEngine.SceneManagement;

// public class GlobalTest : TCPConnector
public class GlobalTest : MonoBehaviour
{

    // [SerializeField] private GameObject Player;

    // [SerializeField] private GameObject Ground;

    // [SerializeField] private List<GameObject> Agents;

    // //optional: rotation, Y-translation and Size scale to apply to the prefabs correspoding to the different species of agents
    // [SerializeField] private List<float> rotations = new List<float> { 90.0f, 90.0f, 0.0f };
    // [SerializeField] private List<float> rotationsCoeff = new List<float> { 1, 1, 0.0f };
    // [SerializeField] private List<float> YValues = new List<float> { -0.9f, -0.9f, 0.15f };
    // [SerializeField] private List<float> Sizefactor = new List<float> { 0.3f, 0.3f, 1.0f }; 

    // // optional: define a scale between GAMA and Unity for the location given
    // [SerializeField] private float GamaCRSCoefX = 1.0f;
    // [SerializeField] private float GamaCRSCoefY = 1.0f;
    // [SerializeField] private float GamaCRSOffsetX = 0.0f;
    // [SerializeField] private float GamaCRSOffsetY = 0.0f;

    // // Z offset and scale
    // [SerializeField] private float GamaCRSOffsetZ = 180.0f;
    // [SerializeField] private float GamaCRSCoefZ = 1.0f;

    // //Y scale for the ground
    // [SerializeField] private float groundY = 1.0f;

    // //Y-offset to apply to the background geometries
    // [SerializeField] private float offsetYBackgroundGeom = 0.1f;



    // private List<Dictionary<int, GameObject>> agentMapList;

    // private TcpClient socketConnection;
    // private Thread clientReceiveThread;

    // private bool initialized = false;
    // private bool playerPositionUpdate = false;

    // private bool defineGroundSize = false;

    // private static bool receiveInformation = true;

    // private WorldJSONInfo infoWorld = null;

    // private ConnectionParameter parameters = null;

    // private List<GAMAGeometry> geoms;

    // private static System.Timers.Timer aTimer;

    // private CoordinateConverter converter;

    // private Dictionary<int, GameObject> buildingsMap;

    // private PolygonGenerator polyGen;

    // private static bool tryGAMAConnection;
    // private Coroutine connectCoroutine;
    // private int periodAlive;

    // public static event Action<WorldJSONInfo> OnGamaDataReceived;
    
    // void OnEnable() {
    //     GameManager.OnGameRestarted += HandleGameRestart;
    // }

    // void OnDisable() {
    //     GameManager.OnGameRestarted -= HandleGameRestart;
    // }

    // void Start() {
    //     tryGAMAConnection = false;
    //     periodAlive = 0;
    //     agentMapList = new List<Dictionary<int, GameObject>>();
    //     foreach (GameObject i in Agents)
    //     {
    //         agentMapList.Add(
    //             new Dictionary<int, GameObject>());
    //     }
    //     Debug.Log("START WORLD");
    //     connectCoroutine = StartCoroutine(StartGameCoroutine());
    // }

    // IEnumerator StartGameCoroutine() {
    //     Debug.Log("Start coroutine started");
    //     while(!tryGAMAConnection) {
    //         yield return new WaitForSeconds(0.5f);
    //     }
    //     ConnectToTcpServer();
    // }


    // void FixedUpdate() {  
    //     // checkGamaConnection();     

    //     // GENERATE BUILDINGS IF NOT DONE
    //     if (parameters != null && geoms != null) {
    //         foreach (GAMAGeometry g in geoms) {
    //             if (polyGen == null) {
    //                 polyGen = PolygonGenerator.GetInstance();
    //                 polyGen.Init(converter, offsetYBackgroundGeom);
    //             }
    //             polyGen.GeneratePolygons(g);
    //         }

    //         buildingsMap = polyGen.GetGeneratedBuildings();
    //         geoms = null;
    //     } 
        
    //     // DEFINE GROUND SIZE AND PLAYER INITIAL PARAMETERS
    //     if (parameters != null && Ground != null && !defineGroundSize) {
    //         Vector3 ls = converter.fromGAMACRS(parameters.world[0], parameters.world[1]);
    //         if (ls.z < 0)
    //             ls.z = -ls.z;
    //         if (ls.x < 0)
    //             ls.x = -ls.x;
    //         ls.y = groundY;
    //         Ground.transform.localScale = ls;

    //         Vector3 ps = converter.fromGAMACRS(parameters.world[0] / 2, parameters.world[1] / 2);
    //         ps.y = -groundY;

    //         Ground.transform.position = ps;
    //         defineGroundSize = true;

    //         if (Player != null) {
    //             Vector3 pos = converter.fromGAMACRS(parameters.position[0], parameters.position[1]);
    //             Player.transform.position = pos;
    //         } 
            
    //         if (parameters.physics && Player != null) {
    //             if (!Player.TryGetComponent(out Rigidbody rigidBody)) {
    //                 Player.AddComponent<Rigidbody>();
    //             }
    //         } else {
    //             if (Player.TryGetComponent(out Rigidbody rigidBody)) {
    //                 Destroy(rigidBody);
    //             }
    //         }
    //     }

    //     // HANDLE PLAYER TELEPORTTION FROM GAMA TO UNITY
    //     if (Player != null && playerPositionUpdate && parameters != null) {
    //         Player.transform.position = converter.fromGAMACRS(parameters.position[0], parameters.position[1]);
    //         playerPositionUpdate = false;
    //     }

    //     // SEND PLAYER POSITION TO GAMA
    //     if (initialized && Player != null && receiveInformation) {
    //         SendPlayerPosition();
    //     } 
        
    //     // HANDLE SIMUATION INFORMATIONS RECEIVED FROM GAMA
    //     if (infoWorld != null && receiveInformation) {
    //         OnGamaDataReceived?.Invoke(infoWorld);
    //         UpdateAgentList();
    //         infoWorld = null;
    //     }
    // }


    // private void SendPlayerPosition() {
    //     Vector2 vF = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
    //     Vector2 vR = new Vector2(transform.forward.x, transform.forward.z);
    //     vF.Normalize();
    //     vR.Normalize();
    //     float c = vF.x * vR.x + vF.y * vR.y;
    //     float s = vF.x * vR.y - vF.y * vR.x;

    //     double angle = ((s > 0) ? -1.0 : 1.0) * (180 / Math.PI) * Math.Acos(c) * parameters.precision;
        

    //     List<int> p = converter.toGAMACRS(Camera.main.transform.position);
    //     string closedRoads = BuildClosedRoadsArray();
    //     if (GameManager.Instance.IsState(GameState.GAME)) SendMessageToServer("{\"position\":[" + p[0] + "," + p[1] + "],\"rotation\": " + (int)angle + ", \"closedRoads\": " + closedRoads + "}");
    // }

    // private string BuildClosedRoadsArray() {
    //     List<string> closedRoads = RoadManager.GetClosedRoads();
    //     StringBuilder t = new StringBuilder("[");
    //     for(int i = 0; i < closedRoads.Count; i++) {
    //         t.Append("\"" + closedRoads[i] + "\"");
    //         if (i < closedRoads.Count - 1) {t.Append(",");}
    //     }
    //     t.Append("]");
    //     return t.ToString();
    // }

    // private void UpdateAgentList() {
    //     if (infoWorld.position.Count == 2)
    //     {
    //         parameters.position = infoWorld.position;
    //         playerPositionUpdate = true;

    //     }
    //     foreach (Dictionary<int, GameObject> agentMap in agentMapList)
    //     {
    //         foreach (GameObject obj in agentMap.Values)
    //         {
    //             obj.SetActive(false);
    //         }
    //     }

    //     foreach (AgentInfo pi in infoWorld.agents)
    //     {
    //         int speciesIndex = pi.v[0];
    //         GameObject Agent = Agents[speciesIndex];
    //         int id = pi.v[1];
    //         GameObject obj = null;
    //         Dictionary<int, GameObject> agentMap = agentMapList[speciesIndex];
    //         if (!agentMap.ContainsKey(id))
    //         {
    //             obj = Instantiate(Agent);
    //             float scale = Sizefactor[speciesIndex];
    //             obj.transform.localScale = new Vector3(scale, scale, scale);
    //             obj.SetActive(true);

    //             agentMap.Add(id, obj);
    //         } else {
    //             obj = agentMap[id];
    //         }


    //         Vector3 pos = converter.fromGAMACRS(pi.v[2], pi.v[3]);
    //         pos.y = YValues[speciesIndex];
    //         float rot = rotationsCoeff[speciesIndex] * (pi.v[4] / parameters.precision) + rotations[speciesIndex];
    //         obj.transform.SetPositionAndRotation(pos, Quaternion.AngleAxis(rot, Vector3.up));

    //         obj.SetActive(true);

    //     } foreach (Dictionary<int, GameObject> agentMap in agentMapList) {
    //         List<int> ids = new List<int>(agentMap.Keys);
    //         foreach (int id in ids)
    //         {
    //             GameObject obj = agentMap[id];
    //             if (!obj.activeSelf)
    //             {
    //                 obj.transform.position = new Vector3(0, -100, 0);
    //                 agentMap.Remove(id);
    //                 GameObject.Destroy(obj);
    //             }
    //         }
    //     }
    // }

    // protected override void ManageMessage(string mes) {
    //     if (mes.Contains("precision")) {
            
    //         parameters = ConnectionParameter.CreateFromJSON(mes);
    //         // converter = new CoordinateConverter(parameters.precision, GamaCRSCoefX, GamaCRSCoefY, GamaCRSOffsetX, GamaCRSOffsetY);
    //         converter = new CoordinateConverter(parameters.precision, GamaCRSCoefX, GamaCRSCoefY, GamaCRSCoefY, GamaCRSOffsetX, GamaCRSOffsetY, GamaCRSOffsetZ); // Added Z
    //         SendMessageToServer("ok");
    //         initialized = true;
    //         playerPositionUpdate = true;

    //     } else if (mes.Contains("points")) {
    //         GAMAGeometry g = GAMAGeometry.CreateFromJSON(mes);
    //         if (geoms == null)
    //         {
    //             geoms = new List<GAMAGeometry>();
    //         }
    //         geoms.Add(g);

    //     } else if (mes.Contains("agents") && parameters != null) {
    //         infoWorld = WorldJSONInfo.CreateFromJSON(mes);
    //     }

    // }

   
    // public void TryLaunchGame() {
    //     tryGAMAConnection = true;
    // }

    // private void checkGamaConnection() {
    //     Thread gamaThread = GetClientReceiveThread() != null ? GetClientReceiveThread() : null;
        
    //     if (gamaThread != null && !gamaThread.IsAlive) {
    //         if (tryGAMAConnection) { 
    //             Debug.Log("Connection with GAMA lost");
    //             GameManager.Instance.DisplayInfoText("Unable to connect to GAMA", Color.red);
    //             if (connectCoroutine != null) StopCoroutine(connectCoroutine);
    //             tryGAMAConnection = false;
    //             connectCoroutine = StartCoroutine(StartGameCoroutine());
    //             SetClientReceiveThread(null);
    //         }

    //         if (GameManager.Instance.IsState(GameState.GAME) || GameManager.Instance.IsState(GameState.WAITING)) {
    //             GameManager.Instance.DisplayInfoText("Connection lost. Restart game", Color.red);
    //             GameManager.Instance.UpdateGameState(GameState.CRASH);
    //         }
    //     } else if(gamaThread != null && gamaThread.IsAlive && GameManager.Instance.IsState(GameState.MENU)) {
    //         periodAlive++;
    //         if (periodAlive==5) {
    //             GameManager.Instance.UpdateGameState(GameState.WAITING);
    //             tryGAMAConnection = false;
    //         }
            
    //     }
    // }

    // private void HandleGameRestart() {
    //     polyGen = null;
    //     PolygonGenerator.DestroyInstance();
    // }
 }
