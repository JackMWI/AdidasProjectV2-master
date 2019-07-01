using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class devRandomColorMat : MonoBehaviour
{
    public Renderer rend;

    [Range(0, 1)]
    public float hueMin, hueMax = 1;
    [Range(0, 1)]
    public float saturationMin, saturationMax = 1;
    [Range(0, 1)]
    public float valueMin, valueMax = 1;

    public int ticker, tickerMax = 10;

    private void Update()
    {
        ticker++;
        if(ticker > tickerMax)
        {
            ticker = 0;
            rend.material.color = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ticker = 0;
            rend.material.color = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax);
        }
    }
}
