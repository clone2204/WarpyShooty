using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour
{
    private Transform weaponPort;

    [SerializeField] private GameObject startingWeapon;
    private IWeapon currentWeapon;

    public GameObject gunContainerPrefab;

    private GunContainer swapableGunContainer;
    private bool weaponSwapActive;
    public int gunSwapHoldTimeMS;
    
    // Use this for initialization
    void Start()
    {
        transform.fin
        playerHud = GameObject.Find("_SCRIPTS_").GetComponentInChildren<PlayerHUDManager>();

        if (playerPOV != null)
        {
            weaponPort = playerPOV.transform.Find("WeaponPort").transform;
        }
        else
        {
            weaponPort = transform.Find("WeaponPort").transform;
        }

        Debug.LogWarning(weaponPort);

        if (isServer)
        {
            StartCoroutine(SwapWeaponCoroutine(gunSwapHoldTimeMS));
            SpawnStartingWeapon();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown("`"))
        {
            //CmdDropCurrentWeapon();
            DropCurrentWeapon();
        }
    }

    //=================================================================================================
    //Interface Functions
    //=================================================================================================

    public void StartPrimaryFire(GamePlayer player)
    {
        currentWeapon.StartPrimaryFire(player, GetBulletSpawnLocation);
    }

    public void StopPrimaryFire()
    {
        currentWeapon.StopPrimaryFire();
    }

    public void StartAltFire(GamePlayer player)
    {
        currentWeapon.StartAltFire(player, GetBulletSpawnLocation);
    }

    public void StopAltFire()
    {
        currentWeapon.StopAltFire();
    }

    public void StartReload()
    {
        currentWeapon.StartReload();
    }

    public void StopReload()
    {
        currentWeapon.StopReload();
    }

    public string GetWeaponName()
    {
        if (currentWeapon == null)
            return "NONE";

        return currentWeapon.GetWeaponName();
    }

    public int GetWeaponAmmoPool()
    {
        if (currentWeapon == null)
            return 0;

        return currentWeapon.GetCurrentAmmoPool();
    }

    public int GetWeaponCurrentAmmo()
    {
        if (currentWeapon == null)
            return 0;

        return currentWeapon.GetCurrentAmmo();
    }

    public void StartWeaponPickup()
    {
        this.weaponSwapActive = true;
    }

    public void StopWeaponPickup()
    {
        this.weaponSwapActive = false;
    }

    //=================================================================================================
    //Helper Functions
    //=================================================================================================

    
    private Vector3 GetBulletSpawnLocation()
    {
        float initialX = this.transform.position.x;
        float initialY = this.transform.position.y + .7f;
        float initialZ = this.transform.position.z;

        Vector3 rotation = GetLookDirection();

        return new Vector3(initialX + rotation.x, initialY + rotation.y, initialZ + rotation.z);
    }

    private IEnumerator SwapWeaponCoroutine(int holdTime)
    {
        while (true)
        {
            yield return new WaitUntil(() => weaponSwapActive);

            int currentTime = 0;
            while (weaponSwapActive && swapableGunContainer != null && currentTime < holdTime)
            {
                currentTime++;

                yield return new WaitForSecondsRealtime(.1f);
            }

            if (currentTime >= holdTime)
            {
                SwapGun();
            }
        }
    }

    //Sets the players gun to the gun provided
    private void SetPlayersGun(IWeapon newWeapon)
    {
        Debug.Log("SETTING GUN TO: " + newWeapon.GetWeaponName());
        
        this.currentWeapon = newWeapon;
        this.currentWeapon.SetWeaponPosition(weaponPort);
    }

    //=================================================================================================
    //Server Functions
    //=================================================================================================

    public void SpawnStartingWeapon()
    {
        if (startingWeapon.GetComponent<IWeapon>() == null)
            return;

        IWeapon weapon = ((GameObject)Instantiate(startingWeapon)).GetComponent<IWeapon>();
        SetWeapon(weapon);
    }

    public void SetWeapon(IWeapon weapon)
    {
        this.currentWeapon = weapon;
        currentWeapon.SetWeaponPosition(weaponPort);
    }

    public void DropCurrentWeapon()
    {
        GunContainer dropGunContainer = ((GameObject)Instantiate(gunContainerPrefab, GetBulletSpawnLocation(), new Quaternion())).GetComponent<GunContainer>();
        dropGunContainer.SetContainedGun(currentWeapon);

        currentWeapon = null;
    }

    
    public void SwapGun()
    {
        DropCurrentWeapon();

        GunContainer pickupGunContainer = swapableGunContainer;
        SetWeapon(pickupGunContainer.GetContainedGun());

        pickupGunContainer.DestroyContainer();

        //RpcUpdatePlayerGun(GetComponent<NetworkIdentity>().netId, newGun.GetComponent<NetworkIdentity>().netId, newGunScript.ammoData);
        //RpcUpdateContainerGun(newGunContainer.GetComponent<NetworkIdentity>().netId, oldGun.GetComponent<NetworkIdentity>().netId, oldGunScript.ammoData);
    }

    //[Command]
    //public void CmdDropCurrentWeapon()
    //{
    //    GameObject newGunContainer = (GameObject)Instantiate(this.gunContainerPrefab, GetBulletSpawnLocation(), new Quaternion());
    //    GunContainer newGunContainerScript = newGunContainer.GetComponent<GunContainer>();

    //    GameObject oldGun = currentGunObject;
    //    IGun oldGunScript = oldGun.GetComponent<IGun>();

    //    //newGunContainerScript.SetContainedGun(oldGun);

    //    NetworkServer.Spawn(newGunContainer);

    //    currentGunObject = null;
    //    currentGun = null;
        
    //    //RpcUpdateContainerGun(newGunContainer.GetComponent<NetworkIdentity>().netId, oldGun.GetComponent<NetworkIdentity>().netId, oldGunScript.ammoData);
    //}

    //=================================================================================================
    //Client Functions
    //=================================================================================================


    //[ClientRpc]
    //public void RpcUpdatePlayerGun(NetworkInstanceId gunID)
    //{
    //    if (isServer)
    //        return;

    //    SetPlayersGun(ClientScene.FindLocalObject(gunID));
    //}

    //[ClientRpc]
    //public void RpcUpdateContainerGun(NetworkInstanceId containerID, NetworkInstanceId gunID, GunBase.AmmoData ammoData)
    //{
    //    if (isServer)
    //        return;

    //    GameObject gunContainerPrefab = ClientScene.FindLocalObject(containerID);
    //    GunContainer gunContainerScript = gunContainerPrefab.GetComponent<GunContainer>();

    //    gunContainerScript.SetContainedGun(ClientScene.FindLocalObject(gunID));

    //    GunBase containerGunScript = gunContainerScript.containedGun.GetComponent<GunBase>();
    //    containerGunScript.ammoData = ammoData;

    //}

    //Sets the gun container that the player is currently able to pick up
    public void SetSwappableGunContainer(GunContainer gunContainer)
    {
        this.swapableGunContainer = gunContainer;
        if (gunContainer == null || gunContainer.GetContainedGun() == null)
        {
            //Debug.LogWarning("Container: " + gunContainer);
            //Debug.LogWarning("Gun: " + gunContainer.GetContainedGun().GetWeaponName());

            playerHud.ClearWeaponPickupName();
        }
        else
        {
            playerHud.SetWeaponPickupName(gunContainer.GetContainedGun().GetWeaponName());
        }
    }
}
