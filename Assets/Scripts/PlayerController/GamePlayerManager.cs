using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayerManager : NetworkBehaviour
{
    private PlayerHUDManager playerHUD;

    private IWarp warpManager;
    private GunManager gunManager;

    [SyncVar] private string playerName;
    [SyncVar] private GameManager.Team playerTeam;
    [SyncVar] private int playerHealth;


    void Start()
    {
        DontDestroyOnLoad(this);
        playerHUD = GameObject.Find("Menues").transform.Find("PlayerHud").GetComponent<PlayerHUDManager>();
        playerHUD.SetupCameras(GetComponentInChildren<Camera>());

        warpManager = GetComponent<Warp>();
        gunManager = GetComponent<GunManager>();
    }

    private void Update()
    {
        playerHUD.SetWeaponName(gunManager.GetWeaponName());
        playerHUD.SetWeaponAmmo(gunManager.GetWeaponCurrentAmmo(), gunManager.GetWeaponMaxAmmo());
    }

    //=================================================================================================
    //Interact Functions
    //=================================================================================================

    public void SetupPlayer(string playerName, GameManager.Team playerTeam)
    {
        if (!isServer)
            return;

        this.playerName = playerName;
        this.playerTeam = playerTeam;

        TargetSetupPlayer(this.connectionToClient);
    }

    public string GetName()
    {
        return playerName;
    }

    public GameManager.Team GetTeam()
    {
        return playerTeam;
    }

    public void SpawnPlayer(Vector3 spawnPoint)
    {
        if (!isServer)
            return;

        Debug.LogWarning("Spawn Player");
        transform.position = spawnPoint;
        this.playerHealth = 100;

        TargetClientSpawnTasks(this.connectionToClient);
    }

    public void DespawnPlayer()
    {
        if (!isServer)
            return;

        Debug.LogWarning("DESPAWN");
        transform.position = new Vector3(0, 10, 0);

        TargetClientDespawnTasks(this.connectionToClient);
    }

    public void SetHealth(int health)
    {
        if (!isServer)
            return;

        playerHealth = health;
    }

    public int GetHealth()
    {
        return playerHealth;
    }

    public void WarpPlayer()
    {
        warpManager.WarpPlayer();
    }

    public void WarpPlayerToLocation(Warp.Location location)
    {
        WarpPlayerToLocation(location);
    }

    public void StartPrimaryFire()
    {
        gunManager.StartPrimaryFire();
    }

    public void StopPrimaryFire()
    {
        gunManager.StopPrimaryFire();
    }

    public void StartAltFire()
    {
        gunManager.StartAltFire();
    }

    public void StopAltFire()
    {
        gunManager.StopAltFire();
    }

    public void StartReload()
    {
        gunManager.StartReload();
    }

    public void StopReload()
    {
        gunManager.StopReload();
    }

    public void StartWeaponPickup()
    {
        gunManager.StartWeaponPickup();
    }

    public void StopWeaponPickup()
    {
        gunManager.StopWeaponPickup();
    }

    //=================================================================================================
    //Server Functions
    //=================================================================================================



    //=================================================================================================
    //Interface Functions
    //=================================================================================================


    [TargetRpc]
    private void TargetSetupPlayer(NetworkConnection playerConn)
    {
        playerHUD.SetPlayerName(playerName);
        playerHUD.SetPlayerTeam(playerTeam);
    }

    [TargetRpc]
    private void TargetClientSpawnTasks(NetworkConnection target)
    {
        Debug.LogWarning("Client Enable");
        GetComponent<LocalPlayerController>().enabled = true;
        playerHUD.SetToPlayerView();

        playerHUD.SetPlayerHealth(playerHealth);
    }

    [TargetRpc]
    private void TargetClientDespawnTasks(NetworkConnection networkConnection)
    {
        GetComponent<LocalPlayerController>().enabled = false;
        playerHUD.SetToObserverView();
    }
}
