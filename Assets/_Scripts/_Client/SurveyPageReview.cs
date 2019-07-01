using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyPageReview : SurveyPagePulsateContinueBase
{
    public InputField nameField;
    public Dropdown genderDropdown;
    public Dropdown shoeSizeDropdown;
    public Dropdown shoeVariantDropdown;
    public Button hiddenButton;

    public string playerName = "";
    public string playerGender = "";
    public string playerShoeSize = "";
    public string playerShoeVariant = "";

    // Stores if the page is completed. Each time the continue button
    // is pressed, it assigns this to the value of CanContinue().
    private bool hasHitContinue = false;

    private float previousTapTime = 0;
    private int tapCount = 0;


    public void ButtonOnHiddenTap()
    {
        if(Time.time - previousTapTime > 2)
        {
            tapCount = 0;
        }
        tapCount++;

        if(tapCount > 5)
        {
            hasHitContinue = true;
        }

        previousTapTime = Time.time;
    }

    public override bool CheckIfCanContinue()
    {
        return CanContinue();
    }

    public override bool CheckIfPageCompleted()
    {
        return hasHitContinue;
    }

    public override QuizResultCollection GetPageResults()
    {
        List<QuizPageResult> output = new List<QuizPageResult>();

        output.Add(new QuizPageResult("name", nameField.text.ToLower()));
        output.Add(new QuizPageResult("gender", genderDropdown.options[genderDropdown.value].text.ToLower()));
        output.Add(new QuizPageResult("shoe size", shoeSizeDropdown.options[shoeSizeDropdown.value].text));
        output.Add(new QuizPageResult("quiz result", shoeVariantDropdown.options[shoeVariantDropdown.value].text));

        return new QuizResultCollection(output);
    }

    public override void SendSurveyResults(Dictionary<string, string> surveyResults)
    {
        playerName = GetFromDict(surveyResults, "name");
        playerGender = GetFromDict(surveyResults, "gender");
        playerShoeSize = GetFromDict(surveyResults, "shoe size");
        playerShoeVariant = GetFromDict(surveyResults, "quiz result");

        nameField.text = playerName;
        genderDropdown.value = FindIndexInDropdown(genderDropdown, playerGender);
        shoeSizeDropdown.value = FindIndexInDropdown(shoeSizeDropdown, playerShoeSize);
        shoeVariantDropdown.value = FindIndexInDropdown(shoeVariantDropdown, playerShoeVariant);
    }

    private int FindIndexInDropdown(Dropdown dropDown, string text)
    {
        text = text.Replace("\"", "");
        for(int i = 0; i < dropDown.options.Count; i++)
        {
            if (dropDown.options[i].text.ToLower().Equals(text.ToLower()))
            {
                Debug.Log("Found " + text);
                return i;
            }
        }
        Debug.Log("COULD NOT FIND " + text);
        return -1;
    }

    private string GetFromDict(Dictionary<string, string> surveyResults, string name)
    {
        string output = "ERR";
        if(surveyResults.ContainsKey(name))
        {
            output = surveyResults[name];
        }
        else
        {
            Debug.Log("ERROR!! Tried to get element from surveyResults that doesn't exist? (surveyResults does not have key " + name + ").");
        }

        return output;
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
    }


    // Called by a button in the page's UI
    public void ButtonHitContinue()
    {
        hasHitContinue = CanContinue();
    }

    // Called by ButtonHitContinue() & CheckIfCanContinue()
    // Determines if the user has completed everything they need to on the page
    private bool CanContinue()
    {
        bool output = true;
        output = output && !string.IsNullOrEmpty(nameField.text.Trim());
        output = output && shoeSizeDropdown.value != 0;
        output = output && genderDropdown.value != 0;

        return output;
    }
}
