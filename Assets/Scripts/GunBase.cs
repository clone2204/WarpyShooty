using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class GunBase : NetworkBehaviour
{
    protected GunManager gunManager;
    public string gunName;

    public Vector3 gunViewLocation;
    public int damage;
    
    public float refireTime;

    public int startAmmoPoolCount;
    public int magazineSize;
    public float reloadTime;

    protected bool isReloadCooldown;
    protected bool isRefireCooldown;

    public AmmoData ammoData;

    public struct AmmoData
    {
        public int currentAmmoPoolCount;
        public int currentAmmoCount;

        public bool hasBeenInitialized;

        public AmmoData(int currentAmmoCount, int ammoPoolCount)
        {
            this.currentAmmoPoolCount = ammoPoolCount;
            this.currentAmmoCount = currentAmmoCount;

            this.hasBeenInitialized = true;
        }

    }
    
    public void Start()
    {
        if(!ammoData.hasBeenInitialized)
        {
            this.ammoData = new AmmoData(this.magazineSize, this.startAmmoPoolCount);
        }
        
    }
    
    public void SetGunManager(GunManager manager)
    {
        this.gunManager = manager;
    }

    public void InitFire()
    {
        if (!isAbleToFire())
            return;

        StartCoroutine(RefireCooldownCoroutine(refireTime));
    }
    
    public void InitAltFire()
    {

    }

    //What the gun is to do when it fires or altfires
    public abstract void FireAction();
    public abstract void AltFireAction();
    
    public void ReloadGun()
    {
        if (this.gunManager == null)
            return;

        if (ammoData.currentAmmoCount == magazineSize)
            return;

        StartCoroutine(ReloadCooldownCoroutine(reloadTime));
    }

    protected bool isAbleToFire()
    {
        if (this.gunManager == null)
            return false;

        //Checks for ammo before Firing
        if (this.ammoData.currentAmmoCount <= 0 && !isReloadCooldown)
        {
            if(ammoData.currentAmmoPoolCount > 0)
            {
                ReloadGun();
            }
            return false;
        }

        return (!this.isRefireCooldown && !this.isReloadCooldown);
    }

    protected IEnumerator RefireCooldownCoroutine(float refireCooldown)
    {
        while(Input.GetKey("mouse 0") && isAbleToFire())
        {
            isRefireCooldown = true;

            FireAction();

            ammoData.currentAmmoCount--;
            gunManager.ModifyCurrentAmmoCount(this.ammoData.currentAmmoCount, this.ammoData.currentAmmoPoolCount);

            yield return new WaitForSecondsRealtime(refireCooldown);

            isRefireCooldown = false;
        }
    }

    protected IEnumerator ReloadCooldownCoroutine(float reloadCooldown)
    {
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
        this.gunManager.ModifyCurrentAmmoCount(this.ammoData.currentAmmoCount, this.ammoData.currentAmmoPoolCount);

        isReloadCooldown = false;
    }

}
