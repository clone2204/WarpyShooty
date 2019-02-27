using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfoManager_Server : NetworkBehaviour, PlayerInfoManager
{
    private PlayerInfoManager_Client client;

    [SyncVar] private string name;

    public void Init(PlayerInfoManager playerInfoManager)
    {
        client = (PlayerInfoManager_Client)playerInfoManager;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return name;
    }
}
