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
    private NetworkDiscovery networkDiscovery;
    private MatchInfo matchInfo;
    private bool matchOpen;

    private string passwordAttempt;
    private bool hostActionConfirmed;
    private bool hostActionCancelled;

    // Use this for initialization
    void Start()
    {
        serverBrowser = GameObject.Find("ServerBrowserCanvas").GetComponent<ServerBrowser>();
        lobbyManager = transform.Find("NetworkObjects").gameObject.GetComponent<LobbyManager>();
        networkDiscovery = GetComponent<NetworkDiscovery>();

        //base.StartMatchMaker();
        //StartCoroutine(AutoRefreshServerBrowser());

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        if (scene.name == "TitleScreen")
        {
            serverBrowser = GameObject.Find("ServerBrowserCanvas").GetComponent<ServerBrowser>();
            lobbyManager = transform.Find("NetworkObjects").gameObject.GetComponent<LobbyManager>();

            //base.StartMatchMaker();
            //StartCoroutine(AutoRefreshServerBrowser());
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
        matchOpen = true;

        networkDiscovery.StopBroadcast();
        networkDiscovery.Initialize();

        networkDiscovery.StartAsServer();

        StartHost();
        //StartCoroutine(AutoRefreshServer(serverName, serverPassword));
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

            if (!base.isNetworkActive)
            {
                base.StartHost(matchInfo);
            }
        }
        else
        {
            base.matchMaker.DestroyMatch(matchInfo.networkId, matchInfo.domain, OnDestroyMatch);
        }
    }

    public void ChangeGameSettings(int timeLimit, int killLimit)
    {
        lobbyManager.ChangeGameSettings(timeLimit, killLimit);
    }

    public void StartGame()
    {
        CheckReadyToBegin();
        //base.matchMaker.DestroyMatch(matchInfo.networkId, matchInfo.domain, OnDestroyMatch);
        matchOpen = false;

        lobbyManager.StartGame();
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayerManager lobbyPlayerManager = lobbyPlayer.GetComponent<LobbyPlayerManager>();
        GamePlayerManager gamePlayerManager = gamePlayer.GetComponent<GamePlayerManager>();
        
        lobbyManager.PlayerLoaded(lobbyPlayerManager, gamePlayerManager);

        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
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
        //base.matchMaker.DestroyMatch(matchInfo.networkId, matchInfo.domain, OnDestroyMatch);

        matchOpen = false;
        this.matchInfo = null;
        
        StopCoroutine("PingServer");

        menuStates.LeaveGame();
        lobbyManager.Clear();
        StopMatchMaker();
        StopHost();
    }
    #endregion

    //------------------------------------------------------------------------------------------------------------------------------
    //Client Functions
    //------------------------------------------------------------------------------------------------------------------------------
    #region
    public void JoinLobby()
    {
        MatchInfoContainer matchContainer = serverBrowser.GetSelectedServer();
        
        if(matchContainer.matchInfo != null)
        {
            if (matchContainer.matchInfo.isPrivate)
            {
                menuStates.NeedsPassword();

                StartCoroutine(WaitForPassword(matchContainer.matchInfo));
            }
            else
            {
                matchMaker.JoinMatch(matchContainer.matchInfo.networkId, "", "", "", 0, 0, OnMatchJoined);
            }
        }
        else
        {
            Debug.LogWarning("ADDRESS: " + matchContainer.lanAddress);
            base.networkAddress = matchContainer.lanAddress;
            StartClient();
            //base.client.Connect(matchContainer.lanAddress, 8080);

            menuStates.EnterLobby();
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
            Debug.LogWarning("CONNECT IP: " + matchInfo.address + ":" + matchInfo.port);

            base.StartClient();
            base.client.Connect(matchInfo);
            
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
    #endregion

    //------------------------------------------------------------------------------------------------------------------------------
    //Match Maker Functions
    //The Match Maker is currently being disable due to issues with Unity
    //throttling games connected through the matchmaker.
    //------------------------------------------------------------------------------------------------------------------------------
    #region
    //public void RefreshServerBrowser()
    //{
    //    base.matchMaker.ListMatches(0, 10, "", false, 0, 0, OnInternetMatchList);
    //}

    //private IEnumerator AutoRefreshServerBrowser()
    //{
    //    while (true)
    //    {
    //        yield return new WaitUntil(() => menuStates.GetState() is ServerBrowserState);

    //        while (menuStates.GetState() is ServerBrowserState)
    //        {
    //            RefreshServerBrowser();

    //            yield return new WaitForSecondsRealtime(5f);
    //        }

    //        if (GameObject.Find("JoinServerButton") == null)
    //            break;

    //        GameObject.Find("JoinServerButton").GetComponent<Button>().interactable = false;
    //    }
    //}

    //public void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    //{
    //    try
    //    {
    //        Debug.Log("Adding Servers: " + matches.Count);
    //        serverBrowser.AddNewServers(matches);
    //    }
    //    catch { }
    //}

    public void StartServerBrowser()
    {
        networkDiscovery.Initialize();
        networkDiscovery.StartAsClient();

        StartCoroutine(PingServers());
    }

    private IEnumerator PingServers()
    {
        while (true)
        {
            Debug.LogWarning("Server Count: " + networkDiscovery.broadcastsReceived.Keys.Count);
            serverBrowser.AddNewServers(networkDiscovery.broadcastsReceived);

            yield return new WaitForSecondsRealtime(5);
        }
    }

    public void StopServerBrowser()
    {
        StopCoroutine("PingServers");
        networkDiscovery.StopBroadcast();
    }

    #endregion

    //------------------------------------------------------------------------------------------------------------------------------
    //Lobby Host Actions
    //------------------------------------------------------------------------------------------------------------------------------
    #region

    public void StartSwap(System.Func<List<int>> GetSelected)
    {
        hostActionConfirmed = false;
        hostActionCancelled = false;

        StartCoroutine(WaitForSwapSelection(GetSelected));
    }

    private IEnumerator WaitForSwapSelection(System.Func<List<int>> GetSelected)
    {
        yield return new WaitUntil(() => hostActionConfirmed || hostActionCancelled);

        if (hostActionConfirmed)
        {
            List<int> selectedPlayers = GetSelected();
            lobbyManager.SwapPlayers(selectedPlayers);
        }

        hostActionConfirmed = false;
        hostActionCancelled = false;
    }

    public void StartKick(System.Func<List<int>> GetSelected)
    {
        hostActionConfirmed = false;
        hostActionCancelled = false;

        StartCoroutine(WaitForKickSelection(GetSelected));
    }

    private IEnumerator WaitForKickSelection(System.Func<List<int>> GetSelected)
    {
        yield return new WaitUntil(() => hostActionConfirmed || hostActionCancelled);

        if(hostActionConfirmed)
        {
            List<int> selectedPlayers = GetSelected();
            lobbyManager.KickPlayers(selectedPlayers);
        }

        hostActionConfirmed = false;
        hostActionCancelled = false;
    }

    public void StartBan(System.Func<List<int>> GetSelected)
    {
        hostActionConfirmed = false;
        hostActionCancelled = false;

        StartCoroutine(WaitForBanSelection(GetSelected));
    }

    private IEnumerator WaitForBanSelection(System.Func<List<int>> GetSelected)
    {
        yield return new WaitUntil(() => hostActionConfirmed || hostActionCancelled);

        if (hostActionConfirmed)
        {
            List<int> selectedPlayers = GetSelected();
            lobbyManager.BanPlayers(selectedPlayers);
        }

        hostActionConfirmed = false;
        hostActionCancelled = false;
    }

    public void ConfirmHostAction()
    {
        this.hostActionConfirmed = true;
    }

    public void CancelHostAction()
    {
        this.hostActionCancelled = true;
    }
    #endregion

    //------------------------------------------------------------------------------------------------------------------------------
    //Event Calls
    //------------------------------------------------------------------------------------------------------------------------------
    #region


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);

        if (!((LobbyManager)lobbyManager).GetBeenInitialized())
        {
            lobbyManager.Init();
        }

        Debug.Log("PLAYER ADDED");
        lobbyManager.AddPlayer(conn, playerControllerId);

        
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.lastError == NetworkError.Ok)
            base.OnServerDisconnect(conn);

        NetworkServer.DestroyPlayersForConnection(conn);
        
        lobbyManager.RemovePlayer(conn);

        //base.OnServerDisconnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if(conn.lastError == NetworkError.Ok)
            base.OnClientDisconnect(conn);

        Debug.LogWarning("CLIENT DISCONNECT");
        ClientScene.DestroyAllClientObjects();

        menuStates.Disconnected("Disconnected.", conn.lastError.ToString());

    }

    private void OnApplicationQuit()
    {
        NetworkServer.DisconnectAll();
        //base.matchMaker.DestroyMatch(matchInfo.networkId, matchInfo.domain, OnDestroyMatch);
        StopHost();
    }
    #endregion
}