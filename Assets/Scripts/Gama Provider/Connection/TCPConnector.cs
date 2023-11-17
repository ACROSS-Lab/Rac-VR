using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using TMPro;

public abstract class TCPConnector : MonoBehaviour
{
    public string ip = "localhost";
    public int port = 8000;
    [SerializeField] private string endMessageSymbol = "&&&";

    private static TcpClient socketConnection;
    private static Thread clientReceiveThread;

    protected void SendMessageToServer(string clientMessage)
    {

        if (socketConnection == null) {
            return;
        } 
        
        try {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            // stream.Flush();
            if (stream.CanWrite)
            {
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(clientMessage + "\n");

                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                //stream.Flush();
                //Debug.Log("Client sent his message - should be received by server");
            }
        } catch (Exception e) {
            Debug.Log("Unable to send message to server : " + e.Message);
        }
    }

    protected virtual void ManageMessage(string message) { }

    protected void ListenForData() {
        try {
            socketConnection = new TcpClient(PlayerPrefs.GetString("IP"), port);
            SendMessageToServer("connected");
            Byte[] bytes = new Byte[1024];
            string fullMessage = "";
            while (true)
            {
                using (NetworkStream stream = socketConnection.GetStream())
                {

                    int length=1;

                    if(length==0) {
                        Debug.Log("Connection closed");
                        break;
                    }
                    // Read incomming stream into byte arrary. 					

                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);					
                        string serverMessage = Encoding.UTF8.GetString(incommingData);
                        stream.Flush();
                        fullMessage += serverMessage;
                        if (fullMessage.Contains(endMessageSymbol))
                        {

                            string[] messages = fullMessage.Split(endMessageSymbol);
                            
                            for (int i = 0; i < messages.Length - 1; i++)
                            {
                                string mes = messages[i];
                                ManageMessage(mes);
                            }

                            fullMessage = messages[messages.Length - 1] != null ? messages[messages.Length - 1] : "";
                        }
                    }
                }
            }
            
        } catch (SocketException socketException) {
            Debug.Log("Socket exception: " + socketException);
            return;
        }
    }

    public void ConnectToTcpServer() {
        try {
            Debug.Log("Connecting to TCP server...");
            
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        } catch (Exception e) {
            Debug.Log("Error while connecting client to server " + e);
        }
    }

    public static Thread GetClientReceiveThread() {
        return clientReceiveThread;
    }

    public static TcpClient GetSocketConnection() {
        return socketConnection;
    }

    public static void SetClientReceiveThread(Thread thread) {
        clientReceiveThread = thread;
    }

    public static void ResetConnection() {
        socketConnection = null;
        clientReceiveThread = null;
    }


}
