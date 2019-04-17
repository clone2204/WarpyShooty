using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StandardReloadBehavior : NetworkBehaviour, IReloadBehavior
{
    [SerializeField] private float reloadTime;

    private bool reloadActive;
    private bool reloadOnCountdown;

    private System.Func<int, bool> TransferAmmo;
    private IPrimaryFireBehavior primaryFireBehavior;
    private IAltFireBehavior altFireBehavior;

    public void Init(System.Func<int, bool> TransferAmmo, IPrimaryFireBehavior primaryFireBehavior, IAltFireBehavior altFireBehavior)
    {
        this.TransferAmmo = TransferAmmo;
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
                TransferAmmo(-1);

                reloadOnCountdown = false;

            }
        }
    }
}
