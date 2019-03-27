using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class LobbyManager_Server : NetworkBehaviour, ILobbyManager
{
    private LobbyManager clientProxy;

    private PlayerManager[] players;
    private List<string> bannedPlayers;

    IGameManager gameManager;
    private int gameTimeLimit;
    private int gameKillLimit;
    private Dictionary<PlayerManager, bool> readyPlayers;
    
    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init()
    {
        clientProxy = GetComponent<LobbyManager>();

        players = new PlayerManager[12];
        bannedPlayers = new List<string>();

        this.gameManager = GetComponent<GameManager>();
        gameTimeLimit = 10;
        gameKillLimit = 15;
    }

    public void Clear()
    {
        clientProxy = null;
        players = null;
    }

    public void AddPlayer(NetworkConnection playerConnection, short controllerID)
    {
        Debug.LogWarning("ADD PLAYER: " + playerConnection.connectionId);
        if(bannedPlayers.Contains(playerConnection.address))
        {
            StartCoroutine(WaitForConnection(playerConnection));
            return;
        }

        StartCoroutine(WaitForClientInfo(playerConnection, controllerID));
    }

    private IEnumerator WaitForConnection(NetworkConnection connection)
    {
        yield return new WaitUntil(() => connection.isConnected);

        connection.Disconnect();
    }

    private IEnumerator WaitForClientInfo(NetworkConnection playerConnection, short controllerID)
    {
        GameObject player = playerConnection.playerControllers[controllerID].gameObject;
        PlayerManager playerInfo = player.GetComponent<PlayerManager>();
        playerInfo.Init(null);

        Debug.LogWarning("DING");
        yield return new WaitUntil(() => playerInfo.GetName() != null);
        Debug.LogWarning("DANG");
        playerInfo.SetPlayerObjectID(player.GetComponent<NetworkIdentity>().netId);
        playerInfo.SetPlayerConnection(playerConnection);

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
        Debug.LogWarning("Ban Players " + players.Count);
        foreach (int player in players)
        {
            if (player == 0 || this.players[player] == null)
                continue;

            Debug.LogWarning("Ban " + this.players[player].GetName() + " || " + this.players[player].GetPlayerConnection().connectionId);
            this.bannedPlayers.Add(this.players[player].GetPlayerConnection().address);
            this.players[player].GetPlayerConnection().Disconnect();
        }
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
        Queue<int> leftPlayers = new Queue<int>();
        Queue<int> rightPlayers = new Queue<int>();

        foreach(int player in players)
        {
            if (player == 0)
                continue;

            if(player < 6)
            {
                leftPlayers.Enqueue(player);
            }
            else
            {
                rightPlayers.Enqueue(player);
            }
        }

        foreach(int leftPlayer in leftPlayers)
        {
            if (rightPlayers.Count == 0)
                break;

            int rightPlayer = rightPlayers.Dequeue();
            PlayerManager temp = this.players[rightPlayer];
            this.players[rightPlayer] = this.players[leftPlayer];
            this.players[leftPlayer] = temp;
        }

        RequestUpdatePlayerList();
    }

    public void ChangeGameSettings(int timeLimit, int killLimit)
    {
        this.gameTimeLimit = timeLimit;
        this.gameKillLimit = killLimit;

        clientProxy.RpcUpdateGameSettings(gameTimeLimit, gameKillLimit);
    }

    public void StartGame()
    {
        Debug.LogWarning("Game Manager: " + gameManager);
        gameManager.SetupGame(new List<PlayerManager>(this.players), this.gameTimeLimit, this.gameKillLimit);

        readyPlayers = new Dictionary<PlayerManager, bool>();
        for(int loop = 0; loop < 12; loop++)
        {
            if (players[loop] == null)
                continue;

            readyPlayers.Add(players[loop], false);
        }
    }

    public void PlayerLoaded(PlayerManager player)
    {
        readyPlayers[player] = true;

        if (AreAllPlayersLoaded())
            gameManager.StartGame();
    }

    private bool AreAllPlayersLoaded()
    {
        foreach(PlayerManager player in readyPlayers.Keys)
        {
            if (readyPlayers[player] == false)
                return false;
        }

        return true;
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
        }

        Debug.LogWarning("CLIENT PROXY: " + clientProxy);
        clientProxy.RpcUpdatePlayerList(playerList);
        clientProxy.RpcUpdateGameSettings(this.gameTimeLimit, this.gameKillLimit);
    }

}
