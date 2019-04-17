﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    void StartPrimaryFire(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetLookDirection);

    void StopPrimaryFire();

    void StartAltFire(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetLookDirection);

    void StopAltFire();

    void StartReload();

    void StopReload();

    string GetWeaponName();

    int GetAmmoPool();

    int GetWeaponAmmo();

    Vector3 GetGunPositionData();
}