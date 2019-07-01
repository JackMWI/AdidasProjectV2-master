using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

public class Client2UIController : MonoBehaviour
{
    public static Client2UIController selfReference;

    [Header("Required References")]
    // The animator that this will use to move around the animation transform.
    public Animator animator;
    // The TCPClient that will be used to send survey results to a TCP server.
    public TCPClient2 networkController;
    // The transform that new pages will be spawned under. Gets moved
    // arround by the animator to create the illusion of moving pages.
    public Transform animationTransform;
    // UI to enable / disable when data is sending.
    public GameObject sendingDataUI;
    // An array of prefabs that represent each survey page. These will be cycled
    // through in order to create the survey.
    public GameObject[] surveyPagePrefabs;

    // The currently displayed surveyPage
    private SurveyPageBase currentSurveyPage;



    private void Awake()
    {
        selfReference = this;
        sendingDataUI.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(SurveyLoop());
    }

    // The main loop running the survey system.
    // Displays a survey, waits for it to complete, waits 2 seconds, then loops.
    IEnumerator SurveyLoop()
    {
        while (true)
        {
            yield return DisplaySurveySequence();
            yield return new WaitForSeconds(2);
        }
    }


    // This is where the bulk of the work is done. This coroutine is more or less the brain of this script.
    // Calling this couroutine runs and displays a copy of the survey.
    IEnumerator DisplaySurveySequence()
    {
        // A dictionary that contains each survey result. This will get added to based on what
        // each page returns as a result.
        Dictionary<string, string> surveyResults = new Dictionary<string, string>();

        // An array that keeps track of the user's current quiz score. Each index refers to the current
        // score of a QuizAttributeType.
        //
        // For example:
        // quizScores[0] is the current number of points with type None (essentially a null type)
        // quizScores[1] is the current number of points with type Predator
        // quizScores[2] is the current number of points with type Nemeziz
        // quizScores[3] is the current number of points with type X
        int[] quizScores = new int[Enum.GetNames(typeof(QuizAttributeType)).Length];

        sendingDataUI.SetActive(false);

        // Loops through each survey page prefab provided, and creates / destroys each one as the
        // user goes through the survey.
        for(int i = 0; i < surveyPagePrefabs.Length; i++)
        {
            // Creates a page
            GameObject currentPageObject = Instantiate(surveyPagePrefabs[i], animationTransform);
            Debug.Log("Displaying survey page " + i + " | " + currentPageObject.name);

            // Gets a reference to the page component
            currentSurveyPage = currentPageObject.GetComponent<SurveyPageBase>();
            // Tells the page that it's loading in
            currentSurveyPage.SetPageState(SurveyPageState.LoadingIn);
            // Sends the quiz scores to the page in case it needs them (only really required for the results page)
            currentSurveyPage.SendSurveyResults(surveyResults);

            // Plays the loading in animation, waits for it to finish, then tells the page that it's being displayed.
            animator.Play("PageLoadIn");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.1f);
            currentSurveyPage.SetPageState(SurveyPageState.Displaying);

            // Waits for the page to report that it has been completed
            while (!currentSurveyPage.CheckIfPageCompleted())
            {
                yield return new WaitForEndOfFrame();
            }

            // Tells the page that it's loading out
            currentSurveyPage.SetPageState(SurveyPageState.LoadingOut);

            // Gets the page results from the page, then adds them to surveyResults.
            // It does this in a for loop because GetPageResults returns a list of results.
            // Each page could potentially return more than one result (for example, the initial data entry page.
            List<QuizPageResult> resultCol = currentSurveyPage.GetPageResults().list;
            if(resultCol != null)
            {
                for (int z = 0; z < resultCol.Count; z++)
                {
                    surveyResults[resultCol[z].resultName] = resultCol[z].pageResult;
                }
            }
            

            // Gets the quiz results from the page. This determines how the quiz score is effected.
            // Most pages wont do anything, but the Quiz Question pages will return a QuizAttribute
            // that effects the user's quiz score.
            QuizAtribute attr = currentSurveyPage.GetQuizEffects();
            quizScores[(int)attr.type] += attr.weight;

            // Plays the loading out animation, waits for it to finish, then destroys the current page.
            animator.Play("PageLoadOut");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.1f);
            Destroy(currentPageObject);
        }

        Debug.Log("Done!");
        // Handles the results of the survey.
        HandleSurveyResults(surveyResults);
    }

    
    // Called at the end of each survey.
    private void HandleSurveyResults(Dictionary<string, string> surveyResults)
    {
        // hack to debug shit
        if(SurveyPageDebug.inDebugMode)
        {
            if(SurveyPageDebug.shouldWriteCSV)
            {
                WriteResultCSV(surveyResults);
            }
            if(SurveyPageDebug.shouldSendCSVToServer)
            {
                SendResultsToServer(surveyResults);
            }
        }
        else
        {
            WriteResultCSV(surveyResults);
            SendResultsToServer(surveyResults);
        }
        
    }

    // Takes in survey results, converts them to a JSON-like format, then
    // sends them to a server using the networkController.
    private void SendResultsToServer(Dictionary<string, string> surveyResults)
    {
        StartCoroutine(_SendResultsToServerRoutine(surveyResults));
    }

    private IEnumerator _SendResultsToServerRoutine(Dictionary<string, string> surveyResults)
    {
        sendingDataUI.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        Dictionary<string, string> dictToGetJSONd = new Dictionary<string, string>();
        foreach (string key in surveyResults.Keys)
        {
            dictToGetJSONd[key] = ProcessSurveyResultString(key, surveyResults[key]);
        }
        string resultJson = JsonConvert.SerializeObject(dictToGetJSONd);

        Debug.Log(resultJson);

        // Sends line1 and line2 over the network through the TCPClient
        bool sentCorrectly = networkController.SendMessageOverNetwork(resultJson);
        if (!sentCorrectly)
        {
            Debug.Log("INFORMATION NOT SENT OVER NETWORK!!! THERE WAS AN ERROR");
        }

        string basePath = Application.persistentDataPath;
        string folderPath = "surveyData";
        string folderDirectory = Path.Combine(basePath, folderPath);

        // Creates the filename based off of the current date and time
        string fileName = string.Format("quizResult ({0}).json", DateTime.Now.ToString("yyyy MM dd HH mm ss ffff"));
        string filePath = Path.Combine(folderDirectory, fileName);
        Debug.Log(filePath);

        // Creates the save directory if it doesn't exist
        if (!Directory.Exists(folderDirectory))
        {
            Directory.CreateDirectory(folderDirectory);
        }


        // Creates the file at the desired filepath
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.Write(resultJson);
        writer.Close();
    }


    // Processes strings that are JSON'd in the method above.
    // This method may manually catch and change some results (etc quiz results)
    private string ProcessSurveyResultString(string resultName, string result)
    {
        string trimmedResult = result.Replace("\"", "");
        //trimmedResult = trimmedResult.Replace(" ", "_");
        if (resultName == "quiz result")
        {
            if(trimmedResult == "none")
            {
                return "0";
            }
            else if (trimmedResult == "predator")
            {
                return "1";
            }
            else if (trimmedResult == "nemeziz")
            {
                return "2";
            }
            else if (trimmedResult == "x")
            {
                return "3";
            }
        }
        return trimmedResult;
    }

    // This method takes in a list of QuizPageResults and writes them to a CSV file at
    // Application.datapath\surveyData\quizResult (yyyy MM dd HH mm ss ffff)
    //      (NOTE: this location changes on a platform by platform basis. The stuff in
    //       the parenthesis at the end is a datetime formatted to a string)
    //
    private void WriteResultCSV(Dictionary<string, string> surveyResults)
    {
        string basePath = Application.persistentDataPath;
        string folderPath = "surveyData";
        string folderDirectory = Path.Combine(basePath, folderPath);
        Debug.Log(folderDirectory);

        // Creates the filename based off of the current date and time
        string fileName = string.Format("quizResult ({0}).csv", DateTime.Now.ToString("yyyy MM dd HH mm ss ffff"));
        string filePath = Path.Combine(folderDirectory, fileName);
        Debug.Log(filePath);

        // Creates the save directory if it doesn't exist
        if (!Directory.Exists(folderDirectory))
        {
            Directory.CreateDirectory(folderDirectory);
        }

        
        // Creates the file at the desired filepath
        StreamWriter writer = new StreamWriter(filePath, true);

        string line1 = "";
        string line2 = "";
        bool hasAddedSeperator = false;
        // Loops through surveyResults and formats them in CSV style.
        // This more or less means that it adds commas between each value.
        foreach(string key in surveyResults.Keys)
        {
            if (!hasAddedSeperator)
            {
                hasAddedSeperator = true;
            }
            else
            {
                line1 += ",";
                line2 += ",";
            }

            line1 += key;
            line2 += surveyResults[key];
        }

        // Writes the created strings to the device then closes the streamwriter.
        writer.WriteLine(line1);
        writer.WriteLine(line2);
        writer.Close();

        Debug.Log("Wrote quizdata to file " + filePath);
    }
}
