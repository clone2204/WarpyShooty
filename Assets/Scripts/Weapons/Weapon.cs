using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Weapon : NetworkBehaviour, IWeapon
{
    [SerializeField] protected string weaponName;
    [SerializeField] protected Vector3 weaponViewOffset;
    
    protected IPrimaryFireBehavior primaryFireBehavior;
    protected IAltFireBehavior altFireBehavior;
    protected IAmmoBehavior ammoBehavior;
    
    public void Start()
    {
        primaryFireBehavior = GetComponent<IPrimaryFireBehavior>();
        altFireBehavior = GetComponent<IAltFireBehavior>();
        ammoBehavior = GetComponent<IAmmoBehavior>();

        primaryFireBehavior.Init(altFireBehavior, ammoBehavior);
        altFireBehavior.Init(primaryFireBehavior, ammoBehavior);
        ammoBehavior.Init(primaryFireBehavior, altFireBehavior);
    }

    public string GetWeaponName()
    {
        return weaponName;
    }

    public GameObject GetWeaponObject()
    {
        return gameObject;
    }
     
    public void SetWeaponPosition(Transform weaponPort)
    {
        transform.parent = weaponPort;
        transform.localPosition = weaponViewOffset;
        transform.rotation = new Quaternion();
    }

    public void StartPrimaryFire(GamePlayer player, System.Func<Vector3> GetSpawnLocation)
    {
        primaryFireBehavior.PrimaryFireStart(player, GetSpawnLocation);
    }

    public void StopPrimaryFire()
    {
        primaryFireBehavior.PrimaryFireStop();
    }

    public void StartAltFire(GamePlayer player, System.Func<Vector3> GetSpawnLocation)
    {
        altFireBehavior.StartAltFire(player, GetSpawnLocation);
    }

    public void StopAltFire()
    {
        altFireBehavior.StopAltFire();
    }

    public void StartReload()
    {
        ammoBehavior.StartReload();
    }

    public void StopReload()
    {
        ammoBehavior.StopReload();
    }

    public int GetCurrentAmmoPool()
    {
        return ammoBehavior.GetCurrentAmmoPool();
    }

    public int GetCurrentAmmo()
    {
        return ammoBehavior.GetCurrentAmmo();
    }

    
    

    
    
}
