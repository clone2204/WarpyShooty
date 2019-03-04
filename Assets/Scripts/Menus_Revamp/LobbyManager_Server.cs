using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class LobbyManager_Server : NetworkBehaviour, ILobbyManager
{
    private LobbyManager clientProxy;

    private Dictionary<int, PlayerInfoManager> players;
    private int hostID;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(ILobbyManager lobbyManager)
    {
        Debug.LogWarning("Server Init");
        players = new Dictionary<int, PlayerInfoManager>();
        clientProxy = (LobbyManager)lobbyManager;

        hostID = 0;
    }

    public void Clear()
    {
        clientProxy = null;
        players = null;
        hostID = 0;
    }

    public void AddPlayer(NetworkConnection playerConnection, short controllerID)
    {
        StartCoroutine(WaitForClientInfo(playerConnection, controllerID));
    }

    private IEnumerator WaitForClientInfo(NetworkConnection playerConnection, short controllerID)
    {
        GameObject player = playerConnection.playerControllers[controllerID].gameObject;
        PlayerInfoManager playerInfo = player.GetComponent<PlayerInfoManager>();
        playerInfo.Init(null);

        yield return new WaitUntil(() => playerInfo.GetName() != null);
        
        //Generates a random player ID and ensures it is unique
        System.Random rand = new System.Random();
        int randomPlayerID = rand.Next();
        while(players.ContainsKey(randomPlayerID))
        {
            randomPlayerID = rand.Next();
        }

        if (players.Count == 0)
            hostID = randomPlayerID;

        playerInfo.SetPlayerID(randomPlayerID);
        playerInfo.SetPlayerObjectID(player.GetComponent<NetworkIdentity>().netId);
        playerInfo.SetPlayerConnection(playerConnection);

        players.Add(randomPlayerID, playerInfo);

        RequestUpdatePlayerList();
    }

    
    public void RemovePlayer(NetworkConnection playerConnection)
    {
        int playerToRemove = 0;
        foreach(int player in players.Keys)
        {
            if(players[player].GetPlayerConnection().Equals(playerConnection))
            {
                Debug.LogWarning("Removing: " + players[player].GetName());
                playerToRemove = player;
            }
        }

        Debug.LogWarning(players.Count);
        players.Remove(playerToRemove);
        Debug.LogWarning(players.Count);

        RequestUpdatePlayerList();
    }

    public void BanPlayer()
    {
        throw new NotImplementedException();
    }

    public void KickPlayer(NetworkConnection playerConnection)
    {
        throw new NotImplementedException();
    }

   
    public void SwapPlayers()
    {
        throw new NotImplementedException();
    }

    private void RequestUpdatePlayerList()
    {
        Debug.LogWarning("Request Player List Update");

        string[] playerList = new string[players.Count];
        List<PlayerInfoManager> temp = new List<PlayerInfoManager>(players.Values);

        for (int loop = 0; loop < players.Count; loop++)
        {
            playerList[loop] = temp[loop].GetName();
        }

        Debug.LogWarning(playerList.Length);
        clientProxy.RpcUpdatePlayerList(playerList, players[hostID].GetName());
    }
}
