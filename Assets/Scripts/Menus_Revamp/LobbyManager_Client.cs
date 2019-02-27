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

    public void AddPlayer(NetworkConnection playerConnection)
    {
        throw new System.NotImplementedException();
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

    [TargetRpc]
    public void TargetRequestPlayerControllerNetID(NetworkConnection conn)
    {
        StartCoroutine(WaitForLocalPlayer());
    }

    private IEnumerator WaitForLocalPlayer()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("localPlayer") != null);

        NetworkInstanceId netID = GameObject.FindGameObjectWithTag("localPlayer").GetComponent<NetworkIdentity>().netId;
        Debug.LogWarning("NetID = " + netID);

        server.CmdRecievePlayerControllerNetID(netID);
    }
    
    [ClientRpc]
    public void RpcUpdatePlayerList(string[] players, string host)
    {
        List<string> playerList = new List<string>(players);
        listManager.UpdatePlayerList(playerList, host);
    }
}
