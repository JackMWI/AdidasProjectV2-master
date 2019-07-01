using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class devClient1 : NetworkBehaviour
{
    [SyncVar]
    public Color currentColor = Color.white;

    public override void OnStartLocalPlayer()
    {
        currentColor = Random.ColorHSV(0, 1, 0.9f, 1, 0.85f, 1);
        CmdAddIndicator();
        devTestNetworkingManager._localInstance.SuccessfullyConnectedToServer(gameObject);
    }

    [Command]
    public void CmdAddIndicator()
    {
        devTestNetworkingManager._localInstance.CreateClientConnectedIndicator(currentColor);
    }

    [Command]
    public void CmdDevDing()
    {
        devTestNetworkingManager._localInstance.DevDing();
    }
}
