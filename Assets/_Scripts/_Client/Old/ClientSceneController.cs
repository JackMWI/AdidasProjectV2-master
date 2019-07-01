using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientSceneController : MonoBehaviour
{
    // This scene's instance of the SceneController. Instantiated on Awake()
    public static ClientSceneController _localInstance;


    // The Scene's UI Controller
    public ClientUIController UIController;
    // The Scene's Network Manager
    //public ClientNetworkManager networkManager;
    // The Scene's Client Survey
    public ClientSurvey clientSurvey;
    // The Scene's local client
    //public ClientClient currentClient;

    // The IP of the Ambassador iPad according to Ian's networking layout
    //private string ambassadorIP = "192.168.1.10";

    private void Awake()
    {

        _localInstance = this;

        // Makes the iPad's not fall asleep while this scene is open
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // Client connection gets started in OnApplicationPause()
        
    }

    private void Start()
    {
        BypassNetworkCode();
    }

    // Called when the application is paused or un-paused
    private void OnApplicationPause(bool pause)
    {
        Debug.Log("Application Paused = " + pause);
    }

    private void BypassNetworkCode()
    {
        UIController.SwitchToUI(0);
        UIController.AnimGoToSplash();
        UIController.SwitchToUI(2);
        UIController.EnterSurveyUI();
    }


    // Attempts connection to a manually entered IP
    // Called by a button in the UI
    public void ManuallyConnectToAmbassador()
    {
        Debug.Log("Trying to connect to ambassador?? This shouldn't happen...");
    }

    // Called by various buttons in the Scene
    // Stops any started server and returns to the main menu of the app
    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SplashScreen");
    }

    // Jumps to the start of the survey, and cleans out any old survey values in the process.
    // Called by a button on the survey splash screen
    public void StartSurvey()
    {
        clientSurvey.ResetSurveyState();
        UIController.CleanDataEntryUI();
        UIController.AnimGoToDataEntry();
    }

    // Called by the Continue button in the Data Entry UI (index 6)
    // Checks if the user has entered a name and email correctly, then either moves on
    // to the survey or stops and requests that the user enters their email and name again.
    public void ContinueToQuestions()
    {
        string errorText = "";
        if(string.IsNullOrEmpty(UIController.surveyNameField.text))
        {
            errorText += "Please enter your name.\n";
        }
        if(!UIController.surveyEmailField.text.Contains("@"))
        {
            errorText += "Please enter a valid email.";
        }

        if(errorText.Length > 0)
        {
            UIController.SetDataEntryError(errorText);
            return;
        }

        clientSurvey.currentName = UIController.surveyNameField.text;
        clientSurvey.currentEmail = UIController.surveyEmailField.text;
        clientSurvey.currentAge = UIController.surveyAgeField.text;
        clientSurvey.currentCleatSize = UIController.surveyCleatField.captionText.text;
        clientSurvey.currentGender = UIController.surveyGenderField.captionText.text;

        UIController.AnimGoToQuestions();
    }

    // Called at the end of a survey.
    // Packages the Survey results and sends them to the Ambassador iPad.
    // Also switches to the Survey Complete UI (index 7)
    public void FinishSurvey()
    {

        // The loop below traverses the currentScores array and finds the index with the highest score.
        // This method favors the first largest number it finds, so if there is a tie between two indexes,
        // then the first one it finds will be used.
        int shoeType = -1;
        int largest = -1;
        for(int i = 0; i < clientSurvey.currentScores.Length; i++)
        {
            if(clientSurvey.currentScores[i] > largest)
            {
                largest = clientSurvey.currentScores[i];
                shoeType = i;
            }
        }

        string dataString = "|" + clientSurvey.currentAge + "|" + clientSurvey.currentGender + "|" + clientSurvey.currentCleatSize + clientSurvey.currentSurvey2Form.ToString();




        Debug.Log(dataString);
        // Sends the survey results over the network to the Ambassador iPad
        HandleSurveyInformation(clientSurvey, shoeType, dataString);
    }

    private void HandleSurveyInformation(ClientSurvey surv, int shoeType, string dataString)
    {
        Debug.Log("\nTODO");
        Debug.Log("Handle " + surv.ToString());
        Debug.Log("Handled shoe type: " + shoeType);
        Debug.Log("Handle data string:\n" + dataString);
    }
}
