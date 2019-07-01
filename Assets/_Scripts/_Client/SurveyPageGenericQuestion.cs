using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


public class SurveyPageGenericQuestion : SurveyPageBase
{
    [Header("Required References")]
    public Button[] buttons;

    private string chosenOption = "";

    private void Awake()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            // this next line seems redundant, but it's REQUIRED in order for the
            // delegate to work correctly... if you don't make a new int, it will for
            // some reason use a pointer to i instead of i's value.
            int number = i;
            buttons[number].onClick.AddListener(delegate { ButtonsOnClick(number); });
        }
    }

    private void Update()
    {
        bool interactable = currentPageState == SurveyPageState.Displaying;

        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = interactable;
        }
    }

    public void ButtonsOnClick(int buttonIndex)
    {
        chosenOption = buttons[buttonIndex].name;
        Update();
    }

    public override bool CheckIfPageCompleted()
    {
        return !string.IsNullOrEmpty(chosenOption);
    }

    public override QuizResultCollection GetPageResults()
    {
        return new QuizResultCollection(new QuizPageResult(name, chosenOption));
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
    }
}
