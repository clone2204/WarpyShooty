using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Gun : NetworkBehaviour, IGun
{
    [SerializeField] protected string gunName;
    [SerializeField] protected Vector3 gunViewLocation;
    
    [SerializeField] protected int maxAmmoPool;
    [SerializeField] protected int maxAmmo;
    [SyncVar] [SerializeField] protected int currentAmmoPool;
    [SyncVar] [SerializeField] protected int currentAmmo;
    
    protected IPrimaryFireBehavior primaryFireBehavior;
    protected IAltFireBehavior altFireBehavior;
    protected IReloadBehavior reloadBehavior;
    
    public void Start()
    {
        primaryFireBehavior = GetComponent<IPrimaryFireBehavior>();
        altFireBehavior = GetComponent<IAltFireBehavior>();
        reloadBehavior = GetComponent<IReloadBehavior>();

        primaryFireBehavior.Init(ConsumeAmmo, altFireBehavior, reloadBehavior);
        altFireBehavior.Init(ConsumeAmmo, primaryFireBehavior, reloadBehavior);
        reloadBehavior.Init(TransferAmmoFromPool, primaryFireBehavior, altFireBehavior);
    }

    public void StartPrimaryFire(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, Func<Vector3> GetLookDirection)
    {
        primaryFireBehavior.PrimaryFireStart(player, GetSpawnLocation, GetLookDirection);
    }

    public void StopPrimaryFire()
    {
        primaryFireBehavior.PrimaryFireStop();
    }

    public void StartAltFire(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, Func<Vector3> GetLookDirection)
    {
        altFireBehavior.StartAltFire(player, GetSpawnLocation, GetLookDirection);
    }

    public void StopAltFire()
    {
        altFireBehavior.StopAltFire();
    }

    public void StartReload()
    {
        reloadBehavior.StartReload();
    }

    public void StopReload()
    {
        reloadBehavior.StopReload();
    }

    public string GetWeaponName()
    {
        return gunName;
    }

    public Vector3 GetGunPositionData()
    {
        return gunViewLocation;
    }

    public int GetAmmoPool()
    {
        return currentAmmoPool;
    }

    public int GetWeaponAmmo()
    {
        return currentAmmo;
    }

    
    public bool ConsumeAmmo(int amount)
    {
        if (currentAmmo <= 0)
            return false;

        currentAmmo -= amount;
        return true;
    }

    public bool TransferAmmoFromPool(int amount)
    {
        if (amount == -1)
        {
            amount = maxAmmo;
        }

        if (amount > currentAmmoPool)
        {
            amount = currentAmmoPool;
        }
        else if ((amount + currentAmmo) > maxAmmo)
        {
            amount = maxAmmo - currentAmmo;
        }

        Debug.LogWarning("TRANSFER");
        currentAmmoPool -= amount;
        currentAmmo += amount;
        return true;
    }
}
