using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperOptionsController : MonoBehaviour
{
    public static bool boolDebugMode = false;

    public Toggle toggleDebugMode;

    private void Awake()
    {
        toggleDebugMode.isOn = boolDebugMode;
    }

    public void ContinueToApplication()
    {
        boolDebugMode = toggleDebugMode.isOn;
    }
}
