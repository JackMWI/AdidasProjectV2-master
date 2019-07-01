using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class devSurvey1Button : MonoBehaviour
{
    public devSurvey1Answer answerData;
    public UnityEngine.UI.Text buttonText;

    private void Start()
    {
        buttonText.text = answerData.answer;
    }

    public void Click()
    {
        devSurveyTest1._localInstance.SelectAnswer(answerData);
    }
}
