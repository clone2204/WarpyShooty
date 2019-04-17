using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayerManager : NetworkBehaviour
{
    private GameManager gameManager;

    private PlayerHUDManager playerHUD;
    private IWarp warpManager;
    private GunManager gunManager;

    [SyncVar] private string playerName;
    [SyncVar] private GameManager.Team playerTeam;
    [SyncVar] private int playerHealth;


    void Start()
    {
        gameManager = GameObject.Find("_SCRIPTS_").GetComponent<GameManager>();

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

        Debug.LogWarning("SETUP PLAYER ON SERVER: " + playerName);
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

    public void SetHealth(int health)
    {
        if (!isServer)
            return;

        playerHealth = health;
        TargetUpdatePlayerHealth(connectionToClient);
    }

    public int GetHealth()
    {
        return playerHealth;
    }

    public void DamagePlayer(int damage, GamePlayerManager damager)
    {
        if (!isServer)
            return;

        if ((damager.GetTeam() == playerTeam) && (damager.Equals(this)))
            return;

        playerHealth -= damage;
        TargetUpdatePlayerHealth(connectionToClient);

        if (playerHealth <= 0)
            gameManager.KillPlayer(this, damager);
    }

    public void EnablePlayer()
    {
        TargetEnablePlayerController(this.connectionToClient);
    }

    public void DisablePlayer()
    {
        TargetDisablePlayerController(this.connectionToClient);
    }

    //=================================================================================================
    //Player Control Functions
    //=================================================================================================

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
        gunManager.StartPrimaryFire(this);
    }

    public void StopPrimaryFire()
    {
        gunManager.StopPrimaryFire();
    }

    public void StartAltFire()
    {
        gunManager.StartAltFire(this);
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
    //Client Functions
    //=================================================================================================


    [TargetRpc]
    private void TargetSetupPlayer(NetworkConnection playerConn)
    {
        Debug.LogWarning("SETUP PLAYER");
        playerHUD.SetPlayerName(playerName);
        playerHUD.SetPlayerTeam(playerTeam);
    }

    [TargetRpc]
    private void TargetUpdatePlayerHealth(NetworkConnection playerConn)
    {
        playerHUD.SetPlayerHealth(playerHealth);
    }

    [TargetRpc]
    private void TargetEnablePlayerController(NetworkConnection target)
    {
        GetComponent<LocalPlayerController>().enabled = true;
        playerHUD.SetToPlayerView();
    }

    [TargetRpc]
    private void TargetDisablePlayerController(NetworkConnection networkConnection)
    {
        GetComponent<LocalPlayerController>().enabled = false;
        playerHUD.SetToObserverView();
    }
}
