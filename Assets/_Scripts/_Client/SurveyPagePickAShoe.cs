using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyPagePickAShoe : SurveyPagePulsateContinueBase
{
    public Toggle[] toggles;
    public Button continueButton;
    public string selectedShoe = "";

    private bool canContinue = false;


    private void Awake()
    {
        continueButton.onClick.AddListener(ButtonAttemptContinue);

    }

    public void OnToggleChanged()
    {
        for(int i = 0; i < toggles.Length; i++)
        {
            if(toggles[i].isOn)
            {
                selectedShoe = toggles[i].name;
                return;
            }
        }
    }

    public void ButtonAttemptContinue()
    {
        canContinue = !string.IsNullOrEmpty(selectedShoe);
    }

    public override bool CheckIfPageCompleted()
    {
        return canContinue;
    }

    public override QuizResultCollection GetPageResults()
    {
        return new QuizResultCollection(new QuizPageResult("quiz result", GetShoeName().ToLower()));
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
    }

    // Creates a string from the current quiz result.
    private string GetShoeName()
    {
        return selectedShoe;
    }

    public override bool CheckIfCanContinue()
    {
        return !string.IsNullOrEmpty(selectedShoe);
    }
}
