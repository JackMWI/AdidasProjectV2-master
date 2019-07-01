using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyPageComplete : SurveyPageBase
{
    public UnityEngine.UI.Button thankYouButtonOverride;
    private const float thankYouDecay = 5;
    private float thankYouTimer = 0;

    private bool pageCompleted = false;

    private void Awake()
    {
        thankYouButtonOverride.onClick.AddListener(ButtonOnClick);
    }

    private void Update()
    {
        thankYouButtonOverride.interactable = currentPageState == SurveyPageState.Displaying;
        thankYouTimer += Time.deltaTime;
        if(thankYouTimer >= thankYouDecay)
        {
            pageCompleted = true;
        }
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
        return new QuizResultCollection(false);
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
    }
}
