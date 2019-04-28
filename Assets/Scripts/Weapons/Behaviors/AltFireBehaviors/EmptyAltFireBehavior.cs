using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EmptyAltFireBehavior : NetworkBehaviour, IAltFireBehavior
{
    public void Init(IPrimaryFireBehavior primaryFireBehavior, IAmmoBehavior reloadBehavior)
    {
        return;
    }

<<<<<<< HEAD
    public void StartAltFire(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetSpawnDirection)
=======
    public void StartAltFire(RealPlayer player, System.Func<Vector3> GetSpawnLocation, Func<Vector3> GetLookDirection)
>>>>>>> c472e59ab410daa365ce01237f6b64f3b06c7e48
    {
        return;
    }

    public void StopAltFire()
    {
        return;
    }

    public bool GetAltFireActive()
    {
        return false;
    }
}
