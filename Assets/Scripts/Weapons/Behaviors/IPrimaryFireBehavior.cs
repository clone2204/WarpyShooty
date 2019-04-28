using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrimaryFireBehavior
{
    void Init(IAltFireBehavior altFireBehavior, IAmmoBehavior ammoBehavior);

<<<<<<< HEAD
    void PrimaryFireStart(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetSpawnDirection);
=======
    void PrimaryFireStart(GamePlayer player, System.Func<Vector3> GetSpawnLocation);
>>>>>>> c472e59ab410daa365ce01237f6b64f3b06c7e48

    void PrimaryFireStop();

    bool GetPrimaryFireActive();
}
