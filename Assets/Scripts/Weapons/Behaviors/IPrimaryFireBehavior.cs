using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrimaryFireBehavior
{
    void Init(IAltFireBehavior altFireBehavior, IAmmoBehavior ammoBehavior);

    void PrimaryFireStart(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetLookDirection);

    void PrimaryFireStop();

    bool GetPrimaryFireActive();
}
