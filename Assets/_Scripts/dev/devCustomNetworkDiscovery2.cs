using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class devCustomNetworkDiscovery2 : NetworkDiscovery
{

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);

        devTestNetworkingSceneManager2.networkManager.networkAddress = fromAddress;
        devTestNetworkingSceneManager2.networkManager.StartClient();
        Debug.Log("Found a broadcast!");
        StopBroadcast();
    }


}
