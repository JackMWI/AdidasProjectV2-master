using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show_Control : MonoBehaviour
{
    public OSC Playback_osc;
    public OSC Camera_osc;

    /* Sort_1
     * Sends an OSC trigger to the Playback Server to play the first sorting outcome.
     */
    public void Sort_1()
    {
        string msg_addr = "/Playback/Sort/1";
        SendOSC_QuickToggle(msg_addr, Playback_osc);
    }

    /* Sort_2
     * Sends an OSC trigger to the Playback Server to play the second sorting outcome.
     */
    public void Sort_2()
    {
        string msg_addr = "/Playback/Sort/2";
        SendOSC_QuickToggle(msg_addr, Playback_osc);
    }

    /* Sort_3
     * Sends an OSC trigger to the Playback Server to play the third sorting outcome.
     */
    public void Sort_3()
    {
        string msg_addr = "/Playback/Sort/3";
        SendOSC_QuickToggle(msg_addr, Playback_osc);
    }


    /* Playback_Main
     * Sends an OSC trigger to the Playback Server to play the main video asset.
     */
    public void Playback_Main()
    {
        string msg_addr = "/Playback/Main/begin";
        SendOSC_QuickToggle(msg_addr, Playback_osc);
    }

    /* Take_Picture
     * Sends an OSC trigger to the Camera Server to take a photo and send it to API.
     */
    public void Take_Picture()
    {
        string msg_addr = "/Camera/Take";
        SendOSC_QuickToggle(msg_addr, Camera_osc);
    }

    private void SendOSC_QuickToggle(string msg_addr, OSC osc_service)
    {
        OscMessage message = new OscMessage();

        message.address = msg_addr;
        message.values.Add(1);
        osc_service.Send(message);
        StartCoroutine(ToggleBack(msg_addr, osc_service));
    }



    IEnumerator ToggleBack(string msg_addr, OSC osc)
    {
        yield return new WaitForSeconds(0.1f);
        OscMessage message = new OscMessage();

        message.address = msg_addr;
        message.values.Add(0);
        osc.Send(message);
    }

}
