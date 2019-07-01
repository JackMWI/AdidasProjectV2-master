using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

public class devTCPTest1 : MonoBehaviour
{
    public string networkHostName = "127.0.0.1";
    public int networkPort = 4400;

    private TcpClient tcpClient;

    private void Start()
    {
        tcpClient = new TcpClient(networkHostName, networkPort);
        
    }

    private void SendTCPMessage(string message)
    {
        byte[] messageByte = System.Text.Encoding.ASCII.GetBytes(message);
        NetworkStream stream = tcpClient.GetStream();

        stream.Write(messageByte, 0, messageByte.Length);
        Debug.Log("Sent message " + message);

        stream.Close();
    }

    private void OnDestroy()
    {
        tcpClient.Close();
    }
}
