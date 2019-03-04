using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager_Client : NetworkBehaviour, ILobbyManager
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

    public void Init(ILobbyManager lobbyManager)
    {
        server = (LobbyManager_Server)lobbyManager;

        if(isServer)
        {
            
        }
        else
        {
            
        }
    }

    public void Clear()
    {
        server = null;
        //listManager = null;
    }

    public void AddPlayer(NetworkConnection playerConnection, short controllerID)
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

    public void RemovePlayer(NetworkConnection playerConnection)
    {
        throw new System.NotImplementedException();
    }

    public void SwapPlayers()
    {
        throw new System.NotImplementedException();
    }
    
}
