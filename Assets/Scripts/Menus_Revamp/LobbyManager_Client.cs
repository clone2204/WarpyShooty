using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager_Client : NetworkBehaviour, LobbyManager
{
    private LobbyManager_Server server;
    private PlayerListManager listManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(LobbyManager lobbyManager)
    {
        server = (LobbyManager_Server)lobbyManager;

        if(isServer)
        {
            listManager = GameObject.Find("HostLobbyCanvas").GetComponent<PlayerListManager>();
        }
        else
        {
            listManager = GameObject.Find("LobbyCanvas").GetComponent<PlayerListManager>();
        }
    }

    public void AddPlayer(NetworkConnection playerConnection, short controllerID)
    {
        Debug.LogWarning("Add Player Client");

        
        //StartCoroutine(WaitForLocalPlayerSpawn(playerConnection));
    }

    public void BanPlayer()
    {
        throw new System.NotImplementedException();
    }

    public void KickPlayer(NetworkConnection playerConnection)
    {
        throw new System.NotImplementedException();
    }

    public void RemovePlayer()
    {
        throw new System.NotImplementedException();
    }

    public void SwapPlayers()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator WaitForLocalPlayerSpawn(NetworkConnection playerConnection)
    {
        Debug.LogWarning("Wait for Local Player Object Spawn");
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("localPlayer") != null);


        Debug.LogWarning(ClientScene.localPlayers.Count);

        NetworkInstanceId netID = GameObject.FindGameObjectWithTag("localPlayer").GetComponent<NetworkIdentity>().netId;
        Debug.LogWarning("Local Player Spawned with NetID = " + netID);

        server.CmdRecievePlayerObject(netID);
    }

    [ClientRpc]
    public void RpcUpdatePlayerList(string[] players, string host)
    {
        Debug.LogWarning("Client Update Player List");

        List<string> playerList = new List<string>(players);
        listManager.UpdatePlayerList(playerList, host);
    }

    [TargetRpc]
    public void TargetRpcGetPlayerName(NetworkConnection conn)
    {
        string name = GameObject.Find("_SCRIPTS_").GetComponent<SettingsManager>().GetPlayerName();
        Debug.LogWarning("command");
        server.CmdGetPlayerName(name);
    }

}
