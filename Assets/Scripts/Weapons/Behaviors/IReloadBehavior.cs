using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReloadBehavior
{
    void Init(System.Func<int, bool> TransferAmmo, IPrimaryFireBehavior primaryFireBehavior, IAltFireBehavior altFireBehavior);

    void StartReload();

    void StopReload();

    bool GetReloadActive();
}
