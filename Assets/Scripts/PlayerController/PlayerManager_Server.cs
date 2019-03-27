using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerManager_Server : NetworkBehaviour, IPlayerManager
{
    private PlayerManager clientProxy;

    [SyncVar] private string playerName;
    [SyncVar] private GameManager.Team team;

    [SyncVar] public NetworkInstanceId playerObjectID;
    public NetworkConnection playerConnection;

    private GameObject playerObject;
        
    public void Init()
    {
        clientProxy = GetComponent<PlayerManager>();

        Debug.LogWarning("DANDF");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.LogWarning("Scene Loaded: " + scene.name);
        if (scene.name != "TitleScreen")
        {
            foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                NetworkIdentity playerNetwork = player.GetComponent<NetworkIdentity>();

                if(playerNetwork.Equals(playerConnection))
                {
                    Debug.LogWarning("DINGDING");
                    playerObject = player;
                }
            }
        }
    }

    public void SetName(string name)
    {
        Debug.Log("Set Name: " + name);
        this.playerName = name;
    }
    
    public string GetName()
    {
        return playerName;
    }

    public void SetTeam(GameManager.Team team)
    {
        this.team = team;
    }

    public GameManager.Team GetTeam()
    {
        return this.team;
    }

    public void SetPlayerObjectID(NetworkInstanceId playerObjectID)
    {
        this.playerObjectID = playerObjectID;
    }

    public NetworkInstanceId GetPlayerObjectID()
    {
        return this.playerObjectID;
    }

    public void SetPlayerConnection(NetworkConnection playerConnection)
    {
        this.playerConnection = playerConnection;
    }

    public NetworkConnection GetPlayerConnection()
    {
        return this.playerConnection;
    }

    public void SetPlayerObject(GameObject playerObject)
    {
        Debug.LogWarning("SET PLAYER: " + playerObject);
        this.playerObject = playerObject;
    }

    public GameObject GetPlayerObject()
    {
        return this.playerObject;
    }

    public void SpawnPlayer(Vector3 respawnPoint)
    {
        Debug.LogWarning("SPAWN PLAYER AT: " + respawnPoint);
        playerObject.transform.position = respawnPoint;
    }
}
