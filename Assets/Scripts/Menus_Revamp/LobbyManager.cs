using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

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
    }

    // Update is called once per frame
    void Update() {

    }

    public void Init(ILobbyManager lobbyManager = null)
    {
        Debug.LogWarning("Client INIT: " + gameObject.GetComponent<NetworkIdentity>().netId);
        listManager = GameObject.Find("HostLobbyCanvas").GetComponent<PlayerListManager>();

        realLobbyManager = gameObject.GetComponent<LobbyManager_Server>();
        realLobbyManager.Init(this);
        Debug.LogWarning("lobbyManager: " + realLobbyManager);

        beenInitialized = true;
    }

    public void Clear()
    {
        Debug.LogWarning("CLEAR");
        beenInitialized = false;

        if (realLobbyManager == null) return;

        realLobbyManager.Clear();
        realLobbyManager = null;
    }

    public void AddPlayer(NetworkConnection playerConnection, short controllerID)
    {
        Debug.LogWarning("AddPlayer to: " + realLobbyManager);
        
        realLobbyManager.AddPlayer(playerConnection, controllerID);        
    }

    public void BanPlayer()
    {
        realLobbyManager.BanPlayer();
    }

    public void KickPlayer(NetworkConnection playerConnection)
    {
        realLobbyManager.KickPlayer(playerConnection);
    }

    public void RemovePlayer(NetworkConnection playerConnection)
    {
        realLobbyManager.RemovePlayer(playerConnection);
    }

    public void SwapPlayers()
    {
        realLobbyManager.SwapPlayers();
    }

    public bool GetBeenInitialized()
    {
        return beenInitialized;
    }

    [ClientRpc]
    public void RpcUpdatePlayerList(string[] players, string host)
    {
        Debug.LogWarning("Client Update Player List");

        List<string> playerList = new List<string>(players);
        listManager.UpdatePlayerList(playerList, host);
    }

}
