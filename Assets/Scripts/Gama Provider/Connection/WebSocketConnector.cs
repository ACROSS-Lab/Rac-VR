using System;
using UnityEngine;
using WebSocketSharp;


public abstract class WebSocketConnector : MonoBehaviour
{

    public string host = "10.0.103.89";
    public string port = "8080";

    protected bool UseMiddleware = true; 

    private WebSocket socket;

    public bool UseHeartbeat = true; //only for middleware mode

    public bool DesktopMode = false;
   
    public int numErrorsBeforeDeconnection = 10;
    protected int numErrors = 0;

    void OnEnable() {
       
       
        
        if (DesktopMode)
        {
            host = "localhost";

            if (UseMiddleware)
            {
                port = "8080";
            }
            else 
            {
                port = "1000";
            }
            
        }
        Debug.Log("WebSocketConnector host: " + host + " PORT: " + port + " MIDDLEWARE:" + UseMiddleware);

        socket = new WebSocket("ws://" + host + ":" + port + "/");
        socket.OnOpen += HandleConnectionOpen;
        socket.OnMessage += HandleReceivedMessage;
        socket.OnClose += HandleConnectionClosed;
    }

   void OnDestroy() {
        socket.Close();
    }

    // ############################## HANDLERS ##############################

    protected abstract void HandleConnectionOpen(object sender, System.EventArgs e);

    protected abstract void HandleReceivedMessage(object sender, MessageEventArgs e);

    protected abstract void HandleConnectionClosed(object sender, CloseEventArgs e);

    // #######################################################################

    protected void SendMessageToServer(string message, Action<bool> successCallback) {
       socket.SendAsync(message, successCallback);
    }

    protected WebSocket GetSocket() {
        return socket;
    }

    private bool ValidIp(string ip) {
        if (ip == null || ip.Length == 0) return false;
        string[] ipb = ip.Split(".");
        return (ipb.Length != 4);
    }
}
