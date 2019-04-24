using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RealPlayer : GamePlayer 
{
    private Camera playerPOV;
    private PlayerHUDManager playerHUD;
    private IWarp warpManager;
    
    [SyncVar] private string playerName;

    void Start()
    {
        gameManager = GameObject.Find("_SCRIPTS_").GetComponent<GameManager>();

        playerPOV = GetComponentInChildren<Camera>();
        playerHUD = GameObject.Find("Menues").transform.Find("PlayerHud").GetComponent<PlayerHUDManager>();
        playerHUD.SetupCameras(GetComponentInChildren<Camera>());

        warpManager = GetComponent<Warp>();
        weaponManager = GetComponent<WeaponManager>();
    }

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

    public override void SetHealth(int health)
    {
        base.SetHealth(health);

        TargetUpdatePlayerHealth(connectionToClient);
    }

    public override void DamagePlayer(int damage, GamePlayer damager)
    {
        if (!isServer)
            return;

        if ((damager.GetTeam() == playerTeam) && (damager.Equals(this)))
            return;

        playerHealth -= damage;
        TargetUpdatePlayerHealth(connectionToClient);

        if (!(damager is RealPlayer))
            return;

        if (playerHealth <= 0)
            base.gameManager.KillPlayer(this, (RealPlayer)damager);
    }

    public override Vector3 GetPlayerLookDirection()
    {
        float xrot = Mathf.Sin(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y) * Mathf.Cos(Mathf.Deg2Rad * this.playerPOV.transform.rotation.eulerAngles.x);
        float zrot = Mathf.Cos(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y) * Mathf.Cos(Mathf.Deg2Rad * this.playerPOV.transform.rotation.eulerAngles.x);
        float yrot = -Mathf.Sin(Mathf.Deg2Rad * playerPOV.transform.rotation.eulerAngles.x);

        return new Vector3(xrot, yrot, zrot);
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
