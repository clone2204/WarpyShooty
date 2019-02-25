using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager_Client : NetworkBehaviour, LobbyManager
{
    private LobbyManager_Server server;

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

    private string test = "TEST STRING ALPHA!";
    public void RequestPlayerControllerNetID()
    {
        StartCoroutine(WaitForLocalPlayer());
    }

    IEnumerator WaitForLocalPlayer()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("localPlayer") != null);

        NetworkInstanceId netID = GameObject.FindGameObjectWithTag("localPlayer").GetComponent<NetworkIdentity>().netId;
        Debug.LogWarning("NetID = " + netID);
        CmdSendPlayerControllerNetID(netID);
    }

    [Command]
    public void CmdSendPlayerControllerNetID(NetworkInstanceId netID)
    {
        server.SendPlayerControllerNetID(netId);
    }
}
