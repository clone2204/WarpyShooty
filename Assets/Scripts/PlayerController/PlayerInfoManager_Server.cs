using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfoManager_Server : NetworkBehaviour, IPlayerInfoManager
{
    private PlayerInfoManager_Client client;

    [SyncVar] private int playerID;
    [SyncVar] private string playerName;
    //Team here

    [SyncVar] public NetworkInstanceId playerObjectID;
    public NetworkConnection playerConnection;

    public void Init(IPlayerInfoManager playerInfoManager)
    {
        client = (PlayerInfoManager_Client)playerInfoManager;
    }

    public void SetPlayerID(int playerID)
    {
        this.playerID = playerID;
    }

    public int GetPlayerID()
    {
        return this.playerID;
    }

    public void SetName(string name)
    {
        Debug.LogWarning("Set Name: " + name);
        this.playerName = name;
    }

    [Command]
    public void CmdSetName(string name)
    {
        SetName(name);
    }

    public string GetName()
    {
        return playerName;
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
}
