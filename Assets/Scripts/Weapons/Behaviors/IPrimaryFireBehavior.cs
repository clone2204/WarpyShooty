using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrimaryFireBehavior
{
    void Init(System.Func<int, bool> ConsumeAmmo, IAltFireBehavior altFireBehavior, IReloadBehavior reloadBehavior);

    void PrimaryFireStart(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetLookDirection);

    void PrimaryFireStop();

    bool GetPrimaryFireActive();
}
