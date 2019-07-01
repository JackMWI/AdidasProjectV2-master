using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;

public class devTCPTest2 : MonoBehaviour
{
    [Header("UI Stuff")]
    public Button connectAndSendButton;

    [Header("Network Stuff")]
    public string networkHostName = "127.0.0.1";
    public int networkPort = 4400;

    [Header("Manual Stuff")]
    public string messageToSend = "TestTestTestTestTestTestTestTestTestTest";

    private TcpClient tcpClient;

    public void ButtonSendMessage()
    {
        ConnectAndSendMessage(messageToSend);
    }

    private void ConnectAndSendMessage(string messageIn)
    {
        if(tcpClient != null)
        {
            tcpClient.Close();
        }
        tcpClient = new TcpClient(networkHostName, networkPort);

        byte[] messageByte = System.Text.Encoding.ASCII.GetBytes(messageIn);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(messageByte, 0, messageByte.Length);
        Debug.Log("Sent message " + messageIn);

        stream.Close();
        tcpClient.Close();
    }
}
