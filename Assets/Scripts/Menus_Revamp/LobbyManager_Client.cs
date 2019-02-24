using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager_Client : NetworkBehaviour, LobbyManager
{
    private LobbyManager_Server server;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(LobbyManager lobbyManager)
    {
        server = (LobbyManager_Server)lobbyManager;
    }

    public void AddPlayer(NetworkConnection playerConnection)
    {
        throw new System.NotImplementedException();
    }

    public void BanPlayer()
    {
        throw new System.NotImplementedException();
    }

    public void KickPlayer(NetworkConnection playerConnection)
    {
        throw new System.NotImplementedException();
    }

    public void RemovePlayer()
    {
        throw new System.NotImplementedException();
    }

    public void SwapPlayers()
    {
        throw new System.NotImplementedException();
    }

}
