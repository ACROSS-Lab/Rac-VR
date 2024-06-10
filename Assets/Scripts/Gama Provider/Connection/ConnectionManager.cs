using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;

public class ConnectionManager : WebSocketConnector
{

    [SerializeField] private ConnectionState currentState;
    private string connectionId = null;
    private bool connectionRequested;

    // called when the connection state is manually changed
    public static event Action<ConnectionState> OnConnectionStateChange;

    // called when a "json_simulation" message is received
    public static event Action<JObject> OnServerMessageReceived;

    // called when a "json_state" message is received 
    public static event Action<JObject> OnConnectionStateReceived;

    // called when a connection request fails
    public static event Action<bool> OnConnectionAttempted;

    public static ConnectionManager Instance = null;
    
    // ############################################# UNITY FUNCTIONS #############################################
    void Awake() {
        Instance = this;
    }

    void Start() {
       // connectionId = Guid.NewGuid().ToString();
        UpdateConnectionState(ConnectionState.DISCONNECTED);
        connectionRequested = false;
        TryConnectionToServer();
        // if (PlayerPrefs.GetString("CONNECT_ID") != "") {
        //     connectionId = PlayerPrefs.GetString("CONNECT_ID");
        // } else {
        //     connectionId = Guid.NewGuid().ToString();
        //     PlayerPrefs.SetString("CONNECT_ID", connectionId);
        // }
        // UpdateConnectionState(ConnectionState.DISCONNECTED);
        // connectionRequested = false;
    }

    
    // ############################################# CONNECTION HANDLER #############################################
    public void UpdateConnectionState(ConnectionState newState) {

        switch(newState) {
            case ConnectionState.PENDING:
                Debug.Log("ConnectionManager: UpdateConnectionState -> PENDING");
                break;
            case ConnectionState.CONNECTED:
                Debug.Log("ConnectionManager: UpdateConnectionState -> CONNECTED");
                break;
            case ConnectionState.AUTHENTICATED:
                Debug.Log("ConnectionManager: UpdateConnectionState -> AUTHENTICATED");
                break;
            case ConnectionState.DISCONNECTED:
                Debug.Log("ConnectionManager: UpdateConnectionState -> DISCONNECTED");
                break;
            default:
                break;
        }

        currentState = newState;
        OnConnectionStateChange?.Invoke(newState);        
    }

    // ############################################# HANDLERS #############################################

    protected override void HandleConnectionOpen(object sender, System.EventArgs e)
    {
        bool UseHeartbeat = false;
        var jsonId = new Dictionary<string,string> {
                {"type", "connection"},
                { "id", GetConnectionId()},
                { "set_heartbeat", UseHeartbeat ? "true": "false" }
            };
        string jsonStringId = JsonConvert.SerializeObject(jsonId);
        SendMessageToServer(jsonStringId, new Action<bool>((success) => {
            if (success) {}
        }));
        Debug.Log("ConnectionManager: Connection opened");
    }

  
    protected override void HandleReceivedMessage(object sender, MessageEventArgs e)
    {
        if (e.IsText)
        {
            Debug.Log(e.Data);
            JObject jsonObj = JObject.Parse(e.Data);
            string type = (string) jsonObj["type"];

            switch(type) {
                case "ping":
                    var jsonId = new Dictionary<string, string> { { "type", "pong" } };
                    string jsonStringId = JsonConvert.SerializeObject(jsonId);
                    SendMessageToServer(jsonStringId, new Action<bool>((success) => {
                        if (success) { }
                    }));
                    break;
                case "json_state":
                    OnConnectionStateReceived?.Invoke(jsonObj);
                    bool authenticated = (bool)jsonObj["in_game"];
                    bool connected = (bool)jsonObj["connected"];

                    if (authenticated && connected)
                    {
                        if (!IsConnectionState(ConnectionState.AUTHENTICATED))
                        {
                            Debug.Log("ConnectionManager: Player successfully authenticated");
                            UpdateConnectionState(ConnectionState.AUTHENTICATED);
                        }

                    }
                    else if (connected && !authenticated)
                    {
                        if (!IsConnectionState(ConnectionState.CONNECTED))
                        {
                            connectionRequested = false;
                            Debug.Log("ConnectionManager: Successfully connected, waiting for authentication...");
                            UpdateConnectionState(ConnectionState.CONNECTED);
                            OnConnectionAttempted?.Invoke(true);
                        }
                        else
                        {
                            Debug.LogWarning("ConnectionManager: Already connected, waiting for authentication...");
                        }

                    }
                    break;


                   /* OnConnectionStateReceived?.Invoke(jsonObj);
                    bool authenticated = (bool) jsonObj["player"][connectionId]["authentified"];
                    bool connected = (bool) jsonObj["player"][connectionId]["connected"];

                    if (authenticated && connected)  {
                        if (!IsConnectionState(ConnectionState.AUTHENTICATED)) {
                            Debug.Log("ConnectionManager: Player successfully authenticated");
                            UpdateConnectionState(ConnectionState.AUTHENTICATED);
                        }
                        
                    } else if (connected && !authenticated) {
                        if(!IsConnectionState(ConnectionState.CONNECTED)) {
                            connectionRequested = false;
                            Debug.Log("ConnectionManager: Successfully connected, waiting for authentication...");
                            UpdateConnectionState(ConnectionState.CONNECTED);
                            OnConnectionAttempted?.Invoke(true);
                        } else {
                            Debug.LogWarning("ConnectionManager: Already connected, waiting for authentication...");
                        }
                        
                    }
                    break;*/

                case "json_output":
                    JObject content = (JObject) jsonObj["contents"];
                    OnServerMessageReceived?.Invoke(content);
                    break;

                default:
                    break;
            }
        }
    }

    protected override void HandleConnectionClosed(object sender, CloseEventArgs e) {   
        // checks if the connection was closed just after a connection request
        if (connectionRequested) { 
            connectionRequested = false;
            OnConnectionAttempted?.Invoke(false);
            Debug.Log("ConnectionManager: Failed to connect to server");
        }
        UpdateConnectionState(ConnectionState.DISCONNECTED);
    }

    // ############################################# UTILITY FUNCTIONS #############################################
    public void TryConnectionToServer() {
        if(IsConnectionState(ConnectionState.DISCONNECTED)) {
            Debug.Log("ConnectionManager: Attempting to connect to middleware...");
            connectionRequested = true;
            UpdateConnectionState(ConnectionState.PENDING);
            GetSocket().Connect();
        } else {
            Debug.LogWarning("ConnectionManager: Already connected to middleware");
        }
        
    }

    public bool IsConnectionState(ConnectionState currentState) {
        return this.currentState == currentState;
    }

    public void SendExecutableExpression(string expression) {
        Dictionary<string,string> jsonExpression = new Dictionary<string,string> {
            {"type", "expression"},
            {"expr", expression}
        };
        string jsonStringExpression = JsonConvert.SerializeObject(jsonExpression);
        Debug.Log("jsonStringExpression: " + jsonStringExpression);
        SendMessageToServer(jsonStringExpression, new Action<bool>((success) => {
            if (!success) {
                Debug.LogError("ConnectionManager: Failed to send executable expression");
            }
        }));
    }

    

    public string GetConnectionId()
    {

        if (connectionId == null || connectionId.Length == 0)
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            string lastIP = myIP.Split(".")[3];
            connectionId = "Player_" + lastIP;// + lastIP;
        }
        return connectionId;
    }

}


public enum ConnectionState {
    DISCONNECTED,
    // waiting for connection to be established
    PENDING, 
    // connection established, waiting for authentication
    CONNECTED,
    // connection established and authenticated
    AUTHENTICATED
}
