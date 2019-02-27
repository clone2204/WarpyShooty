using System.Collections;
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
        CmdSetName(name);
    }

    [Command]
    public void CmdSetName(string name)
    {
        server.SetName(name);
    }

    public string GetName()
    {
        return server.GetName();
    }

}
