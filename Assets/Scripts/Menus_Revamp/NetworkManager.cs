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
    private MenuStates menuStates; //Can't I do better than this tangled mess?
    private ServerBrowser serverBrowser;
    private bool browserActive;

    private LobbyManager lobbyManager;
    private MatchInfo matchInfo;

    private string joinPassword;
    private bool passwordEntered;

    // Use this for initialization
    void Start()
    {
        joinPassword = "";
        passwordEntered = false;

        lobbyManager = transform.Find("NetworkObjects").gameObject.GetComponent<LobbyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("`"))
        {
            base.StopClient();
        }
    }

    public void SetMenuStates(MenuStates menuStates)
    {
        this.menuStates = menuStates; //Can I do better than this?
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
    //Server Hosting Functions
    //------------------------------------------------------------------------------------------------------------------------------
    #region
    public void HostServer(string serverName, string serverPassword)
    {
        lobbyManager.Init();

        base.StartHost();
        base.matchMaker.CreateMatch(serverName, 12, true, serverPassword, "", "", 0, 0, OnMatchCreate);
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Debug.Log("Match Created");
        
        if (success)
        {
            this.matchInfo = matchInfo;
        }
        else
        {
            base.matchMaker.DestroyMatch(matchInfo.networkId, matchInfo.domain, OnDestroyMatch);
        }
    }

    public void LeaveHostLobby()
    {
        if (client == null)
        {
            menuStates.Back();
            return;
        }
        menuStates.LeaveGame();

        base.matchMaker.DestroyMatch(matchInfo.networkId, 0, OnDestroyMatch);

        StopServerBrowser();
        lobbyManager.Clear();

        base.client.Disconnect();
        base.StopHost();
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

        if (success)
        {
            StopServerBrowser();

            menuStates.EnterLobby();

            base.StartClient();
        }
        else
        {
            menuStates.Disconnected();
        }
    }

    public void LeaveLobby()
    {
        menuStates.LeaveGame();
        StopServerBrowser();
        lobbyManager.Clear();

        base.client.Disconnect();
        base.StopClient();
    }
    #endregion

    //------------------------------------------------------------------------------------------------------------------------------
    //Match Maker Functions
    //------------------------------------------------------------------------------------------------------------------------------
    #region
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
    #endregion

    //------------------------------------------------------------------------------------------------------------------------------
    //Event Calls
    //------------------------------------------------------------------------------------------------------------------------------
    #region
    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);


    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);

        if (!((LobbyManager)lobbyManager).GetBeenInitialized())
        {
            lobbyManager.Init(null);
        }

        Debug.Log("Player Connected");
        lobbyManager.AddPlayer(conn, playerControllerId);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        if (NetworkServer.active)
            return;

        if (!((LobbyManager)lobbyManager).GetBeenInitialized())
        {
            //lobbyManager.Init(null);
        }

        //lobbyManager.AddPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        Debug.LogWarning("Player Disconnected");
        lobbyManager.RemovePlayer(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        menuStates.LeaveGame();
    }
    #endregion
}