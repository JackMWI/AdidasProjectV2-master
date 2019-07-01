using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class devFormDisplay2 : MonoBehaviour
{
    public devForm2 formData;
    public Text nameDisplay, colorDisplay;

    public void UpdateUI()
    {
        nameDisplay.text = formData.name;
        colorDisplay.text = formData.color;
    }
}
