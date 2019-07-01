using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class devClient2 : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        devTestNetworkingSceneManager2._localInstance.currentClient = this;
    }

    [Command]
    public void CmdCallDevDing()
    {
        devTestNetworkingSceneManager2._localInstance.DevDing();
    }

    [Command]
    public void CmdSendFormInfo(string currentName, string currentColor, string currentEmail)
    {
        devTestNetworkingSceneManager2._localInstance.AddNewFormInfo(new devForm2(currentName, currentColor, currentEmail));
    }
}
