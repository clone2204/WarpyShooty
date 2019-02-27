﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfoManager_Client : NetworkBehaviour, PlayerInfoManager
{
    private PlayerInfoManager_Server server;

    public void Init(PlayerInfoManager playerInfoManager)
    {
        server = (PlayerInfoManager_Server)playerInfoManager;
    }

    public void SetName(string name)
    {
        Debug.LogWarning("Non-Command Set Name");
        CmdSetName(name);
    }

    [Command]
    public void CmdSetName(string name)
    {
        Debug.LogWarning("CMD Set Name");
        server.SetName(name);
    }

    public string GetName()
    {
        return server.GetName();
    }

}