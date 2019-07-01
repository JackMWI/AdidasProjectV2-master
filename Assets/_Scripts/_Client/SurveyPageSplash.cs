using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyPageSplash : SurveyPageBase
{
    public Button startSurveyButton;
    private bool hasHitStart = false;

    private void Update()
    {
        startSurveyButton.interactable = currentPageState == SurveyPageState.Displaying;
    }

    private void Awake()
    {
        startSurveyButton.onClick.AddListener(ButtonOnClick);
    }

    public void ButtonOnClick()
    {
        hasHitStart = true;
    }

    public override bool CheckIfPageCompleted()
    {
        return hasHitStart;
    }

    public override QuizResultCollection GetPageResults()
    {
        return new QuizResultCollection(false);
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
    }
}
