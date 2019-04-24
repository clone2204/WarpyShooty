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

    void StartPrimaryFire(GamePlayer player, System.Func<Vector3> GetSpawnLocation);

    void StopPrimaryFire();

    void StartAltFire(GamePlayer player, System.Func<Vector3> GetSpawnLocation);

    void StopAltFire();

    void StartReload();

    void StopReload();

    
}
