using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenSceneController : MonoBehaviour
{
    public Animator anim; 

    public string ambassadorSceneName = "Ambassador";
    public string clientSceneName = "Client";
    public string debugSceneName = "Debug";
    
    private int debugCounter = 0;

    private void Awake()
    {
        // Makes the iPad's not fall asleep while this scene is open
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void ChooseAmbassador()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ambassadorSceneName);
    }

    public void ChooseClient()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(clientSceneName);
    }

    public void ChooseDebug()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(debugSceneName);
    }

    public void TapDebugButton()
    {
        debugCounter++;

        if(debugCounter >= 10)
        {
            debugCounter = 0;

            anim.SetTrigger("ChooseDebug");
        }
    }
}
