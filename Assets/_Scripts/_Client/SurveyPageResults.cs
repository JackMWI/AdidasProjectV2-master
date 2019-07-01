using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyPageResults : SurveyPageBase
{
    public Text resultText;
    public Button continueButton;

    private bool pressedContinue = false;
    private QuizAttributeType quizResult;

    private void Awake()
    {
        continueButton.onClick.AddListener(ButtonOnClick);
    }

    public void ButtonOnClick()
    {
        pressedContinue = true;
    }

    public override bool CheckIfPageCompleted()
    {
        return pressedContinue;
    }

    public override QuizResultCollection GetPageResults()
    {
        return new QuizResultCollection(new QuizPageResult("quiz result", GetShoeName().ToLower()));
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
    }



    // DEPRECATED METHOD - NO LONGER USED
    //   ONLY HERE IN CASE I CHANGE MY MIND :)
    //
    // Recieves the current user's quiz scores from Client2UIController.
    // Uses the scores to find the first score with the highest value,
    // then uses that as a result (unless the user manually picks a
    // different shoe).
    /*public override void SendScores(int[] scores)
    {
        QuizAttributeType currentMax = QuizAttributeType.None;
        int max = 0;
        for(int i = 0; i < scores.Length; i++)
        {
            if(scores[i] > max)
            {
                max = scores[i];
                currentMax = (QuizAttributeType)i;
            }
        }

        HandleQuizResult(currentMax);
    }*/

    // Called by SendScores() & ButtonPickSpecificShoe()
    // Takes in a quiz result and makes that the user's result!
    //
    private void HandleQuizResult(QuizAttributeType resultIn)
    {
        quizResult = resultIn;

        string resultString = GetShoeName() + "!";

        resultText.text = resultString;
    }

    // Creates a string from the current quiz result.
    private string GetShoeName()
    {
        if (quizResult == QuizAttributeType.NMZ)
        {
            return "Nemeziz";
        }
        else if (quizResult == QuizAttributeType.P)
        {
            return "Predator";
        }
        else if (quizResult == QuizAttributeType.X)
        {
            return "X";
        }

        return "ERROR";
    }

    // Called by 3 buttons in the UI. Manually picks out a specific shoe,
    // Overriding the user's quiz score.
    public void ButtonPickSpecificShoe(int shoeIndex)
    {
        HandleQuizResult((QuizAttributeType)shoeIndex);
    }
}
