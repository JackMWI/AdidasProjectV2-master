using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSceneController : MonoBehaviour
{
    public Camera_commands cameraCommands;

    public void SendDebugCameraCommand()
    {
        cameraCommands.TakePicture("debug@debug.com", "Debug Jones", "|20|gender|15|10|cleatBuyingRate|[likedPlayers]|[socialMedia]|[importantCleatFeatures]|[shoppingLocations]|[cleatLearnSources]|[soccerLocation]");
    }

    public void ExitScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SplashScreen");
    }

    public void ChangeRes()
    {
        Screen.SetResolution(1080, 1440, false);
    }
}
