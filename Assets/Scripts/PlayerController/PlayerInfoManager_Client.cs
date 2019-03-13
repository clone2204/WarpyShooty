using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfoManager_Client : NetworkBehaviour, IPlayerInfoManager
{
    private PlayerInfoManager_Server server;

    public void Init(IPlayerInfoManager playerInfoManager)
    {
        server = (PlayerInfoManager_Server)playerInfoManager;
    }

    public void SetName(string name)
    {
        server.CmdSetName(name);
    }

    public string GetName()
    {
        return server.GetName();
    }

    
    public NetworkInstanceId GetPlayerObjectID()
    {
        return server.GetPlayerObjectID();
    }
    
    public NetworkConnection GetPlayerConnection()
    {
        return server.GetPlayerConnection();
    }

    public void SetPlayerObjectID(NetworkInstanceId playerObjectID)
    {
        throw new System.NotImplementedException();
    }

    public void SetPlayerConnection(NetworkConnection networkConnection)
    {
        throw new System.NotImplementedException();
    }
}
