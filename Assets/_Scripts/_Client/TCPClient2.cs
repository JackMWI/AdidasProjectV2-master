using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

public class TCPClient2 : MonoBehaviour
{
    [Header("Required Network Information")]
    public string networkHostName = "127.0.0.1";
    public int networkPort = 4400;

    [Header("Debug Stuff")]
    public string debugMessage = "";

    private TcpClient tcpClient;

    // Called by Client2UIController
    public bool SendMessageOverNetwork(string messageIn)
    {
        try
        {
            ConnectAndSendMessage(messageIn);
            return true;
        }
        catch(System.Exception e)
        {
            Debug.Log("Caught exception while trying to send TCP message: " + e.ToString());
            return false;
        }
    }

    private void ConnectAndSendMessage(string messageIn)
    {
        if (tcpClient != null)
        {
            tcpClient.Close();
        }


        string ipToUse = networkHostName;
        // Checks to see if debug override IP should be used instead
        if(SurveyPageDebug.inDebugMode && SurveyPageDebug.shouldUseDebugIP)
        {
            ipToUse = SurveyPageDebug.debugIP;
        }
        Debug.Log("Attempting to send message to " + ipToUse + " ...");
        tcpClient = new TcpClient(ipToUse, networkPort);

        byte[] messageByte = System.Text.Encoding.ASCII.GetBytes(messageIn);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(messageByte, 0, messageByte.Length);
        Debug.Log("Sent message " + messageIn);

        stream.Close();
        tcpClient.Close();
    }


    [ContextMenu("Send Debug Result")]
    void SendDebugResult()
    {
        if(!SendMessageOverNetwork(debugMessage))
        {
            Debug.Log("Failed to send debug message!");
        }
    }
}
