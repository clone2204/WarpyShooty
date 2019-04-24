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

    public void StartAltFire(RealPlayer player, System.Func<Vector3> GetSpawnLocation, Func<Vector3> GetLookDirection)
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
