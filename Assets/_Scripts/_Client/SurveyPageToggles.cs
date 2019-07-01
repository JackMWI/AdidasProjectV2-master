using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SurveyPageToggles : SurveyPagePulsateContinueBase
{
    [Header("Manually Assigned Required References")]
    // Reference to each toggle on the page
    public Toggle[] pageToggles;
    // Reference to the continue button
    public Button continueButton;

    [Header("Continue Flash Options")]
    public bool requiresOneSelection = false;

    private bool hasHitContinue = false;

    private void Awake()
    {
        continueButton.onClick.AddListener(ButtonHitContinue);
    }

    private new void Update()
    {
        bool interactable = currentPageState == SurveyPageState.Displaying;
        continueButton.interactable = interactable;
        for (int i = 0; i < pageToggles.Length; i++)
        {
            pageToggles[i].interactable = interactable;
        }

        base.Update();
    }

    public void ButtonHitContinue()
    {
        hasHitContinue = CheckIfCanContinue();
    }

    public override bool CheckIfPageCompleted()
    {
        return hasHitContinue && CheckIfCanContinue();
    }

    // Takes all of the toggles on this page marked true, packs them into a string,
    // and returns them as a quiz page result.
    // Remember: it only packs the ones marked true! This is to save space / make it
    // more readable.
    //
    // Seperates each true value with a comma.
    public override QuizResultCollection GetPageResults()
    {
        StringBuilder output = new StringBuilder();
        bool hasAddedSeperator = false;
        for(int i = 0; i < pageToggles.Length - 1; i++)
        {
            if(pageToggles[i].isOn)
            {
                if(!hasAddedSeperator)
                {
                    hasAddedSeperator = true;
                    output.Append(pageToggles[i].name.ToLower());
                }
                else
                {
                    output.Append(seperatorString + pageToggles[i].name.ToLower());
                }
            }
        }

        return new QuizResultCollection(new QuizPageResult(quizResultName, output.ToString()));
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
    }

    public override bool CheckIfCanContinue()
    {
        bool output = true;

        if(requiresOneSelection)
        {
            bool oneOn = false;
            for (int i = 0; i < pageToggles.Length; i++)
            {
                oneOn = oneOn || pageToggles[i].isOn;
            }

            output = output && oneOn;
        }


        return output;
    }
}
