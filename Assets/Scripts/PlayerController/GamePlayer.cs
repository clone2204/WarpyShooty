using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour
{
    private Camera playerPOV;
    private PlayerHUDManager playerHUD;
    private IWarp warpManager;

    [SyncVar] private string playerName;
    [SyncVar] protected int playerHealth;
    [SyncVar] protected GameManager.Team playerTeam;

    protected GameManager gameManager;
    protected WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("_SCRIPTS_").GetComponentInChildren<GameManager>();
        playerPOV = GetComponentInChildren<Camera>();
        playerHUD = GameObject.Find("Menues").transform.Find("PlayerHud").GetComponent<PlayerHUDManager>();
        playerHUD.SetupCameras(GetComponentInChildren<Camera>());

        warpManager = GetComponent<Warp>();
        weaponManager = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        playerHUD.SetWeaponName(weaponManager.GetWeaponName());
        playerHUD.SetWeaponAmmo(weaponManager.GetWeaponCurrentAmmo(), weaponManager.GetWeaponAmmoPool());
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

    public void DamagePlayer(int damage, GamePlayer damager)
    {
        if (!isServer)
            return;

        //If damager is null, damager is a target dummy, so we dont need to check for team and can just damage the player
        if (damager != null && (damager.GetTeam() == playerTeam) && (damager.Equals(this)))
            return;

        playerHealth -= damage;
        TargetUpdatePlayerHealth(connectionToClient);

        if (playerHealth <= 0)
            gameManager.KillPlayer(this, damager);
    }

    public GameManager.Team GetTeam()
    {
        return playerTeam;
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

    public void StartPrimaryFire()
    {
        weaponManager.StartPrimaryFire(this);
    }

    public void StopPrimaryFire()
    {
        weaponManager.StopPrimaryFire();
    }

    public void StartAltFire()
    {
        weaponManager.StartAltFire(this);
    }

    public void StopAltFire()
    {
        weaponManager.StopAltFire();
    }

    public void StartReload()
    {
        weaponManager.StartReload();
    }

    public void StopReload()
    {
        weaponManager.StopReload();
    }

    public void StartWeaponPickup()
    {
        weaponManager.StartWeaponPickup();
    }

    public void StopWeaponPickup()
    {
        weaponManager.StopWeaponPickup();
    }

    public void WarpPlayer()
    {
        warpManager.WarpPlayer();
    }

    public void WarpPlayerToLocation(Warp.Location location)
    {
        WarpPlayerToLocation(location);
    }

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
