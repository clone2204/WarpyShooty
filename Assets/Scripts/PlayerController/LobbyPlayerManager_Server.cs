using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobbyPlayerManager_Server : NetworkBehaviour, ILobbyPlayerManager
{
    private LobbyPlayerManager clientProxy;

    [SyncVar] private string playerName;
    
    [SyncVar] public NetworkInstanceId playerObjectID;
    public NetworkConnection playerConnection;
   
    public void Init()
    {
        clientProxy = GetComponent<LobbyPlayerManager>();

        
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
