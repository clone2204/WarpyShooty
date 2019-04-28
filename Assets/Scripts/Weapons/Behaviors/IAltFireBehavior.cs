using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAltFireBehavior 
{
    void Init(IPrimaryFireBehavior primaryFireBehavior, IAmmoBehavior ammoBehavior);

<<<<<<< HEAD
    void StartAltFire(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetSpawnDirection);
=======
    void StartAltFire(GamePlayer player, System.Func<Vector3> GetSpawnLocation);
>>>>>>> c472e59ab410daa365ce01237f6b64f3b06c7e48

    void StopAltFire();

    bool GetAltFireActive();
}
