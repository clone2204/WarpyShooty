using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayerManager : NetworkBehaviour, IGamePlayerManager
{
    private GamePlayerManager_Server realGamePlayer;

    private PlayerHUDManager playerHUD;

    void Start()
    {
        DontDestroyOnLoad(this);
        realGamePlayer = GetComponent<GamePlayerManager_Server>();
        playerHUD = GameObject.Find("Menues").transform.Find("PlayerHud").GetComponent<PlayerHUDManager>();
        playerHUD.SetupCameras(GetComponentInChildren<Camera>());
    }

    
    public void SetupPlayer(string playerName, GameManager.Team playerTeam)
    {
        if(!isServer)
            return;
        
        if(realGamePlayer == null)
            realGamePlayer = GetComponent<GamePlayerManager_Server>();

        realGamePlayer.SetupPlayer(playerName, playerTeam);
    }

    [TargetRpc]
    public void TargetSetupPlayer(NetworkConnection playerConn)
    {
        playerHUD.SetPlayerName(GetName());
        playerHUD.SetPlayerTeam(GetTeam());
    }

    public string GetName()
    {
        return realGamePlayer.GetName();
    }

    public GameManager.Team GetTeam()
    {
        return realGamePlayer.GetTeam();
    }

    public void SpawnPlayer(Vector3 spawnPoint)
    {
        
        realGamePlayer.SpawnPlayer(spawnPoint);
    }

    public void DespawnPlayer()
    {
        realGamePlayer.DespawnPlayer();
    }

    public void SetHealth(int health)
    {
        if (!isServer)
            return;

        realGamePlayer.SetHealth(health);
    }

    public int GetHealth()
    {
        return realGamePlayer.GetHealth();
    }

    [TargetRpc]
    public void TargetClientSpawnTasks(NetworkConnection target)
    {
        Debug.LogWarning("Client Enable");
        GetComponent<LocalPlayerController>().enabled = true;
        playerHUD.SetToPlayerView();

        playerHUD.SetPlayerHealth(GetHealth());
    }

    [TargetRpc]
    public void TargetClientDespawnTasks(NetworkConnection networkConnection)
    {
        GetComponent<LocalPlayerController>().enabled = false;
        playerHUD.SetToObserverView();
    }
}
