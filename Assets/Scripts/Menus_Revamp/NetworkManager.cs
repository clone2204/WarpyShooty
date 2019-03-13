using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : NetworkLobbyManager
{
    private MenuStateManager menuStates; //Can't I do better than this tangled mess?
    private ServerBrowser serverBrowser;
    
    private LobbyManager lobbyManager;
    private MatchInfo matchInfo;
    private bool matchOpen;

    private string passwordAttempt;

    // Use this for initialization
    void Start()
    {
        serverBrowser = GameObject.Find("ServerBrowserCanvas").GetComponent<ServerBrowser>();
        lobbyManager = transform.Find("NetworkObjects").gameObject.GetComponent<LobbyManager>();
        base.StartMatchMaker();

        StartCoroutine(AutoRefreshServerBrowser());

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        if (scene.name == "TitleScreen")
        {
            serverBrowser = GameObject.Find("ServerBrowserCanvas").GetComponent<ServerBrowser>();
            lobbyManager = transform.Find("NetworkObjects").gameObject.GetComponent<LobbyManager>();
            base.StartMatchMaker();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("`"))
        {
            base.StopClient();
        }
    }

    public void SetMenuStates(MenuStateManager menuStates)
    {
        this.menuStates = menuStates; //Can I do better than this?
    }
    
    //------------------------------------------------------------------------------------------------------------------------------
    //Server Hosting Functions
    //------------------------------------------------------------------------------------------------------------------------------
    #region
    public void HostServer(string serverName, string serverPassword)
    {
        lobbyManager.Init();

        base.StartHost();
        matchOpen = true;

        StartCoroutine(AutoRefreshServer(serverName, serverPassword));
    }

    private IEnumerator AutoRefreshServer(string name, string password)
    {
        while (matchOpen)
        {
            Debug.LogWarning("Refreshing Match......");
            base.matchMaker.CreateMatch(name, 12, true, password, "", "", 0, 0, OnMatchCreate);

            yield return new WaitForSecondsRealtime(31f);
        }

        StopCoroutine("PingServer");
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.LogWarning(".......Match Created!");
            this.matchInfo = matchInfo;
        }
        else
        {
            base.matchMaker.DestroyMatch(matchInfo.networkId, matchInfo.domain, OnDestroyMatch);
        }
    }
    
    public void LeaveHostLobby()
    {
        if (!isNetworkActive)
        {
            Debug.LogWarning("Non-Host Back");
            menuStates.Back();
            return;
        }

        Debug.LogWarning("Attemp Match Destroy");
        NetworkServer.DisconnectAll();
        base.matchMaker.DestroyMatch(matchInfo.networkId, matchInfo.domain, OnDestroyMatch);

        matchOpen = false;
        this.matchInfo = null;
        
        StopCoroutine("PingServer");

        menuStates.LeaveGame();
        lobbyManager.Clear();
        StopMatchMaker();
        StopHost();
    }

    public override void OnDestroyMatch(bool success, string extendedInfo)
    {
        base.OnDestroyMatch(success, extendedInfo);

        Debug.LogWarning("Match Destroyed");
        
    }

    #endregion

    //------------------------------------------------------------------------------------------------------------------------------
    //Client Joining Functions
    //------------------------------------------------------------------------------------------------------------------------------
    #region
        public void JoinMatch()
        {
            MatchInfoSnapshot matchInfo = serverBrowser.GetSelectedServer();

            if (matchInfo.isPrivate)
            {
                menuStates.NeedsPassword();

                StartCoroutine(WaitForPassword(matchInfo));
            }
            else
            {
                matchMaker.JoinMatch(matchInfo.networkId, "", "", "", 0, 0, OnMatchJoined);
            }

        }

        public void EnterPassword(string password)
        {
            passwordAttempt = password;
        }

        private IEnumerator WaitForPassword(MatchInfoSnapshot matchInfo)
        {
            passwordAttempt = "";

            yield return new WaitUntil(() => passwordAttempt != "");

            matchMaker.JoinMatch(matchInfo.networkId, passwordAttempt, "", "", 0, 0, OnMatchJoined);
            passwordAttempt = "";
        }

        public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            Debug.LogWarning("Match Joined: " + success);

            if (success)
            {
                base.StartClient();
                menuStates.EnterLobby();

                this.matchInfo = matchInfo;
            }
            else
            {
                menuStates.Disconnected("Error", "Could not connect to Server.");
            }
        }

        public void LeaveLobby()
        {
            Debug.LogWarning("Leave Lobby: " + base.matchMaker);

            menuStates.LeaveGame();
        
            lobbyManager.Clear();
            StopMatchMaker();
            StopClient();
        }

        public override void OnDropConnection(bool success, string extendedInfo)
        {
            base.OnDropConnection(success, extendedInfo);

            Debug.LogWarning("DROP CONNECTION");
        }
    #endregion

    //------------------------------------------------------------------------------------------------------------------------------
    //Match Maker Functions
    //------------------------------------------------------------------------------------------------------------------------------
    #region
    public void RefreshServerBrowser()
    {
        base.matchMaker.ListMatches(0, 10, "", false, 0, 0, OnInternetMatchList);
    }

    private IEnumerator AutoRefreshServerBrowser()
    {
        while (true)
        {
            yield return new WaitUntil(() => menuStates.GetState() is ServerBrowserState);

            Debug.LogWarning(menuStates.GetState());
            while(menuStates.GetState() is ServerBrowserState)
            {
                RefreshServerBrowser();

                yield return new WaitForSecondsRealtime(5f);
            }

            GameObject.Find("JoinServerButton").GetComponent<Button>().interactable = false;
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
    #endregion

    //------------------------------------------------------------------------------------------------------------------------------
    //Lobby Host Actions
    //------------------------------------------------------------------------------------------------------------------------------
    #region
    public void StartKick()
    {
        StartCoroutine(WaitForKickSelection()); 
    }

    private IEnumerator WaitForKickSelection()
    {
        yield return new WaitUntil(TEST);
    }

    public System.Func<bool> TEST()
    {
        return TEST2;
    }

    public bool TEST2()
    {
        return true;
    }
    #endregion

    //------------------------------------------------------------------------------------------------------------------------------
    //Event Calls
    //------------------------------------------------------------------------------------------------------------------------------
    #region
        public override void OnLobbyServerConnect(NetworkConnection connection)
        {
            base.OnLobbyServerConnect(connection);


        }

        public override void OnLobbyClientEnter()
        {
            base.OnLobbyClientEnter();

            Debug.LogWarning("LOBBY CLIENT ENTER");
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            base.OnServerAddPlayer(conn, playerControllerId);

            if (!((LobbyManager)lobbyManager).GetBeenInitialized())
            {
                lobbyManager.Init(null);
            }

            Debug.Log("PLAYER ADDED");
            lobbyManager.AddPlayer(conn, playerControllerId);
        }

        public override void OnLobbyClientConnect(NetworkConnection conn)
        {
            base.OnLobbyClientConnect(conn);

            Debug.LogWarning("CLIENT CONNECT");
            //menuStates.EnterLobby();
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (conn.lastError == NetworkError.Ok)
                base.OnServerDisconnect(conn);

            NetworkServer.DestroyPlayersForConnection(conn);
        
            lobbyManager.RemovePlayer(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            if(conn.lastError == NetworkError.Ok)
                base.OnClientDisconnect(conn);

            Debug.LogWarning("CLIENT DISCONNECT");
            ClientScene.DestroyAllClientObjects();

            menuStates.Disconnected("Disconnected.", conn.lastError.ToString());

        }
    #endregion
}