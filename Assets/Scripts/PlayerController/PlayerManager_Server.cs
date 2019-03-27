using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager_Server : NetworkBehaviour, IPlayerManager
{
    private PlayerManager clientProxy;

    [SyncVar] private string playerName;
    [SyncVar] private GameManager.Team team;

    [SyncVar] public NetworkInstanceId playerObjectID;
    public NetworkConnection playerConnection;

    public void Init(IPlayerManager playerInfoManager)
    {
        clientProxy = (PlayerManager)playerInfoManager;
    }

    public void SetName(string name)
    {
        Debug.Log("Set Name: " + name);
        this.playerName = name;
    }
    
    public string GetName()
    {
        return playerName;
    }

    public void SetTeam(GameManager.Team team)
    {
        this.team = team;
    }

    public GameManager.Team GetTeam()
    {
        return this.team;
    }

    public void SetPlayerObjectID(NetworkInstanceId playerObjectID)
    {
        this.playerObjectID = playerObjectID;
    }

    public NetworkInstanceId GetPlayerObjectID()
    {
        return this.playerObjectID;
    }

    public void SetPlayerConnection(NetworkConnection playerConnection)
    {
        this.playerConnection = playerConnection;
    }

    public NetworkConnection GetPlayerConnection()
    {
        return this.playerConnection;
    }

    public void SpawnPlayer()
    {

    }
}
