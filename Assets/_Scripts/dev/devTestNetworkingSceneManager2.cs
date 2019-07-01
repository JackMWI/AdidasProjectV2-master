using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class devTestNetworkingSceneManager2 : MonoBehaviour
{
    public enum NetworkState
    {
        Uninitialized,
        Client,
        Host
    }



    public static devTestNetworkingSceneManager2 _localInstance;
    public static NetworkDiscovery networkDiscovery;
    public static devCustomNetworkManager2 networkManager;

    public NetworkState nState = NetworkState.Uninitialized;
    public devClient2 currentClient = null;
    public GameObject[] UIObjects;
    public Transform formDisplayParent;
    public GameObject formDisplayPrefab;
    public AudioSource devAudioSource;
    public AudioClip devDing;
    public Text ipText;
    public string manualIP;
    public string currentName, currentColor, currentEmail;

    public List<devFormDisplay2> currentForms;


    private void Awake()
    {
        _localInstance = this;
        networkDiscovery = GetComponent<NetworkDiscovery>();
        networkManager = GetComponent<devCustomNetworkManager2>();
        ReturnToMainMenu();

        networkManager.ConnectionFailed.AddListener(OnConnectionError);
        networkManager.ConnectionSuccesful.AddListener(OnSuccesfullyConnected);
    }



    public void StartDiscoveringServer()
    {
        // Try starting the network discovery sequence. If the port is taken, go back to the menu.
        if (!networkDiscovery.Initialize())
        {
            ReturnToMainMenu();
            Debug.Log("Can't start the network discovery sequence.... The port is taken??");
            return;
        }

        nState = NetworkState.Client;
        SwitchToUI(2);

        networkDiscovery.StartAsClient();

        Debug.Log("Attempting To connect...");
    }

    public void ManuallyConnectToIP()
    {
        networkManager.networkAddress = manualIP;
        networkManager.StartClient();
        Debug.Log("Trying to manually connect");
    }

    public void StartHostingServer()
    {
        // Try starting the network discovery sequence. If the port is taken, go back to the menu.
        if (!networkDiscovery.Initialize())
        {
            ReturnToMainMenu();
            Debug.Log("Can't start the network discovery sequence.... The port is taken??");
            return;
        }

        nState = NetworkState.Host;
        SwitchToUI(1);

        networkDiscovery.StartAsServer();
        networkManager.StartHost();

        AssignIPText();
        SwitchToUI(3);

    }

    void OnSuccesfullyConnected()
    {
        if (nState != NetworkState.Client)
        {
            return;
        }

        SwitchToUI(4);
    }

    void OnConnectionError()
    {
        if (nState != NetworkState.Client)
        {
            return;
        }

        SwitchToUI(5);
    }



    public void UpdateManualIPString(string inp)
    {
        manualIP = inp;
    }

    public void UpdateCurrentName(string inp)
    {
        currentName = inp;
    }

    public void UpdateCurrentColor(string inp)
    {
        currentColor = inp;
    }

    public void UpdateCurrentEmail(string inp)
    {
        currentEmail = inp;
    }



    public void CallDevDing()
    {
        currentClient.CmdCallDevDing();
    }

    public void DevDing()
    {
        devAudioSource.PlayOneShot(devDing);
    }

    public void SendCurrentFormInfo()
    {
        currentClient.CmdSendFormInfo(currentName, currentColor, currentEmail);
    }

    public void AddNewFormInfo(devForm2 newInfo)
    {
        if(nState != NetworkState.Host)
        {
            return;
        }

        Debug.Log("form recieved!!");
        GameObject temp = Instantiate(formDisplayPrefab, formDisplayParent);


        currentForms.Add(temp.GetComponent<devFormDisplay2>());
        currentForms[currentForms.Count - 1].formData = newInfo;
        currentForms[currentForms.Count - 1].UpdateUI();
    }



    public void ReturnToMainMenu()
    {
        nState = NetworkState.Uninitialized;
        ipText.text = "";
        manualIP = "";
        currentForms = new List<devFormDisplay2>();
        SwitchToUI(0);
    }

    public void SwitchToUI(int inp)
    {
        for (int i = 0; i < UIObjects.Length; i++)
        {
            UIObjects[i].SetActive(i == inp);
        }
    }

    public void AssignIPText()
    {
        ipText.text = "";
        foreach(string tempIP in LocalIPAddresses())
        {
            ipText.text += tempIP + "\n";
        }
        ipText.text = ipText.text.Substring(0, ipText.text.Length -1);
    }

    public List<string> LocalIPAddresses()
    {
        List<string> outputList = new List<string>();
        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                outputList.Add(ip.ToString());
                
            }
        }
        return outputList;
    }
}
