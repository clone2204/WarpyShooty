using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class GamePlayer : NetworkBehaviour
{
    [SerializeField] [SyncVar] protected int playerHealth;
    [SerializeField] [SyncVar] protected GameManager.Team playerTeam;

    protected GameManager gameManager;
    protected WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("_SCRIPTS_").GetComponentInChildren<GameManager>();
        weaponManager = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //=================================================================================================
    //Interact Functions
    //=================================================================================================

    public abstract void DamagePlayer(int damage, GamePlayer damager);

    public GameManager.Team GetTeam()
    {
        return playerTeam;
    }

    public virtual void SetHealth(int health)
    {
        if (!isServer)
            return;

        playerHealth = health;
    }

    public int GetHealth()
    {
        return playerHealth;
    }

    public abstract Vector3 GetPlayerLookDirection();

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


}
