using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyAnswerButton2 : MonoBehaviour
{
    public Animator anim;
    public QuizAnswer answerData;
    public Button surveyButton;
    public Text buttonText;
    public Image buttonImage;

    public void SetupButton()
    {
        buttonText.text = answerData.answer.ToUpper();
        buttonImage.sprite = answerData.image;
        if(!surveyButton)
        {
            surveyButton = GetComponent<Button>();
        }
    }
}
