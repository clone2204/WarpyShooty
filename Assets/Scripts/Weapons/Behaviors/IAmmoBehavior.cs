using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAmmoBehavior
{
    void Init(IPrimaryFireBehavior primaryFireBehavior, IAltFireBehavior altFireBehavior);

    void StartReload();

    void StopReload();

    bool ConsumeAmmo(int amount);

    int GetCurrentAmmoPool();

    int GetCurrentAmmo();
  

    bool GetReloadActive();
}
