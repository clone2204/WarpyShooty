using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobbyPlayerManager : NetworkBehaviour, ILobbyPlayerManager
{
    private ILobbyPlayerManager realInfoManager;

    private NetworkLobbyPlayer lobbyPlayer;
    private PlayerHUDManager playerhud;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        realInfoManager = gameObject.GetComponent<LobbyPlayerManager_Server>();
        realInfoManager.Init();

        lobbyPlayer = gameObject.GetComponent<NetworkLobbyPlayer>();

        realInfoManager.Init();

        if (isLocalPlayer)
        {
            gameObject.tag = "localPlayer";

            string name = GameObject.Find("_SCRIPTS_").GetComponent<SettingsManager>().GetPlayerName();
            CmdSetName(name);

        }

        //GetComponent<NetworkLobbyPlayer>().SendReadyToBeginMessage();
        //lobbyPlayer.readyToBegin = true;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        if (scene.name != "TitleScreen")
        {
            playerhud = GameObject.Find("PlayerHud").GetComponent<PlayerHUDManager>();
        }
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

    public void ReadyPlayer()
    {
        realInfoManager.ReadyPlayer();
    }
}
