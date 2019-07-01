using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClientSurvey : MonoBehaviour
{
    // The scenes copy of ClientSurvey. Assigned on Awake()
    public static ClientSurvey _localInstance;


    public SurveyQuestion[] survey;
    // An integer list containing the current user's scores on the
    // survey. Gets reset everytime a new survey is started.
    // currentScores[0] = Predator score
    // currentScores[1] = Nemeziz score
    // currentScores[2] = X score
    public int[] currentScores = new int[3];
    // The name of the current user. Gets reset every time a new
    // survey is started.
    public string currentName = "";
    // The email of the current user. Gets reset every time a new
    // survey is started.
    public string currentEmail = "";
    // The cleat size of the current user. Gets reset every time a new
    // survey is started.
    public string currentCleatSize = "";
    public string currentAge = "";
    public string currentGender = "";
    // The index of the currently displayed question in the survey array.
    public int currentQuestionIndex = 0;
    public int currentQuestion2Index = 0;

    public Survey2Form currentSurvey2Form = null;

    // The current scene's scene controller. Assigned on Start()
    private ClientSceneController sceneController;
    

    private void Awake()
    {
        _localInstance = this;
    }

    private void Start()
    {
        sceneController = ClientSceneController._localInstance;
        ResetSurveyState();
    }

    // Resets the survey's state for new users.
    public void ResetSurveyState()
    {
        currentScores = new int[3];
        currentName = "";
        currentEmail = "";
        currentQuestionIndex = 0;
        currentQuestion2Index = 0;
        currentSurvey2Form = new Survey2Form();
    }

    

    // Called when the user selects an answer in the Survey UI
    public void SelectAnswer(SurveyAnswer answerInput)
    {
        currentScores[(int)answerInput.answerShoeType] += survey[currentQuestionIndex].pointWorth;
        DisplayNextQuestion();
    }

    public void DisplayNextQuestion()
    {
        currentQuestionIndex++;
        Debug.Log("BIgBUttons: " + currentQuestionIndex);
        if (currentQuestionIndex == 1)
        {
            ClientSceneController._localInstance.UIController.AnimGoToSurvey2(2);
            currentQuestion2Index = 2;
            return;
        }
        else if(currentQuestionIndex == 2)
        {
            ClientSceneController._localInstance.UIController.AnimGoToSurvey2(4);
            currentQuestion2Index = 4;
            return;
        }
        else
        {
            ClientSceneController._localInstance.UIController.AnimGoToSurvey2(3);
            currentQuestion2Index = 3;
            return;
        }
        sceneController.UIController.AnimGoToNextQuestion();

    }

    public void DisplayNextSurvey2Question()
    {
        currentQuestion2Index++;
        Debug.Log("Survey2: " + currentQuestion2Index);
        if(currentQuestion2Index == 3)
        {
            ClientSceneController._localInstance.UIController.AnimGoToQuestionsFrom2();
            return;
        }
        else if(currentQuestion2Index == 5)
        {
            ClientSceneController._localInstance.UIController.AnimGoToQuestionsFrom4();
            return;
        }
        else if(currentQuestion2Index == 4)
        {
            ClientSceneController._localInstance.UIController.AnimGoToSurvey2(5);
            currentQuestion2Index = 6;
            return;

        }
        else if(currentQuestion2Index == 1)
        {
            Debug.Log("SurveyFinished");
            ClientSceneController._localInstance.UIController.AnimGoToSurvey2(9);
            ClientSceneController._localInstance.FinishSurvey();
            return;
        }
        else if(currentQuestion2Index == 10)
        {
            ClientSceneController._localInstance.UIController.AnimGoToSurvey2(0);
            return;
        }
        else if(currentQuestion2Index > 10)
        {
            ClientSceneController._localInstance.UIController.AnimGoToSurvey2(10);
            return;
        }
        
        
        ClientSceneController._localInstance.UIController.AnimGoToSurvey2(currentQuestion2Index);
        if (currentQuestion2Index == 1)
        {
            currentQuestion2Index++;
        }
    }
}

[Serializable]
public class SurveyQuestion
{
    public string question;
    public int pointWorth = 1;
    public SurveyAnswer[] answers;
}

[Serializable]
public class SurveyAnswer
{
    public string answer;
    public Sprite image;
    public ShoeType answerShoeType;
}

public enum ShoeType
{
    P,      // Predator
    NMZ,    // Nemeziz
    X       // X
}

[Serializable]
public class Survey2Form
{
    // How highly the user would recommend adidas
    // Rated 1 - 10 through a slider
    public int reccomendationScore = -1;
    // A list of players that the user likes watching
    public List<string> likedPlayers;
    // The social media service that the user spends the most time on
    public List<string> socialMedia;
    // A list of the most important feature of the user's cleats
    public List<string> importantCleatFeatures;
    // Where the user shops
    public List<string> shoppingLocations;
    // Where the user heard about the cleats they want
    public List<string> cleatLearnSources;
    // How often the user buys cleats
    public string cleatBuyingRate = "null";
    // Where the user spends their time player soccer
    public List<string> soccerLocations;

    // Outputs "|reccomendationScore|cleatBuyingRate|[likedPlayers]|[socialMedia]|[importantCleatFeatures]|[shoppingLocations]|[cleatLearnSources]|[soccerLocation]"
    public override string ToString()
    {
        string output = "|";

        output += reccomendationScore + "|";
        output += cleatBuyingRate + "|";
        

        output += FormatList(likedPlayers) + "|";
        output += FormatList(socialMedia) + "|";
        output += FormatList(importantCleatFeatures) + "|";
        output += FormatList(shoppingLocations) + "|";
        output += FormatList(cleatLearnSources) + "|";
        output += FormatList(soccerLocations);

        return output;
    }
    
    public string FormatList(List<string> inp)
    {
        string output = "";

        output += "[";
        if (inp.Count > 0)
        {
            output += inp[0];
            for (int i = 1; i < inp.Count; i++)
            {
                output += ", " + inp[i];
            }
        }
        output += "]";

        return output;
    }

    public Survey2Form()
    {
        likedPlayers = new List<string>();
        importantCleatFeatures = new List<string>();
        shoppingLocations = new List<string>();
        cleatLearnSources = new List<string>();
        soccerLocations = new List<string>();
        socialMedia = new List<string>();
    }


    // Assigns a value of one of the above strings based off of an index.
    // The indexes are as follows:
    //
    // 0 - socialMedia
    // 1 - importantCleatFeatures
    // 2 - shoppingLocations
    // 3 - cleatLearnSource
    // 4 - cleatBuyingRate
    // 5 - soccerLocation
    // 6 - likedPlayers
    public void AddTickMark(bool tick, int tickIndex, string value)
    {
        if(tickIndex == 0)
        {
            if(tick)
            {
                socialMedia.Add(value);
            }
            else
            {
                socialMedia.Remove(value);
            }
        }
        else if (tickIndex == 1)
        {
            if (tick)
            {
                importantCleatFeatures.Add(value);
            }
            else
            {
                importantCleatFeatures.Remove(value);
            }
        }
        else if (tickIndex == 2)
        {
            if (tick)
            {
                shoppingLocations.Add(value);
            }
            else
            {
                shoppingLocations.Remove(value);
            }
        }
        else if (tickIndex == 3)
        {
            if (tick)
            {
                cleatLearnSources.Add(value);
            }
            else
            {
                cleatLearnSources.Remove(value);
            }
        }
        else if(tickIndex == 4)
        {
            cleatBuyingRate = value;
        }
        else if(tickIndex == 5)
        {
            if(tick)
            {
                soccerLocations.Add(value);
            }
            else
            {
                soccerLocations.Remove(value);
            }
        }
        else if(tickIndex == 6)
        {
            if(tick)
            {
                likedPlayers.Add(value);
            }
            else
            {
                likedPlayers.Remove(value);
            }
        }
        else
        {
            Debug.Log("Error!! Invalid tick index entered!");
        }
    }


    
}
