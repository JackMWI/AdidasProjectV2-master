using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class is kind of a weird merge between the old survey system and the new one.
// It's purpose is to display standard quiz questions with an image, allowing the user to
// pick only one of them before moving on.
public class SurveyPageQuizQuestion : SurveyPageBase
{
    [Header("Manually Assigned Required References")]
    // A class containing information about the question asked on this page.
    // Needs to be manually filled out!!
    public QuizQuestion surveyQuestion;
    // The prefab that will be Instantiated for each survey answer.
    public GameObject surveyButtonPrefab;
    // The transform that surveyButtons will be Instantiated under.
    public Transform surveyButtonParentTransform;
    // Reference to the UI text used to display the survey question.
    public Text surveyQuestionText;

    // A list of gameObjects containing the currently displayed surveyButtons.
    private SurveyAnswerButton2[] currentButtons;
    private QuizAnswer chosenOption = null;

    private void Start()
    {
        SetSurveyUI();
    }

    // Sets up this pages UI to reflect the data entered in surveyQuestion.
    private void SetSurveyUI()
    {
        surveyQuestionText.text = surveyQuestion.question.ToUpper();
        currentButtons = new SurveyAnswerButton2[surveyQuestion.answers.Length];
        for (int i = 0; i < surveyQuestion.answers.Length; i++)
        {
            currentButtons[i] = Instantiate(surveyButtonPrefab, surveyButtonParentTransform).GetComponent<SurveyAnswerButton2>();
            currentButtons[i].answerData = surveyQuestion.answers[i];
            currentButtons[i].SetupButton();

            // Makes a new integer equal to the value of i (THIS IS IMPORTANT!!!)
            int index = i;
            // Tells the current button to call the method ButtonOnClick, and pass in the integer index.
            // For some reason when you define a delegate like this, it passes the memory address or something
            // of the parameter (in this case, index). So if you were to pass it just i, it would freak out
            // later because after this loop ends i is equal to surveyQuestion.answers.Length !
            currentButtons[i].surveyButton.onClick.AddListener(delegate { ButtonOnClick(index); });
        }
    }

    private void Update()
    {
        bool interactable = currentPageState == SurveyPageState.Displaying;

        for (int i = 0; i < currentButtons.Length; i++)
        {
            currentButtons[i].surveyButton.interactable = interactable;
        }
    }

    // A method automatically assigned to be called by the buttons instantiated on this page.
    // It passes in an integer that refers to the index of the button being pressed,
    // so that it knows which button has been selected.
    public void ButtonOnClick(int buttonIndex)
    {
        chosenOption = currentButtons[buttonIndex].answerData;
        SetPageState(SurveyPageState.LoadingOut);
        Update();
    }

    public override bool CheckIfPageCompleted()
    {
        return chosenOption != null;
    }

    public override QuizResultCollection GetPageResults()
    {
        return new QuizResultCollection(new QuizPageResult(quizResultName, chosenOption.answer.ToLower()));
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
    }

    public override QuizAtribute GetQuizEffects()
    {
        return new QuizAtribute(chosenOption.answerAttributeType, surveyQuestion.pointWorth);
    }
}
