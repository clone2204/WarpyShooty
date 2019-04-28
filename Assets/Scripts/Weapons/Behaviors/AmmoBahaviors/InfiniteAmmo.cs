using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InfiniteAmmo : NetworkBehaviour, IAmmoBehavior
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(IPrimaryFireBehavior primaryFireBehavior, IAltFireBehavior altFireBehavior)
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

    public bool ConsumeAmmo(int amount)
    {
        return true;
    }

    public int GetCurrentAmmo()
    {
        return -1;
    }

    public int GetCurrentAmmoPool()
    {
        return -1;
    }

    public bool GetReloadActive()
    {
        return false;
    }

    
}
