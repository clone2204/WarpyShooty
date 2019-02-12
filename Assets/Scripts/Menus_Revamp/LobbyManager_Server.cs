using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class LobbyManager_Server : NetworkBehaviour, LobbyManager
{
    private Dictionary<string, NetworkConnection> players;

    
    // Use this for initialization
    void Start ()
    {
        players = new Dictionary<string, NetworkConnection>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddPlayer(NetworkConnection playerConnection)
    {
        playerConnection.clientOwnedObjects.
        players.Add(playerConnection);
    }

    public void BanPlayer()
    {
        throw new NotImplementedException();
    }

    public void KickPlayer(NetworkConnection playerConnection)
    {
        throw new NotImplementedException();
    }

    public void RemovePlayer()
    {
        throw new NotImplementedException();
    }

    public void SwapPlayers()
    {
        throw new NotImplementedException();
    }

}
