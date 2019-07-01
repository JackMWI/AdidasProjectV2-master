using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorEvents : MonoBehaviour
{
    public UnityEvent event1, event2, event3;

    public void CallEvent1()
    {
        //Debug.Log("--calling event 1");
        event1.Invoke();
    }

    public void CallEvent2()
    {
        //Debug.Log("--calling event 2");
        event2.Invoke();
    }

    public void CallEvent3()
    {
        //Debug.Log("--calling event 3");
        event3.Invoke();
    }
}
