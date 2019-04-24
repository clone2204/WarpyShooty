using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StandardAmmoBehavior : NetworkBehaviour, IAmmoBehavior
{
    [SerializeField] private int maxAmmoPool;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmoPool;
    [SerializeField] private int currentAmmo;

    [SerializeField] private float reloadTime;

    private bool reloadActive;
    private bool reloadOnCountdown;

    private IPrimaryFireBehavior primaryFireBehavior;
    private IAltFireBehavior altFireBehavior;

    public void Init(IPrimaryFireBehavior primaryFireBehavior, IAltFireBehavior altFireBehavior)
    {
        this.primaryFireBehavior = primaryFireBehavior;
        this.altFireBehavior = altFireBehavior;

        reloadActive = false;
        reloadOnCountdown = false;

        StartCoroutine(ReloadCooldownCoroutine());
    }

    public void StartReload()
    {
        Debug.LogWarning("reload");
        reloadActive = true;
    }

    public void StopReload()
    {
        reloadActive = false;
    }

    public bool GetReloadActive()
    {
        return reloadOnCountdown;
    }

    public bool ConsumeAmmo(int amount)
    {
        if (currentAmmo <= 0)
            return false;

        currentAmmo -= amount;
        return true;
    }

    public int GetCurrentAmmoPool()
    {
        return currentAmmoPool;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    private bool TransferAmmoFromPool(int amount)
    {
        if (amount == -1)
        {
            amount = maxAmmo;
        }

        if (amount > currentAmmoPool) //Need to fix reload bug
        {
            amount = currentAmmoPool;
        }
        else if ((amount + currentAmmo) > maxAmmo)
        {
            amount = maxAmmo - currentAmmo;
        }

        Debug.LogWarning("TRANSFER");
        currentAmmoPool -= amount;
        currentAmmo += amount;
        return true;
    }

    private IEnumerator ReloadCooldownCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => reloadActive);

            if (!reloadOnCountdown)
            {
                reloadOnCountdown = true;

                yield return new WaitForSecondsRealtime(reloadTime);

                Debug.LogWarning("...RELOADED!");
                TransferAmmoFromPool(-1);

                reloadOnCountdown = false;

            }
        }
    }
}
