using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class LobbyManager_Server : NetworkBehaviour, LobbyManager
{
    private LobbyManager_Client client;

    private Dictionary<string, NetworkConnection> players;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(LobbyManager lobbyManager)
    {
        Debug.LogWarning("Server Init");
        players = new Dictionary<string, NetworkConnection>();
        client = (LobbyManager_Client)lobbyManager;
    }

    public void AddPlayer(NetworkConnection playerConnection)
    {
        TargetRequestPlayerControllerNetID(playerConnection);
    }

    public void SendPlayerControllerNetID(NetworkInstanceId netID)
    {
        Debug.LogWarning("netID = " + netID);
        GameObject localPlayer = NetworkServer.FindLocalObject(netID);
        Debug.LogWarning("localPlayer = " + localPlayer.gameObject.name);
        NetworkConnection connection = localPlayer.GetComponent<NetworkIdentity>().connectionToClient;
        Debug.LogWarning("connection = " + connection);
        PlayerInfoManager_Proxy playerInfo = localPlayer.GetComponent<PlayerInfoManager_Proxy>();
        Debug.LogWarning("playerInfo = " + playerInfo);
        string name = playerInfo.GetName();

        players.Add(name, connection);
        Debug.LogWarning(players.Count);
    }

    [TargetRpc]
    public void TargetRequestPlayerControllerNetID(NetworkConnection conn)
    {
        client.RequestPlayerControllerNetID();
    }

    //Currently does not work because LobbyManager_Server does not exist on client player object
    //Solution will most likely require a networked playerInfo system.
    //PlayerInfo -> PlayerInfo_Proxy -> PlayerInfo_Client -> PlayerInfo_Server

    [TargetRpc]
    public void TargetRpcGetPlayerName(NetworkConnection conn)
    {
        string name = GameObject.Find("_SCRIPTS_").GetComponent<SettingsManager>().GetPlayerName();
        Debug.LogWarning("command");
        CmdGetPlayerName(name);
    }

    [Command]
    public void CmdGetPlayerName(string name)
    {
        Debug.LogWarning("COMMAND");
        Debug.LogWarning(name);
    }

    public void BanPlayer()
    {
        throw new NotImplementedException();
    }

    public void KickPlayer(NetworkConnection playerConnection)
    {
        throw new NotImplementedException();
    }

    public void RemovePlayer()
    {
        throw new NotImplementedException();
    }

    public void SwapPlayers()
    {
        throw new NotImplementedException();
    }

}
