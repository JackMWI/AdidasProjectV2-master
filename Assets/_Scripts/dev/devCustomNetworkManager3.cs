using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class devCustomNetworkManager3 : NetworkManager
{
    public UnityEvent ConnectionSuccesful, ConnectionFailed;

    public override void OnClientConnect(NetworkConnection conn)
    {
        ConnectionSuccesful.Invoke();
        base.OnClientConnect(conn);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        ConnectionFailed.Invoke();
        base.OnClientError(conn, errorCode);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        ConnectionFailed.Invoke();
        base.OnClientDisconnect(conn);
    }
}
