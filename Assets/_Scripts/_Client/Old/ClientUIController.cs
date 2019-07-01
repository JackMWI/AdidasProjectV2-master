using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientUIController : MonoBehaviour
{
    // The Animator used to control a large portion of the survey UI
    public Animator surveyAnimator;
    // An array of UI GameObjects that will can get enable / disabled by SwitchToUI()
    public GameObject[] UIObjects;
    // The UI Text used to display the current survey question
    public Text surveyQuestionText;
    // The UI Text used to display errors in the Data Entry UI (index 6)
    public Text dataEntryErrorText;
    // The Input field that users type their name into. Found in the Data Entry UI (index 6)
    public InputField surveyNameField;
    // The Input field that users type their email into. Found in the Data Entry UI (index 6)
    public InputField surveyEmailField;
    public Dropdown surveyCleatField;
    public Dropdown surveyGenderField;
    public InputField surveyAgeField;
    public InputField survey2AgeField;
    public Toggle surveyTermsAndConditionsToggle;
    // The Button object that the users presses to continue past the info selection screen.
    // Will get disabled / enabled based on surveyNameField and surveyEmailField's content
    public Button infoContinueButton;
    // An array of every toggle in the survey
    public Toggle[] toggles;
    // The satisfaction slider in survey2
    public Slider survey2Slider;
    // The CanvasRenderer of the button above
    public CanvasRenderer infoContinueButtonRend1, infoContinueButtonRend2;
    // The index to the currently displayed UI in UIObjects
    public int currentUIIndex = 0;
    // The IP to be used if the iPad needs to manually connect.
    // This is entered by a user in the UI only if the connection fails.
    public string manualConnectionIP = "";
    // The GameObject that contains the entire survey UI
    public GameObject surveyUIGameObject;
    // The transform that surveyButtons will be Instantiated under
    public Transform surveyButtonParentTransform;
    // The prefab that will be Instantiated for each survey answer.
    public GameObject surveyButtonPrefab;


    // A list of gameObjects containing the currently displayed surveyButtons.
    private List<SurveyAnswerButton> currentButtons = new List<SurveyAnswerButton>();
    // This scene's copy of ClientSurvey
    public ClientSurvey clientSurvey;
    // A coroutine that gets created upon entering the thank you screen.
    // Will be canceled if the user hits done before 5 seconds pass.
    private Coroutine currentThankYouCoroutine;

    private void Awake()
    {
        //clientSurvey = ClientSurvey._localInstance;
        surveyUIGameObject.SetActive(false);
    }

    // Updates the Survey UI to reflect the current question index
    // This includes setting surveyQuestionText.text and removing / instantiating buttons to the scene.
    public void UpdateSurveyUI()
    {
        for (int i = 0; i < currentButtons.Count; i++)
        {
            Destroy(currentButtons[i].gameObject);
        }
        currentButtons.RemoveRange(0, currentButtons.Count);
        if(clientSurvey.currentQuestionIndex >= clientSurvey.survey.Length)
        {
            Debug.Log("Tried to update to a survey question that does not exist");
            return;
        }
        surveyQuestionText.text = clientSurvey.survey[clientSurvey.currentQuestionIndex].question.ToUpper();
        for (int i = 0; i < clientSurvey.survey[clientSurvey.currentQuestionIndex].answers.Length; i++)
        {
            currentButtons.Add(Instantiate(surveyButtonPrefab, surveyButtonParentTransform).GetComponent<SurveyAnswerButton>());
            currentButtons[currentButtons.Count - 1].answerData = clientSurvey.survey[clientSurvey.currentQuestionIndex].answers[i];
        }
    }

    public void OnCleatDropdownSelect(int inp)
    {
        clientSurvey.currentCleatSize = surveyCleatField.options[inp].text;
    }

    public void OnGenderDropdownSelect(int inp)
    {

        clientSurvey.currentGender = surveyGenderField.options[inp].text;
    }

    public void DropdownVisualUpdate()
    {
        if(surveyCleatField.value == 0)
        {
            surveyCleatField.captionText.color = new Color(0.72f, 0.72f, 0.72f);
        }
        else
        {
            surveyCleatField.captionText.color = new Color(0.196f, 0.196f, 0.196f);
        }

        if (surveyGenderField.value == 0)
        {
            surveyGenderField.captionText.color = new Color(0.72f, 0.72f, 0.72f);
        }
        else
        {
            surveyGenderField.captionText.color = new Color(0.196f, 0.196f, 0.196f);
        }
        UpdateDataEntryContinueButtonState();
    }

    // Clears previous names and emails from the Data Entry UI (index 6)
    public void CleanDataEntryUI()
    {
        surveyNameField.text = "";
        surveyEmailField.text = "";
        dataEntryErrorText.text = "";
        survey2AgeField.text = "";
        surveyAgeField.text = "";
        surveyGenderField.value = 0;
        surveyCleatField.value = 0;
        UpdateDataEntryContinueButtonState();
        survey2Slider.value = 5;
        for(int i = 0; i < toggles.Length; i++)
        {
            toggles[i].isOn = false;
        }
    }

    // Sets the text value of dataEntryErrorText
    public void SetDataEntryError(string error)
    {
        dataEntryErrorText.text = error;
    }

    // Called by the UI when surveyNameField or surveyEmailField is changed
    // Also called by CleanDataEntryUI()
    // Updates the infoContinueButton's interactable property based on surveyNameField and surveyEmailField
    public void UpdateDataEntryContinueButtonState()
    {
        bool isInteractable = !string.IsNullOrEmpty(surveyNameField.text) &&
            !string.IsNullOrEmpty(surveyAgeField.text) &&
            surveyEmailField.text.Contains("@") && 
            surveyGenderField.value != 0 && 
            surveyCleatField.value != 0 &&
            surveyTermsAndConditionsToggle.isOn;

        /*Debug.Log("name " + !string.IsNullOrEmpty(surveyNameField.text));
        Debug.Log("age " + !string.IsNullOrEmpty(surveyAgeField.text));
        Debug.Log("email " + surveyEmailField.text.Contains("@"));
        Debug.Log("gender " + (surveyGenderField.value != 0));
        Debug.Log("cleat " + (surveyCleatField.value != 0));
        Debug.Log("terms " + surveyTermsAndConditionsToggle.isOn);*/

        infoContinueButton.interactable = isInteractable;
        if(isInteractable)
        {
            infoContinueButtonRend1.SetAlpha(1);
            infoContinueButtonRend2.SetAlpha(1);
        }
        else
        {
            infoContinueButtonRend1.SetAlpha(0.5f);
            infoContinueButtonRend2.SetAlpha(0.5f);
        }
    }

    // Sets the manualConnectionIP to inp
    // Called by the UI when the IP input field is changed.
    public void UpdateManualConnectionIP(string inp)
    {
        manualConnectionIP = inp;
    }

    // Transitions into the Survey UI
    public void EnterSurveyUI()
    {
        SwitchToUI(5);
        surveyUIGameObject.SetActive(true);
        UpdateSurveyUI();
        clientSurvey.ResetSurveyState();
        CleanDataEntryUI();
        surveyAnimator.Play("InSplashScreen");
    }

    // Transitions out of the Survey UI
    public void LeaveSurveyUI()
    {
        SwitchToUI(3);
    }

    // Starts the Go To Data Entry animation
    public void AnimGoToDataEntry()
    {
        surveyAnimator.Play("GoToDataEntry");
    }

    // Starts the Go To Questions animation
    public void AnimGoToQuestions()
    {
        UpdateSurveyUI();
        surveyAnimator.Play("GoToQuestions");
    }

    // Starts the Go To Next Question animation
    public void AnimGoToNextQuestion()
    {
        surveyAnimator.Play("GoToNextQuestion");
    }

    // Starts the Go To Complete animation
    public void AnimGoToComplete()
    {
        surveyAnimator.Play("GoToComplete");
        currentThankYouCoroutine = StartCoroutine(CompleteCountDown());
    }

    // Starts the Go To Splash animation
    public void AnimGoToSplash()
    {
        if(currentThankYouCoroutine != null)
        {
            StopCoroutine(currentThankYouCoroutine);
            currentThankYouCoroutine = null;
        }
        surveyAnimator.Play("GoToSplash");
    }

    public void AnimGoToQuestionsFrom2()
    {
        UpdateSurveyUI();
        Debug.Log("GOing to questions from 2");
        surveyAnimator.Play("Survey2-2toQuestions");
    }

    public void AnimGoToQuestionsFrom4()
    {
        UpdateSurveyUI();
        Debug.Log("GOing to questions from 4");
        surveyAnimator.Play("Survey2-4toQuestions");
    }

    public void AnimGoToSurvey2(int index)
    {
        Debug.Log("AnimGoTOSurvey2: " + index);
        if(index == 0)
        {
            surveyAnimator.Play("Survey2-8to0");
        }
        else if(index == 1)
        {
            surveyAnimator.Play("Survey2-" + 0 + "to" + 2);
        }
        else if(index == 2)
        {
            surveyAnimator.Play("Questions to Survey2_2");
        }
        else if(index == 4)
        {
            surveyAnimator.Play("Questions to Survey2_4");
        }
        else if(index == 3)
        {
            surveyAnimator.Play("Questions to Survey2_3");
        }
        else if(index == 5)
        {
            surveyAnimator.Play("Survey2-3to5");
        }
        else if(index > 6 && index < 10)
        {
            index--;
            surveyAnimator.Play("Survey2-" + (index - 1) + "to" + index);
        }
        else if(index >= 10)
        {
            
            surveyAnimator.Play("GoToComplete");
            ClientSceneController._localInstance.FinishSurvey();
            currentThankYouCoroutine = StartCoroutine(CompleteCountDown());
        }
        else
        {
            surveyAnimator.Play("Survey2-" + (index - 1) + "to" + index);
        }
        
    }


    // Called by AnimGoToComplete()
    // Starts a countdown in which the survey will automatically return to the 
    // splash screen if the continue button is not pressed within 5 seconds
    private IEnumerator CompleteCountDown()
    {
        yield return new WaitForSeconds(5);
        surveyAnimator.Play("GoToSplash");
    }

    // Sets the currently instantiated buttons' interactable state
    public void SetSurveyButtonInteraction(bool active)
    {
        Debug.Log("Setting button interaction to " + active);
        for(int i = 0; i < currentButtons.Count; i++)
        {
            currentButtons[i].SetButtonInteraction(active);
        }
    }

    // Enables the UI Gameobject and index inp in UIObjects.
    // This is used to effectivley turn on and off different menus in the UI.
    public void SwitchToUI(int inp)
    {
        currentUIIndex = inp;
        for (int i = 0; i < UIObjects.Length; i++)
        {
            UIObjects[i].SetActive(i == inp);
        }
        surveyUIGameObject.SetActive(false);
    }
}
