using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EmptyReloadBehavior : NetworkBehaviour, IReloadBehavior
{
    public void Init(System.Func<int, bool> TransferAmmo, IPrimaryFireBehavior primaryFireBehavior, IAltFireBehavior reloadBehavior)
    {
        return;
    }

    public void StartReload()
    {
        return;
    }

    public void StopReload()
    {
        return;
    }

    public bool GetReloadActive()
    {
        return false;
    }
}
