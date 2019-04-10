using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GunManager : NetworkBehaviour
{
    private Camera playerPOV;

    public GameObject startGun;

    private GameObject currentGunObject;
    private IGun currentGun;

    public GameObject gunContainerPrefab;

    private GameObject swapableGunContainer;
    public int gunSwapHoldTimeMS;
    
    // Use this for initialization
    void Start()
    {
        playerPOV = GetComponentInChildren<Camera>();

        if(isServer)
            CmdSetGunToStarter();
    }

    private void Update()
    {
        if(Input.GetKeyDown("`"))
        {
            CmdDropCurrentWeapon();
        }
    }

    //=================================================================================================
    //Interface Functions
    //=================================================================================================

    public void StartPrimaryFire()
    {
        currentGun.StartPrimaryFire(GetLookDirection);
    }

    public void StopPrimaryFire()
    {
        currentGun.StopPrimaryFire();
    }

    public void StartAltFire()
    {
        currentGun.StartAltFire(GetLookDirection);
    }

    public void StopAltFire()
    {
        currentGun.StopAltFire();
    }

    public void StartReload()
    {
        currentGun.StartReload();
    }

    public void StopReload()
    {
        currentGun.StopReload();
    }

    public string GetWeaponName()
    {
        if (currentGun == null)
            return "NONE";

        return currentGun.GetWeaponName();
    }

    public int GetWeaponMaxAmmo()
    {
        if (currentGun == null)
            return 0;

        return currentGun.GetAmmoPool();
    }

    public int GetWeaponCurrentAmmo()
    {
        if (currentGun == null)
            return 0;

        return currentGun.GetWeaponAmmo();
    }

    public void StartWeaponPickup()
    {

    }

    public void StopWeaponPickup()
    {

    }

    //=================================================================================================
    //Helper Functions
    //=================================================================================================

    private Vector3 GetLookDirection()
    {
        float xrot = Mathf.Sin(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y) * Mathf.Cos(Mathf.Deg2Rad * this.playerPOV.transform.rotation.eulerAngles.x);
        float zrot = Mathf.Cos(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y) * Mathf.Cos(Mathf.Deg2Rad * this.playerPOV.transform.rotation.eulerAngles.x);
        float yrot = -Mathf.Sin(Mathf.Deg2Rad * playerPOV.transform.rotation.eulerAngles.x);

        return new Vector3(xrot, yrot, zrot);
    }

    private Vector3 GetBulletSpawnLocation()
    {
        float initialX = this.transform.position.x;
        float initialY = this.transform.position.y + .7f;
        float initialZ = this.transform.position.z;

        Vector3 rotation = GetLookDirection();

        return new Vector3(initialX + rotation.x, initialY + rotation.y, initialZ + rotation.z);
    }

    //private IEnumerator SwapWeaponCoroutine(int holdTime)
    //{
    //    int currentTime = 0;

    //    while (Input.GetKey("e") && swapableGunContainer != null && currentTime < holdTime)
    //    {
    //        currentTime++;

    //        yield return new WaitForSecondsRealtime(.1f);
    //    }

    //    if (currentTime >= holdTime)
    //    {
    //        CmdSwapGun();
    //    }
    //}

    //Sets the players gun to the gun provided
    private void SetPlayersGun(GameObject newGunObject)
    {
        Debug.Log("SETTING GUN TO: " + newGunObject.name);

        IGun newGun = newGunObject.GetComponent<IGun>();

        newGunObject.transform.parent = this.transform;
        newGunObject.transform.localPosition = newGun.GetGunPositionData();
        newGunObject.transform.localRotation = new Quaternion();

        this.currentGun = newGun;
        this.currentGunObject = newGunObject;
    }

    //=================================================================================================
    //Server Functions
    //=================================================================================================


    [Command]
    private void CmdSetGunToStarter()
    {
        GameObject gun = (GameObject)Instantiate(startGun);
        SetPlayersGun(gun);

        NetworkServer.Spawn(gun);

        RpcUpdatePlayerGun(gun.GetComponent<NetworkIdentity>().netId);
    }

    //[Command]
    //public void CmdSwapGun()
    //{
    //    GameObject newGunContainer = (GameObject)Instantiate(this.gunContainerPrefab, GetBulletSpawnLocation(), new Quaternion());
    //    GunContainer newGunContainerScript = newGunContainer.GetComponent<GunContainer>();

    //    GameObject oldGunContainer = swapableGunContainer;
    //    GunContainer oldGunContainerScript = oldGunContainer.GetComponent<GunContainer>();

    //    GameObject oldGun = currentGun;
    //    GunBase oldGunScript = oldGun.GetComponent<GunBase>();

    //    GameObject newGun = oldGunContainerScript.containedGun;
    //    GunBase newGunScript = newGun.GetComponent<GunBase>();

    //    newGunContainerScript.SetContainedGun(oldGun);
    //    SetPlayersGun(newGun);

    //    Destroy(oldGunContainer);
    //    NetworkServer.Spawn(newGunContainer);
    //    newGunContainer.GetComponent<Rigidbody>().AddForce(GetLookRotation(), ForceMode.Acceleration);

    //    NetworkInstanceId containerID = gunContainerPrefab.GetComponent<NetworkIdentity>().netId;

    //    RpcUpdatePlayerGun(GetComponent<NetworkIdentity>().netId, newGun.GetComponent<NetworkIdentity>().netId, newGunScript.ammoData);
    //    RpcUpdateContainerGun(newGunContainer.GetComponent<NetworkIdentity>().netId, oldGun.GetComponent<NetworkIdentity>().netId, oldGunScript.ammoData);
    //}

    [Command]
    public void CmdDropCurrentWeapon()
    {
        GameObject newGunContainer = (GameObject)Instantiate(this.gunContainerPrefab, GetBulletSpawnLocation(), new Quaternion());
        GunContainer newGunContainerScript = newGunContainer.GetComponent<GunContainer>();

        GameObject oldGun = currentGunObject;
        IGun oldGunScript = oldGun.GetComponent<IGun>();

        newGunContainerScript.SetContainedGun(oldGun);

        NetworkServer.Spawn(newGunContainer);

        currentGunObject = null;
        currentGun = null;
        
        //RpcUpdateContainerGun(newGunContainer.GetComponent<NetworkIdentity>().netId, oldGun.GetComponent<NetworkIdentity>().netId, oldGunScript.ammoData);
    }

    //=================================================================================================
    //Client Functions
    //=================================================================================================


    [ClientRpc]
    public void RpcUpdatePlayerGun(NetworkInstanceId gunID)
    {
        if (isServer)
            return;

        SetPlayersGun(ClientScene.FindLocalObject(gunID));
    }

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
    public void SetSwappableGunContainer(GameObject gunContainer)
    {
        this.swapableGunContainer = gunContainer;
        if (gunContainer == null || gunContainer.GetComponent<GunContainer>().containedGun == null)
        {
            //this.hudManager.getTextElementByName("Player Interact").text = "";
        }
        else
        {
            //this.hudManager.getTextElementByName("Player Interact").text = "Pickup " + this.swapableGunContainer.GetComponent<GunContainer>().containedGun.GetComponent<GunBase>().gunName;
        }
    }

    public void SpawnProjectile(string projectilePrefabName, ProjectileBase.ProjectileProperties properties)
    {
        //serverCommands.CmdSpawnServerProjectile(projectilePrefabName, properties);
    }

    
}
