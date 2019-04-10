using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public abstract class Gun : NetworkBehaviour, IGun
{
    [SerializeField] protected string gunName;

    [SerializeField] protected Vector3 gunViewLocation;
    [SerializeField] protected int damage;

    [SerializeField] protected float refireTime;

    [SerializeField] protected int startAmmoPoolCount;
    [SerializeField] protected int magazineSize;
    [SerializeField] protected float reloadTime;

    protected bool isReloadCooldown;
    protected bool isRefireCooldown;

    [SyncVar] protected AmmoData ammoData;

    public struct AmmoData
    {
        public int currentAmmoPoolCount;
        public int currentAmmoCount;

        public bool hasBeenInitialized;

        public AmmoData(int currentAmmoCount, int ammoPoolCount)
        {
            this.currentAmmoPoolCount = ammoPoolCount;
            this.currentAmmoCount = currentAmmoCount;

            this.hasBeenInitialized = true;
        }

    }
    
    public void Start()
    {
        if(!ammoData.hasBeenInitialized)
        {
            this.ammoData = new AmmoData(this.magazineSize, this.startAmmoPoolCount);
        }
        
    }

    public abstract void StartPrimaryFire(Func<Vector3> GetLookDirection);
    public abstract void StopPrimaryFire();
    public abstract void StartAltFire(Func<Vector3> GetLookDirection);
    public abstract void StopAltFire();
    public abstract void StartReload();
    public abstract void StopReload();

    public string GetWeaponName()
    {
        return gunName;
    }

    public int GetAmmoPool()
    {
        return ammoData.currentAmmoPoolCount;
    }

    public int GetWeaponAmmo()
    {
        return ammoData.currentAmmoCount;
    }

    public Vector3 GetGunPositionData()
    {
        return gunViewLocation;
    }

}
