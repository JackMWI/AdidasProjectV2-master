using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Survey2Buttons : MonoBehaviour
{
    public Toggle[] toggleGroup;
    public InputField[] inputGroup;

    public void ContinueSlider()
    {
        ClientSurvey._localInstance.DisplayNextSurvey2Question();
    }

    public void ContinueToggles()
    {
        for(int i = 0; i < toggleGroup.Length; i++)
        {
            if(toggleGroup[i].isOn)
            {
                ClientSurvey._localInstance.DisplayNextSurvey2Question();
                return;
            }
        }


    }

    public void ContinueInputFields()
    {
        for (int i = 0; i < inputGroup.Length; i++)
        {
            if (string.IsNullOrEmpty(inputGroup[i].text))
            {
                return;
            }
        }

        ClientSurvey._localInstance.DisplayNextSurvey2Question();
    }
}
