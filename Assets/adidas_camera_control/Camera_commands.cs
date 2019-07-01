using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System;

public class Camera_commands : MonoBehaviour
{

    public int port;
    public string ip_address;


    public void TakePicture(string email, string name, string option)
    {
        UdpClient udpClient = new UdpClient();
        try
        {
            udpClient.Connect(ip_address, port);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(email + "|" + name + "|" + option + "\r\n");
            udpClient.Send(sendBytes, sendBytes.Length);
            udpClient.Close();
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
