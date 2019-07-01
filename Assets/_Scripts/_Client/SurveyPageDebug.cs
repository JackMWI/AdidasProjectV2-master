using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyPageDebug : SurveyPageBase
{
    public static bool inDebugMode = false;
    public static bool shouldWriteCSV;
    public static bool shouldSendCSVToServer;
    public static bool shouldUseDebugIP;
    public static string debugIP;

    public Toggle toggleWriteCSV;
    public Toggle toggleSendCSV;
    public Toggle toggleDebugIP;
    public InputField inputFieldDebugIP;
    public Button buttonContinue;



    private bool pageCompleted = false;

    public void Awake()
    {
        inDebugMode = true;
        buttonContinue.onClick.AddListener(ButtonOnClick);
    }

    public void ButtonOnClick()
    {
        pageCompleted = true;
    }

    public override bool CheckIfPageCompleted()
    {
        return pageCompleted;
    }

    public override QuizResultCollection GetPageResults()
    {
        return new QuizResultCollection();
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
        if(newPageState == SurveyPageState.LoadingOut)
        {
            shouldWriteCSV = toggleWriteCSV.isOn;
            shouldSendCSVToServer = toggleSendCSV.isOn;
            shouldUseDebugIP = toggleDebugIP.isOn;
            debugIP = inputFieldDebugIP.text;
        }
    }
}
