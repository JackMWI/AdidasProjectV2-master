using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class devCustomNetworkDiscovery3 : NetworkDiscovery
{

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);

        devTestNetworkingSceneManager3.networkManager.networkAddress = fromAddress;
        devTestNetworkingSceneManager3.networkManager.StartClient();
        Debug.Log("Found a broadcast!");
        StopBroadcast();
    }


}
