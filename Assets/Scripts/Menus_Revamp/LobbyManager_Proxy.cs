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

    public void Init()
    {
        if (isServer)
        {
            Debug.LogWarning("Proxy Init");
            realLobbyManager = gameObject.AddComponent<LobbyManager_Server>();
            realLobbyManager.Init();
        }
        else
        {
            //Add a LobbyManager_Client?
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
