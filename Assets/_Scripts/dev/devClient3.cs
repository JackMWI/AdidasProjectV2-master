using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class devClient3 : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        devTestNetworkingSceneManager3._localInstance.currentClient = this;
    }

    [Command]
    public void CmdCallDevDing()
    {
        devTestNetworkingSceneManager3._localInstance.DevDing();
    }

    [Command]
    public void CmdSendFormInfo(string currentName, string currentColor, string currentEmail)
    {
        devTestNetworkingSceneManager3._localInstance.AddNewFormInfo(new devForm2(currentName, currentColor, currentEmail));
    }
}
