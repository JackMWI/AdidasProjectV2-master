using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SurveyPageSliders : SurveyPagePulsateContinueBase
{
    [Header("Manually Assigned Required References")]
    // Reference to each slider on the page.
    public Slider[] pageSliders;
    // Reference to the continue button.
    public Button continueButton;

    private bool hasHitContinue = false;

    private void Awake()
    {
        continueButton.onClick.AddListener(ButtonHitContinue);
    }

    private new void Update()
    {
        bool interactable = currentPageState == SurveyPageState.Displaying;
        continueButton.interactable = interactable;
        for (int i = 0; i < pageSliders.Length; i++)
        {
            pageSliders[i].interactable = interactable;
        }

        base.Update();
    }

    // Called by a button in the page's UI
    public void ButtonHitContinue()
    {
        hasHitContinue = true;
    }


    public override bool CheckIfPageCompleted()
    {
        return hasHitContinue && CheckIfCanContinue();
    }

    // Packs all of the information about this page's sliders into a string,
    // then packs that string into a quiz result collection
    // (NOTE 1/23/2019) This should probably be changed to
    // make a new quiz result for each slider instead of putting them all in one,
    // but there's no place in the survey with more than one slider soo.....
    public override QuizResultCollection GetPageResults()
    {
        StringBuilder output = new StringBuilder();
        for (int i = 0; i < pageSliders.Length - 1; i++)
        {
            output.Append(pageSliders[i].name + " = " + pageSliders[i].value + seperatorString);
        }
        output.Append(pageSliders[pageSliders.Length - 1].name + " = " + pageSliders[pageSliders.Length - 1].value);
        return new QuizResultCollection(new QuizPageResult(name, output.ToString()));
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
    }

    public override bool CheckIfCanContinue()
    {
        return true;
    }
}
