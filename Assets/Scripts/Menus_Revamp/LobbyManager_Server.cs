using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class LobbyManager_Server : NetworkBehaviour, LobbyManager
{
    private LobbyManager_Client client;

    private Dictionary<int, string> players;
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
        players = new Dictionary<int, string>();
        client = (LobbyManager_Client)lobbyManager;

        host = null;
    }

    public void AddPlayer(NetworkConnection playerConnection, short controllerID)
    {
        GameObject player = playerConnection.playerControllers[controllerID].gameObject;
        PlayerInfoManager_Proxy playerInfo = player.GetComponent<PlayerInfoManager_Proxy>();
        playerInfo.Init(null);

        Debug.LogWarning(playerInfo.GetName());
        
        
        
        
        
        
        
        /*
        Debug.LogWarning("Add Player Server");
        players.Add(playerConnection.connectionId, null);
        StartCoroutine(WaitForClientInfo(playerConnection, players));
    
        playerConnection.playerControllers[0].
        if (players.Count == 1)
        {
            client.AddPlayer(playerConnection);
        }
        */
    }

    private IEnumerator WaitForClientInfo(NetworkConnection conn, Dictionary<int, string> playerList)
    {
        Debug.LogWarning("Wait for Client Info");
        yield return new WaitUntil(() => playerList[conn.connectionId] != null);

        Debug.LogWarning("Client Info Processed");
        RequestUpdatePlayerList();
    }

    [Command]
    public void CmdRecievePlayerObject(NetworkInstanceId netID)
    {
        Debug.LogWarning("Client Info Recieved");
        GameObject playerObject = NetworkServer.FindLocalObject(netID);
        NetworkConnection connection = playerObject.GetComponent<NetworkIdentity>().connectionToServer;
       
        PlayerInfoManager_Proxy playerInfo = playerObject.GetComponent<PlayerInfoManager_Proxy>();

        players[connection.connectionId] = playerInfo.GetName();
    }
    
    [Command]
    public void CmdRecievePlayerControllerNetID(NetworkInstanceId netID)
    {
        Debug.LogWarning("Server Recieve Player ID: " + netID);

        GameObject localPlayer = NetworkServer.FindLocalObject(netID);
        NetworkConnection connection = localPlayer.GetComponent<NetworkIdentity>().connectionToClient;
        PlayerInfoManager_Proxy playerInfo = localPlayer.GetComponent<PlayerInfoManager_Proxy>();
        string name = playerInfo.GetName();

        //players.Add(name, connection);

        if (host == null)
            host = name;

        RequestUpdatePlayerList();
    }

    

    private void RequestUpdatePlayerList()
    {
        string[] playerList = new string[players.Count];
        List<string> temp = new List<string>(players.Values);

        for(int loop = 0; loop < players.Count; loop++)
        {
            playerList[loop] = temp[loop];
        }

        client.RpcUpdatePlayerList(playerList, host);
    }

    //Currently does not work because LobbyManager_Server does not exist on client player object
    //Solution will most likely require a networked playerInfo system.
    //PlayerInfo -> PlayerInfo_Proxy -> PlayerInfo_Client -> PlayerInfo_Server

    
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
