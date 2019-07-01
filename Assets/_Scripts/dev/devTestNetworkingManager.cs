using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class devTestNetworkingManager : MonoBehaviour
{
    public static devTestNetworkingManager _localInstance;
    public devClient1 currentClient = null;

    public GameObject[] UIObjects;
    public GameObject connectedClientParent;
    public GameObject clientIndicators;
    public GameObject localClientIndicator;

    public GameObject prefabConnectedClient;
    public GameObject prefabFoundPlayer;
    public NetworkDiscovery netDisc;
    public NetworkManager netManager;

    public bool isHost = false;

    public AudioSource audioSource;
    public AudioClip devClip;
    
    private List<GameObject> connectedIndicators = new List<GameObject>();

    private void Awake()
    {
        _localInstance = this;
        SwitchToUI(0);
    }







    public void CreateClientConnectedIndicator(Color col)
    {
        GameObject newIndicator = Instantiate(prefabConnectedClient, connectedClientParent.transform);
        newIndicator.GetComponent<UnityEngine.UI.Image>().color = col;
        connectedIndicators.Add(newIndicator);
    }

    public void StartServer()
    {
        if(!netDisc.Initialize())
        {
            SwitchToUI(0);
            return;
        }
        SwitchToUI(1);
        netDisc.StartAsServer();
        netManager.StartHost();
        SwitchToUI(3);
        clientIndicators.SetActive(true);
        localClientIndicator.SetActive(false);
        isHost = true;
    }

    public void AttemptToConnectToServer()
    {
        if (!netDisc.Initialize())
        {
            SwitchToUI(0);
            return;
        }
        SwitchToUI(2);
        netDisc.StartAsClient();
        Debug.Log("Attempting To connect...");
    }

    public void SuccessfullyConnectedToServer(GameObject newClient)
    {
        if(isHost)
        {
            return;
        }
        Debug.Log("Connected To Client!!");
        currentClient = newClient.GetComponent<devClient1>();
        clientIndicators.SetActive(false);
        localClientIndicator.SetActive(true);
        localClientIndicator.GetComponentInChildren<UnityEngine.UI.Image>().color = currentClient.currentColor;
        SwitchToUI(4);
    }

    public void SwitchToUI(int inp)
    {
        for(int i = 0; i < UIObjects.Length; i++)
        {
            UIObjects[i].SetActive(i == inp);
        }
    }

    public void CallDevDing()
    {
        currentClient.CmdDevDing();
    }

    public void DevDing()
    {
        audioSource.PlayOneShot(devClip);
    }
}
