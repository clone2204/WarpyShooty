using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrimaryFireBehavior
{
    void Init(IAltFireBehavior altFireBehavior, IAmmoBehavior ammoBehavior);

    void PrimaryFireStart(GamePlayer player, System.Func<Vector3> GetSpawnLocation);

    void PrimaryFireStop();

    bool GetPrimaryFireActive();
}
