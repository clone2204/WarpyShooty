using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAltFireBehavior 
{
    void Init(IPrimaryFireBehavior primaryFireBehavior, IAmmoBehavior ammoBehavior);

    void StartAltFire(GamePlayer player, System.Func<Vector3> GetSpawnLocation);

    void StopAltFire();

    bool GetAltFireActive();
}
