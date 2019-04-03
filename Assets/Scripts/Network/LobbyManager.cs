using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour, ILobbyManager
{
    private PlayerListManager listManager;

    private ILobbyManager realLobbyManager;
    private bool beenInitialized;

    // Use this for initialization
    void Start()
    {
        listManager = GameObject.Find("LobbyCanvas").GetComponent<PlayerListManager>();

        beenInitialized = false;
        realLobbyManager = null;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TitleScreen")
        {
            listManager = GameObject.Find("LobbyCanvas").GetComponent<PlayerListManager>();
        }
    }

    public void Init()
    {
        listManager = GameObject.Find("HostLobbyCanvas").GetComponent<PlayerListManager>();

        realLobbyManager = gameObject.GetComponent<LobbyManager_Server>();
        realLobbyManager.Init();
        
        beenInitialized = true;
    }

    public void Clear()
    {
        beenInitialized = false;

        if (realLobbyManager == null) return;

        realLobbyManager.Clear();
        realLobbyManager = null;
    }

    public void AddPlayer(NetworkConnection playerConnection, short controllerID)
    {
        if (!isServer)
            return;

        realLobbyManager.AddPlayer(playerConnection, controllerID);        
    }

    public void RemovePlayer(NetworkConnection playerConnection)
    {
        if (!isServer)
            return;

        realLobbyManager.RemovePlayer(playerConnection);
    }

    public void BanPlayers(List<int> players)
    {
        if (!isServer)
            return;

        realLobbyManager.BanPlayers(players);
    }

    public void KickPlayers(List<int> players)
    {
        if (!isServer)
            return;

        realLobbyManager.KickPlayers(players);
    }

    
    public void SwapPlayers(List<int> players)
    {
        if (!isServer)
            return;

        realLobbyManager.SwapPlayers(players);
    }

    public void ChangeGameSettings(int timeLimit, int killLimit)
    {
        if (!isServer)
            return;

        realLobbyManager.ChangeGameSettings(timeLimit, killLimit);
    }

    public void StartGame()
    {
        if (!isServer)
            return;

        realLobbyManager.StartGame();
    }

    public void PlayerLoaded(LobbyPlayerManager lobbyPlayer, GamePlayerManager gameManager)
    {
        if (!isServer)
            return;

        realLobbyManager.PlayerLoaded(lobbyPlayer, gameManager);
    }

    public bool GetBeenInitialized()
    {
        return beenInitialized;
    }

    [ClientRpc]
    public void RpcUpdatePlayerList(string[] players)
    {
        Debug.Log("Client Update Player List");

        List<string> playerList = new List<string>(players);
        
        listManager.UpdatePlayerList(playerList);
    }

    [ClientRpc]
    public void RpcUpdateGameSettings(int timeLimit, int killLimit)
    {
        if (isServer)
            return;

        Transform lobbySettings = GameObject.Find("LobbyCanvas").transform.Find("ViewLobbySettings");
        lobbySettings.Find("TimeLimit").GetComponent<Text>().text = "Time Limit: " + timeLimit;
        lobbySettings.Find("KillLimit").GetComponent<Text>().text = "Kill Limit: " + killLimit;
    }

}
