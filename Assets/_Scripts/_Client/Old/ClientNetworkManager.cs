using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

/*public class ClientNetworkManager : NetworkManager
{
    public UnityEvent OnClientErrorEvent, OnClientConnectEvent, OnClientDisconnectEvent, OnClientTryReconnect;

    // Bool that determines if the client has connected in the past. Assumed true at the beginning
    private bool hasConnected = true;

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        Debug.Log(errorCode);
        base.OnClientError(conn, errorCode);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if (conn.lastError == NetworkError.Timeout && hasConnected)
        {
            TryReconnecting();
        }
        else
        {
            Debug.Log("Generic Disconnect");
            OnClientDisconnectEvent.Invoke();
        }
        //base.OnClientDisconnect(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnectEvent.Invoke();
        hasConnected = true;
    }

    public void TryReconnecting()
    {
        Debug.Log("Dropped connection from server, trying to reconnect...");
        hasConnected = false;
        OnClientTryReconnect.Invoke();
    }
}*/
