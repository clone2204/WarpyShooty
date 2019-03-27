using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour, IPlayerManager
{
    private IPlayerManager realInfoManager;

    private NetworkLobbyPlayer lobbyPlayer;

    void Start()
    {
        realInfoManager = gameObject.GetComponent<PlayerManager_Server>();
        lobbyPlayer = gameObject.GetComponent<NetworkLobbyPlayer>();

        Debug.LogWarning("DING1");
        if (isLocalPlayer)
        {
            Debug.LogWarning("DANG1");
            gameObject.tag = "localPlayer";

            string name = GameObject.Find("_SCRIPTS_").GetComponent<SettingsManager>().GetPlayerName();
            CmdSetName(name); 
        }

        lobbyPlayer.readyToBegin = true;
    }

    public void Init(IPlayerManager playerInfoManager)
    {
        Start();
    }

    public void SetName(string name)
    {
        if (!isServer)
            return;

        realInfoManager.SetName(name);
    }

    [Command]
    public void CmdSetName(string name)
    {
        SetName(name);
    }

    public string GetName()
    {
        return realInfoManager.GetName();
    }

    public void SetTeam(GameManager.Team team)
    {
        if (!isServer)
            return;

        realInfoManager.SetTeam(team);
    }

    public GameManager.Team GetTeam()
    {
        return realInfoManager.GetTeam();
    }

    public void SetPlayerObjectID(NetworkInstanceId playerObjectID)
    {
        if (!isServer)
            return;

        realInfoManager.SetPlayerObjectID(playerObjectID);
    }

    public NetworkInstanceId GetPlayerObjectID()
    {
        return realInfoManager.GetPlayerObjectID();
    }

    public void SetPlayerConnection(NetworkConnection networkConnection)
    {
        if (!isServer)
            return;

        realInfoManager.SetPlayerConnection(networkConnection);
    }

    public NetworkConnection GetPlayerConnection()
    {
        return realInfoManager.GetPlayerConnection();
    }

    public void SpawnPlayer()
    {
        if (!isServer)
            return;

        realInfoManager.SpawnPlayer();
    }
}
