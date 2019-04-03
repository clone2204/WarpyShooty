using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServerBrowser : MonoBehaviour
{
    [SerializeField] private int serverListRefreshTimer;
    private Dictionary<GameObject, NetworkID> serverEntries;
    private Dictionary<GameObject, string> lanEntries;
    
	// Use this for initialization
	void Start ()
    {
        serverEntries = new Dictionary<GameObject, NetworkID>();
        lanEntries = new Dictionary<GameObject, string>();
    }



    public void AddNewServers(List<MatchInfoSnapshot> servers)
    {
        ClearContentWindow();

        foreach (MatchInfoSnapshot serverInfo in servers)
        {

            if (serverEntries.ContainsValue(serverInfo.networkId))
                continue;

            GameObject newServer = (GameObject)Instantiate<GameObject>(transform.Find("ServerEntry").gameObject);

            newServer.transform.Find("Name").GetComponent<Text>().text = serverInfo.name;
            newServer.transform.Find("Size").GetComponent<Text>().text = serverInfo.currentSize + " / " + serverInfo.maxSize;
            newServer.transform.Find("Pass").GetComponent<Text>().text = (serverInfo.isPrivate == true ? "T" : "F");

            newServer.GetComponent<MatchInfoContainer>().matchInfo = serverInfo;

            
            serverEntries.Add(newServer, serverInfo.networkId);
        }

        UpdateContentWindow();
    }

    public void AddNewServers(Dictionary<string, NetworkBroadcastResult> servers)
    {
        foreach (string serverAddress in servers.Keys)
        {
            NetworkBroadcastResult serverData = servers[serverAddress];
            
            if (lanEntries.ContainsValue(serverAddress))
                continue;

            Debug.LogWarning("Spawn Entry: " + serverAddress);
            GameObject newServer = (GameObject)Instantiate<GameObject>(transform.Find("ServerEntry").gameObject);

            newServer.transform.Find("Name").GetComponent<Text>().text = "LAN: " + serverAddress;
            //newServer.transform.Find("Size").GetComponent<Text>().text = serverInfo.currentSize + " / " + serverInfo.maxSize;
            //newServer.transform.Find("Pass").GetComponent<Text>().text = (serverInfo.isPrivate == true ? "T" : "F");

            newServer.GetComponent<MatchInfoContainer>().lanAddress = serverAddress;

            lanEntries.Add(newServer, serverAddress);
        }

        UpdateContentWindow();
    }
    
    private void UpdateContentWindow()
    {
        
        RectTransform contentWindow = transform.Find("ServerList").Find("Viewport").Find("Content").GetComponent<RectTransform>();
        
        int offset = 0;
        foreach (GameObject lanServer in lanEntries.Keys)
        {
            lanServer.transform.SetParent(contentWindow);
            lanServer.GetComponent<RectTransform>().localPosition = new Vector3(0, offset, 0);

            offset -= 40;
        }

        foreach (GameObject server in serverEntries.Keys)
        {
            server.transform.SetParent(contentWindow);
            server.GetComponent<RectTransform>().localPosition = new Vector3(0, offset, 0);
            
            offset -= 40;
        }
    }

    public void ClearContentWindow()
    {
        Transform contentWindow = transform.Find("ServerList").Find("Viewport").GetChild(0);
        int serverCount = contentWindow.childCount;

        for(int loop = 0;  loop < serverCount; loop++)
        {
            Destroy(contentWindow.GetChild(loop).gameObject);
        }

        ClearServerEntries();

        GameObject.Find("JoinServerButton").GetComponent<Button>().interactable = false;
    }

    public void ClearServerEntries()
    {
        serverEntries.Clear();
        lanEntries.Clear();
    }

    public int GetServerListRefreshTime()
    {
        return serverListRefreshTimer;
    }

    public MatchInfoContainer GetSelectedServer()
    {
        IEnumerator<Toggle> toggleEnum = GetComponent<ToggleGroup>().ActiveToggles().GetEnumerator();
        toggleEnum.MoveNext();

        Toggle toggle = toggleEnum.Current;
        GameObject selected = toggle.gameObject;

        return selected.GetComponent<MatchInfoContainer>();
    }

    public void SetErrorMessage(string type, string message)
    {
        Transform errorBox = GameObject.Find("ErrorMessageBox").transform;

        errorBox.Find("ErrorType").GetComponent<Text>().text = type;
        errorBox.Find("ErrorMessage").GetComponent<Text>().text = message;
    }

    public void ClearErrorMessage()
    {
        Transform errorBox = GameObject.Find("ErrorMessage").transform;

        errorBox.Find("ErrorType").GetComponent<Text>().text = "";
        errorBox.Find("ErrorMessage").GetComponent<Text>().text = "";
    }

    public void ClearSearchBox()
    {
        Transform searchBox = GameObject.Find("ServerSearch").transform;

        searchBox.Find("SearchField").GetComponent<Text>().text = "";
    }

    public void ClearPasswordBox()
    {
        Transform searchBox = GameObject.Find("EnterPassword").transform;

        searchBox.Find("PasswordEntryField").GetComponent<Text>().text = "";
    }
}




