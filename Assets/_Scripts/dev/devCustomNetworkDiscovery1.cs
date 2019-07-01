using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class devCustomNetworkDiscovery1 : NetworkDiscovery
{
    public devTestNetworkingManager devNetManager;

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        NetworkManager.singleton.networkAddress = fromAddress;
        NetworkManager.singleton.StartClient();
        Debug.Log("Found a broadcast!");
        StopBroadcast();
        // Creates client prefab, which upon starting calls devNetManager.SuccessfullyConnectedToServer();
    }


}
