using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class LobbyManager_Server : NetworkBehaviour, LobbyManager
{
    private LobbyManager_Client client;

    private Dictionary<string, NetworkConnection> players;
    private string host;

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

        host = null;
    }

    public void AddPlayer(NetworkConnection playerConnection)
    {
        Debug.LogWarning("Add Player Server");
        client.TargetRequestPlayerControllerNetID(playerConnection);
        
    }
    
    [Command]
    public void CmdRecievePlayerControllerNetID(NetworkInstanceId netID)
    {
        Debug.LogWarning(netID);

        GameObject localPlayer = NetworkServer.FindLocalObject(netID);
        NetworkConnection connection = localPlayer.GetComponent<NetworkIdentity>().connectionToClient;
        PlayerInfoManager_Proxy playerInfo = localPlayer.GetComponent<PlayerInfoManager_Proxy>();
        string name = playerInfo.GetName();

        players.Add(name, connection);
        Debug.LogWarning(players.Count);

        if (host == null)
            host = name;

        RequestUpdatePlayerList();
    }

    

    private void RequestUpdatePlayerList()
    {
        string[] playerList = new string[players.Count];
        List<string> temp = new List<string>(players.Keys);

        for(int loop = 0; loop < players.Count; loop++)
        {
            playerList[loop] = temp[loop];
        }

        client.RpcUpdatePlayerList(playerList, host);
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
