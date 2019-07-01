using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyAnswerButton : MonoBehaviour
{
    public Animator anim;
    public SurveyAnswer answerData;
    public Button surveyButton;
    public Text buttonText;
    public Image buttonImage;

    private bool hasBeenPressed = false;

    private void Start()
    {
        buttonText.text = answerData.answer.ToUpper();
        buttonImage.sprite = answerData.image;
        if(!surveyButton)
        {
            surveyButton = GetComponent<Button>();
        }
        SetButtonInteraction(false);
    }

    // Called by the UIController
    // Sets this button's interactable bool to inp
    public void SetButtonInteraction(bool inp)
    {
        
        surveyButton.interactable = inp;

        if(!hasBeenPressed)
        {
            anim.SetTrigger("Disabled");
            foreach(CanvasRenderer rend in GetComponentsInChildren<CanvasRenderer>())
            {
                if(inp)
                {
                    rend.SetAlpha(1f);
                }
                else
                {
                    rend.SetAlpha(0.5f);
                }
            }
        }
    }

    // Called when this button is tapped
    // Makes sure that this button was ACTUALLY supposed to get clicked
    // Also disables the other buttons
    // Also sends the selected answer to this scene's ClientSurvey instance
    public void OnClick()
    {
        if(!surveyButton.interactable)
        {
            return;
        }
        hasBeenPressed = true;
        anim.SetTrigger("Pressed");
        ClientSceneController._localInstance.UIController.SetSurveyButtonInteraction(false);
        ClientSurvey._localInstance.SelectAnswer(answerData);
    }
}
