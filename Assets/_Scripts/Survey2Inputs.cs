using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Survey2Inputs : MonoBehaviour
{

    public Text displayedText;
    public Toggle toggle;
    public Slider slider;
    public int tickIndex = -1;
    

    private void Awake()
    {
        if(displayedText == null)
        {
            displayedText = GetComponentInChildren<Text>();
        }
        if(toggle == null)
        {
            toggle = GetComponentInChildren<Toggle>();
        }
        

        if(toggle != null)
        {
            toggle.onValueChanged.AddListener(OnTick);
        }
    }

    public void OnTick(bool inp)
    {
        ClientSurvey._localInstance.currentSurvey2Form.AddTickMark(inp, tickIndex, displayedText.text);
    }

    public void OnSliderChange()
    {
        ClientSurvey._localInstance.currentSurvey2Form.reccomendationScore = (int)slider.value;
    }

    public void OnAgeEntry(string inp)
    {
        if(string.IsNullOrEmpty(inp))
        {
            return;
        }
        //ClientSurvey._localInstance.currentSurvey2Form.age = int.Parse(inp);
    }

}
