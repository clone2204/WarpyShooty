using UnityEngine;
using System.Collections;
using System;

public class ProjectileGunBase : Gun
{
    public GameObject projectilePrefab;
    public float velocity;

    private bool primaryFireActive;
    private bool altFireActive;
    private bool reloadActive;

    new void Start()
    {
        base.Start();

        StartCoroutine(RefireCooldownCoroutine(base.refireTime));
        StartCoroutine(ReloadCooldownCoroutine(base.reloadTime));
    }

    public override void StartPrimaryFire(Func<Vector3> GetLookDirection)
    {
        primaryFireActive = true;
    }

    public override void StopPrimaryFire()
    {
        primaryFireActive = false;
    }

    public override void StartAltFire(Func<Vector3> GetLookDirection)
    {
        altFireActive = true;
    }

    public override void StopAltFire()
    {
        altFireActive = false;
    }

    public override void StartReload()
    {
        reloadActive = true;
    }

    public override void StopReload()
    {
        //reloadActive = false;
    }
    protected bool isAbleToFire()
    {

        //Checks for ammo before Firing
        if (ammoData.currentAmmoCount <= 0 && !isReloadCooldown)
        {
            if (ammoData.currentAmmoPoolCount > 0)
            {
                reloadActive = true;
            }
            return false;
        }

        return (!this.isRefireCooldown && !this.isReloadCooldown);
    }

    protected IEnumerator RefireCooldownCoroutine(float refireCooldown)
    {
        while (true)
        {
            yield return new WaitUntil(() => primaryFireActive);

            while (primaryFireActive && isAbleToFire())
            {
                isRefireCooldown = true;

                FireAction();

                ammoData.currentAmmoCount--;

                yield return new WaitForSecondsRealtime(refireCooldown);

                isRefireCooldown = false;
            }
        }
    }

    protected IEnumerator ReloadCooldownCoroutine(float reloadCooldown)
    {
        while (true)
        {
            yield return new WaitUntil(() => reloadActive);

            isReloadCooldown = true;

            yield return new WaitForSecondsRealtime(reloadCooldown);

            int difference = this.magazineSize - this.ammoData.currentAmmoCount;

            if (this.ammoData.currentAmmoPoolCount - difference >= 0)
            {
                this.ammoData.currentAmmoPoolCount -= difference;
                this.ammoData.currentAmmoCount = this.magazineSize;
            }
            else
            {
                this.ammoData.currentAmmoCount += this.ammoData.currentAmmoPoolCount;
                this.ammoData.currentAmmoPoolCount = 0;
            }

            isReloadCooldown = false;

        }
    }

    private void FireAction()
    {
        Debug.LogWarning("FIRE!!!");
    }

}
