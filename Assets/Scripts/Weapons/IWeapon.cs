using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IWeapon
{
    string GetWeaponName();

    GameObject GetWeaponObject();

    void SetWeaponPosition(Transform weaponPort);

    int GetCurrentAmmoPool();

    int GetCurrentAmmo();

    void StartPrimaryFire(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetLookDirection);

    void StopPrimaryFire();

    void StartAltFire(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetLookDirection);

    void StopAltFire();

    void StartReload();

    void StopReload();

    
}
