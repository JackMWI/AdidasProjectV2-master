using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SurveyPagePulsateContinueBase : SurveyPageBase
{
    [Header("ContinueButton Required References")]
    public Graphic flashContinueButton;
    public Color colorFlashOff = new Color(0.75f, 0.75f, 0.75f, 0.25f);
    public Color colorFlashOn = new Color(1, 1, 1, 1);
    public Color currentColor = Color.white;


    public abstract bool CheckIfCanContinue();

    public void Update()
    {
        if(CheckIfCanContinue())
        {
            currentColor = Color.Lerp(colorFlashOff, colorFlashOn, Mathf.Abs(Mathf.Sin(Time.time)));
        }
        else
        {
            currentColor = colorFlashOff;
        }
        flashContinueButton.color = Color.Lerp(flashContinueButton.color, currentColor, Time.deltaTime * 40);
    }
}
