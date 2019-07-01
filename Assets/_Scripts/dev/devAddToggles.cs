using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class devAddToggles : MonoBehaviour
{
    public Transform parent;
    public GameObject prefab;

    public string[] strings;

    private void Start()
    {
        foreach(string str in strings)
        {
            Instantiate(prefab, parent).GetComponentInChildren<UnityEngine.UI.Text>().text = str;
        }
    }
}
