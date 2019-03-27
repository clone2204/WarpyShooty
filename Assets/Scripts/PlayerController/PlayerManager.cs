using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerManager : NetworkBehaviour, IPlayerManager
{
    private IPlayerManager realInfoManager;

    private NetworkLobbyPlayer lobbyPlayer;
    private PlayerHUDManager playerhud;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        realInfoManager = gameObject.GetComponent<PlayerManager_Server>();
        lobbyPlayer = gameObject.GetComponent<NetworkLobbyPlayer>();

        realInfoManager.Init();

        if (isLocalPlayer)
        {
            gameObject.tag = "localPlayer";

            string name = GameObject.Find("_SCRIPTS_").GetComponent<SettingsManager>().GetPlayerName();
            CmdSetName(name);
        }

        lobbyPlayer.readyToBegin = true;
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

    public void SetPlayerObject(GameObject playerObject)
    {
        if (!isServer)
            return;

        realInfoManager.SetPlayerObject(playerObject);
    }

    public GameObject GetPlayerObject()
    {
        return realInfoManager.GetPlayerObject();
    }

    public void SpawnPlayer(Vector3 respawnPoint)
    {
        if (!isServer)
            return;

        realInfoManager.SpawnPlayer(respawnPoint);
    }
}
