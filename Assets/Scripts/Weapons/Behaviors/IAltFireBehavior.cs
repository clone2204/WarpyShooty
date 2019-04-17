using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAltFireBehavior 
{
    void Init(System.Func<int, bool> ConsumeAmmo, IPrimaryFireBehavior primaryFireBehavior, IReloadBehavior reloadBehavior);

    void StartAltFire(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetLookDirection);

    void StopAltFire();

    bool GetAltFireActive();
}
