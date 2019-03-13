using UnityEngine;
using UnityEngine.Networking;
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

    public void Init(ILobbyManager lobbyManager = null)
    {
        listManager = GameObject.Find("HostLobbyCanvas").GetComponent<PlayerListManager>();

        realLobbyManager = gameObject.GetComponent<LobbyManager_Server>();
        realLobbyManager.Init(this);
        
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
        realLobbyManager.AddPlayer(playerConnection, controllerID);        
    }

    public void RemovePlayer(NetworkConnection playerConnection)
    {
        realLobbyManager.RemovePlayer(playerConnection);
    }

    public void BanPlayers(List<int> players)
    {
        realLobbyManager.BanPlayers(players);
    }

    public void KickPlayers(List<int> players)
    {
        realLobbyManager.KickPlayers(players);
    }

    
    public void SwapPlayers(List<int> players)
    {
        realLobbyManager.SwapPlayers(players);
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

}
