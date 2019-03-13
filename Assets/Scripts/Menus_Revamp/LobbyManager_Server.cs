using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class LobbyManager_Server : NetworkBehaviour, ILobbyManager
{
    private LobbyManager clientProxy;

    private PlayerInfoManager[] players;
    
    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(ILobbyManager lobbyManager)
    {
        players = new PlayerInfoManager[12];
        clientProxy = (LobbyManager)lobbyManager;
    }

    public void Clear()
    {
        clientProxy = null;
        players = null;
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
        
        playerInfo.SetPlayerObjectID(player.GetComponent<NetworkIdentity>().netId);
        playerInfo.SetPlayerConnection(playerConnection);

        //Temp Solution
        for(int loop = 0; loop < 6; loop++)
        {
            if(players[loop] == null)
            {
                players[loop] = playerInfo;
                break;
            }
            else if(players[loop + 6] == null)
            {
                players[loop + 6] = playerInfo;
                break;
            }
        }

        RequestUpdatePlayerList();
    }

    
    public void RemovePlayer(NetworkConnection playerConnection)
    {
        for (int loop = 0; loop < 12; loop++)
        {
            if (players[loop] != null && players[loop].GetPlayerConnection().Equals(playerConnection))
            {
                players[loop] = null;
            }
        }

        RequestUpdatePlayerList();
    }

    public void BanPlayers(List<int> players)
    {
        throw new NotImplementedException();
    }

    public void KickPlayers(List<int> players)
    {
        foreach(int player in players)
        {
            if (player == 0 || this.players[player] == null)
                continue;

            this.players[player].GetPlayerConnection().Disconnect();
        }
    }

   
    public void SwapPlayers(List<int> players)
    {
        throw new NotImplementedException();
    }

    private void RequestUpdatePlayerList()
    {
        Debug.Log("Request Player List Update");
        
        string[] playerList = new string[12];
        
        for (int loop = 0; loop < 12; loop++)
        {
            if(players[loop] == null)
            {
                playerList[loop] = "";
            }
            else
            {
                playerList[loop] = players[loop].GetName();
            }

            Debug.LogWarning("Slot " + loop + ": " + playerList[loop]);
        }

        clientProxy.RpcUpdatePlayerList(playerList);
    }

}
