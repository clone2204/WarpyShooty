using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : NetworkLobbyManager
{
    private MenuStates menuStates; //Can't I do better than this tangled mess?
    private ServerBrowser serverBrowser;
    private bool browserActive;

    private LobbyManager lobbyManager;

    private string serverName;
    private bool serverRequiresPassword;
    private string serverPassword;

    private string joinPassword;
    private bool passwordEntered;

	// Use this for initialization
	void Start ()
    {
        serverName = "";
        serverRequiresPassword = false;
        serverPassword = "";

        joinPassword = "";
        passwordEntered = false;

        SceneManager.sceneLoaded += OnSceneLoaded;

        lobbyManager = GetComponent<LobbyManager_Proxy>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown("`"))
        {
            StopHost();
        }
	}

    public void SetMenuStates(MenuStates menuStates)
    {
        this.menuStates = menuStates; //Can I do better than this?
    }

    public void SetServerName(string name)
    {
        serverName = name;
    }

    public void SetServerPassword(string pass)
    {
        serverPassword = pass;
    }

    public void SetRequirePassword(bool require)
    {
        serverRequiresPassword = require;
    }

    public void SetJoinPassword(string pass)
    {
        joinPassword = pass;
    }

    public void SetPasswordEntered(bool entered)
    {
        passwordEntered = entered;
    }

    //------------------------------------------------------------------------------------------------------------------------------
    //Hosts a new server, switchin over to the lobby scene, and starting a new server
    //------------------------------------------------------------------------------------------------------------------------------
    public void HostServer()
    {
        base.StartHost();
        
        //See if adding this to a coroutine will keep it in the server browser
        base.matchMaker.CreateMatch(serverName, 12, true, serverPassword, "", "", 0, 0, OnMatchCreate);
        
        menuStates.EnterGame();

        //lobbyManager = gameObject.AddComponent<LobbyManager_Server>();
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Debug.Log("Match Created");

        if(success)
        {
            
        }
        else
        {

        }
        
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        if (scene.name == "TitleScreen")
        {
            GetComponent<MenuManager>().SetUIActions();
            return;
        }

        base.TryToAddPlayer();

        
    }

    //------------------------------------------------------------------------------------------------------------------------------
    //Joins a Server, switches over to the lobby scene, then starts the client
    //------------------------------------------------------------------------------------------------------------------------------

    public void JoinMatch()
    {
        MatchInfoSnapshot matchInfo = serverBrowser.GetSelectedServer();

        if (matchInfo.isPrivate)
        {
            menuStates.NeedsPassword();

            joinPassword = "";
            passwordEntered = false;

            StartCoroutine(WaitForPassword(matchInfo));
        }
        else
        {
            matchMaker.JoinMatch(matchInfo.networkId, "", "", "", 0, 0, OnMatchJoined);
        }

    }

    private IEnumerator WaitForPassword(MatchInfoSnapshot matchInfo)
    {
        yield return new WaitUntil(() => passwordEntered);

        matchMaker.JoinMatch(matchInfo.networkId, joinPassword, "", "", 0, 0, OnMatchJoined);
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Debug.LogWarning("Match Joined: " + success);

        //base.TryToAddPlayer();

        if (success)
        {
            StopServerBrowser();

            menuStates.EnterGame();
            
            base.StartClient();
        }
        else
        {
            menuStates.Disconnected();
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------

    public void SetErrorMessage(string type, string message)
    {
        serverBrowser.SetErrorMessage(type, message);
    }

    public void ClearErrorMessage()
    {
        serverBrowser.ClearServerEntries();
    }
    
    //------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------

    public void StartServerBrowser()
    {
        serverBrowser = GameObject.Find("ServerBrowserCanvas").GetComponent<ServerBrowser>();

        browserActive = true;
        base.StartMatchMaker();
        StartCoroutine(ServerListAutoRefresh(serverBrowser.GetServerListRefreshTime()));
        
    }

    public void StopServerBrowser()
    {
        serverBrowser.ClearContentWindow();

        StopCoroutine("ServerListAutoRefresh");
        base.StopMatchMaker();
        browserActive = false;

        GameObject.Find("JoinServerButton").GetComponent<Button>().interactable = false;
    }

    public void RefreshServerBrowser()
    {
        base.matchMaker.ListMatches(0, 50, "", false, 0, 0, OnInternetMatchList);
    }

    private IEnumerator ServerListAutoRefresh(int refreshTime)
    {
        while (browserActive)
        {
            Debug.LogWarning("LIST MATCHES: ");

            RefreshServerBrowser();
            yield return new WaitForSecondsRealtime(refreshTime);
        }
    }

    public void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        try
        {
            Debug.Log("Adding Servers: " + matches.Count);

            serverBrowser.AddNewServers(matches);
        }
        catch { }
    }

    //------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------

    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);

        Debug.LogWarning("DING");
        Debug.LogWarning(connection);

        lobbyManager.AddPlayer(connection);

        //connection.Disconnect();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        //menuStates.LeaveGame();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        menuStates.LeaveGame();
    }

    public override void OnLobbyStopHost()
    {
        base.OnLobbyStopHost();

        Debug.LogWarning("DING");
        menuStates.LeaveGame();
    }
}
