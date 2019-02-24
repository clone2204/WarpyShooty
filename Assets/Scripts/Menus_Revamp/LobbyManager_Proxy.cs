using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyManager_Proxy : NetworkBehaviour, LobbyManager
{
    private LobbyManager realLobbyManager;

    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(LobbyManager lobbyManager)
    {
        if (isServer)
        {
            realLobbyManager = gameObject.GetComponentInChildren<LobbyManager_Server>();
            LobbyManager client = gameObject.GetComponentInChildren<LobbyManager_Client>();

            realLobbyManager.Init(client);
            client.Init(realLobbyManager);
        }
        else
        {
            realLobbyManager = gameObject.GetComponentInChildren<LobbyManager_Client>();
            LobbyManager server = gameObject.GetComponentInChildren<LobbyManager_Server>();

            realLobbyManager.Init(server);
            server.Init(realLobbyManager);
        }
    }

    public void AddPlayer(NetworkConnection playerConnection)
    {
        realLobbyManager.AddPlayer(playerConnection);
    }

    public void BanPlayer()
    {
        realLobbyManager.BanPlayer();
    }

    public void KickPlayer(NetworkConnection playerConnection)
    {
        realLobbyManager.KickPlayer(playerConnection);
    }

    public void RemovePlayer()
    {
        realLobbyManager.RemovePlayer();
    }

    public void SwapPlayers()
    {
        realLobbyManager.SwapPlayers();
    }
}
