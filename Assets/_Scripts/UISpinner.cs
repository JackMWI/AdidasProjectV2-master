using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpinner : MonoBehaviour
{
    public float spinSpeed = 5;

    private void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, Vector3.forward);
    }
}
