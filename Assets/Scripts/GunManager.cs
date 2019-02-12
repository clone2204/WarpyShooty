using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GunManager : NetworkBehaviour
{
    public GameObject startGun;
    private GameObject currentGun { get; set; }
    private GunBase gunScript;

    public GameObject gunContainerPrefab;

    Camera playerPOV;
    Canvas playerHud;
    PlayerHUDManager hudManager;

    PlayerServerCommands serverCommands;

    private GameObject swapableGunContainer;
    public int gunSwapHoldTimeMS;
    
    // Use this for initialization
    void Start()
    {
        playerPOV = GetComponentInChildren<Camera>();
        playerHud = GetComponentInChildren<Canvas>();
        hudManager = playerHud.GetComponent<PlayerHUDManager>();

        serverCommands = GetComponent<PlayerServerCommands>();

        if(isServer)
            CmdSetGunToStarter();
    }

    //Player Controlls
    //==============================================================================================================================

    //Tells the current held gun to fire
    public void FireGun()
    {
        if (isLocalPlayer)
            gunScript.InitFire();
    }

    public void AltFireGun()
    {
        if (isLocalPlayer)
            gunScript.InitAltFire();
    }
    //Tells the current held gun to reload
    public void ReloadGun()
    {
        if(isLocalPlayer)
            gunScript.ReloadGun();
    }

    //Spawns a GunContainer on the server and destroys the players current held gun
    public void SwapGun()
    {
        StartCoroutine(SwapWeaponCoroutine(gunSwapHoldTimeMS));
    }

    //===============================================================================================================================

    [Command]
    public void CmdSetGunToStarter()
    {
        GameObject gun = (GameObject)Instantiate(startGun);
        SetPlayersGun(gun);

        NetworkServer.Spawn(gun);

        RpcUpdatePlayerGun(GetComponent<NetworkIdentity>().netId, gun.GetComponent<NetworkIdentity>().netId, gun.GetComponent<GunBase>().ammoData);
    }

    [Command]
    public void CmdSwapGun()
    {
        GameObject newGunContainer = (GameObject)Instantiate(this.gunContainerPrefab, GetBulletSpawnLocation(), new Quaternion());
        GunContainer newGunContainerScript = newGunContainer.GetComponent<GunContainer>();

        GameObject oldGunContainer = swapableGunContainer;
        GunContainer oldGunContainerScript = oldGunContainer.GetComponent<GunContainer>();

        GameObject oldGun = currentGun;
        GunBase oldGunScript = oldGun.GetComponent<GunBase>();

        GameObject newGun = oldGunContainerScript.containedGun;
        GunBase newGunScript = newGun.GetComponent<GunBase>();

        newGunContainerScript.SetContainedGun(oldGun);
        SetPlayersGun(newGun);

        Destroy(oldGunContainer);
        NetworkServer.Spawn(newGunContainer);
        newGunContainer.GetComponent<Rigidbody>().AddForce(GetLookRotation(), ForceMode.Acceleration);

        NetworkInstanceId containerID = gunContainerPrefab.GetComponent<NetworkIdentity>().netId;

        RpcUpdatePlayerGun(GetComponent<NetworkIdentity>().netId, newGun.GetComponent<NetworkIdentity>().netId, newGunScript.ammoData);
        RpcUpdateContainerGun(newGunContainer.GetComponent<NetworkIdentity>().netId, oldGun.GetComponent<NetworkIdentity>().netId, oldGunScript.ammoData);
    }

    [Command]
    public void CmdDropCurrentWeapon()
    {
        GameObject newGunContainer = (GameObject)Instantiate(this.gunContainerPrefab, GetBulletSpawnLocation(), new Quaternion());
        GunContainer newGunContainerScript = newGunContainer.GetComponent<GunContainer>();

        GameObject oldGun = currentGun;
        GunBase oldGunScript = oldGun.GetComponent<GunBase>();

        newGunContainerScript.SetContainedGun(oldGun);

        NetworkServer.Spawn(newGunContainer);

        RpcUpdateContainerGun(newGunContainer.GetComponent<NetworkIdentity>().netId, oldGun.GetComponent<NetworkIdentity>().netId, oldGunScript.ammoData);
    }

    [ClientRpc]
    public void RpcUpdatePlayerGun(NetworkInstanceId playerID, NetworkInstanceId gunID, GunBase.AmmoData ammoData)
    {
        if (isServer)
            return;

        GameObject player = ClientScene.FindLocalObject(playerID);
        GunManager playerGunManager = player.GetComponent<GunManager>();

        playerGunManager.SetPlayersGun(ClientScene.FindLocalObject(gunID));

        GunBase playerGunScript = playerGunManager.currentGun.GetComponent<GunBase>();
        playerGunScript.ammoData = ammoData;
    }

    [ClientRpc]
    public void RpcUpdateContainerGun(NetworkInstanceId containerID, NetworkInstanceId gunID, GunBase.AmmoData ammoData)
    {
        if (isServer)
            return;

        GameObject gunContainerPrefab = ClientScene.FindLocalObject(containerID);
        GunContainer gunContainerScript = gunContainerPrefab.GetComponent<GunContainer>();

        gunContainerScript.SetContainedGun(ClientScene.FindLocalObject(gunID));

        GunBase containerGunScript = gunContainerScript.containedGun.GetComponent<GunBase>();
        containerGunScript.ammoData = ammoData;

    }

    

    //Sets the players gun to the gun provided
    public void SetPlayersGun(GameObject gun)
    {
        Debug.Log("SETTING GUN TO: " + gun);

        GunBase newGunScript = gun.GetComponent<GunBase>();

        gun.transform.parent = this.transform;
        gun.transform.localPosition = newGunScript.gunViewLocation;
        gun.transform.localRotation = new Quaternion();
        
        newGunScript.SetGunManager(this);

        if(!newGunScript.ammoData.hasBeenInitialized)
        {
            newGunScript.ammoData = new GunBase.AmmoData(newGunScript.magazineSize, newGunScript.startAmmoPoolCount);
        }

        
        if(this.isLocalPlayer)
        {
            if(hudManager == null)
            {
                playerHud = this.GetComponentInChildren<Canvas>();
                hudManager = playerHud.GetComponent<PlayerHUDManager>();
            }
            hudManager.UpdateNewGunInfo(gun);
        }
            

        this.currentGun = gun;
        this.gunScript = newGunScript;
    }

    public Vector3 GetLookRotation()
    {
        float xrot = Mathf.Sin(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y) * Mathf.Cos(Mathf.Deg2Rad * this.playerPOV.transform.rotation.eulerAngles.x);
        float zrot = Mathf.Cos(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y) * Mathf.Cos(Mathf.Deg2Rad * this.playerPOV.transform.rotation.eulerAngles.x);
        float yrot = -Mathf.Sin(Mathf.Deg2Rad * playerPOV.transform.rotation.eulerAngles.x);

        return new Vector3(xrot, yrot, zrot);
    }

    public Vector3 GetBulletSpawnLocation()
    {
        float initialX = this.transform.position.x;
        float initialY = this.transform.position.y + .7f;
        float initialZ = this.transform.position.z;

        Vector3 rotation = GetLookRotation();

        return new Vector3(initialX + rotation.x, initialY + rotation.y, initialZ + rotation.z);
    }
    
    public void ModifyCurrentAmmoCount(int currentAmmo, int maxAmmo)
    {
        this.hudManager.getTextElementByName("Ammo Count").text = currentAmmo + " / " + maxAmmo;
    }


    //Sets the gun container that the player is currently able to pick up
    public void SetSwappableGunContainer(GameObject gunContainer)
    {
        this.swapableGunContainer = gunContainer;
        if (gunContainer == null || gunContainer.GetComponent<GunContainer>().containedGun == null)
        {
            this.hudManager.getTextElementByName("Player Interact").text = "";
        }
        else
        {
            this.hudManager.getTextElementByName("Player Interact").text = "Pickup " + this.swapableGunContainer.GetComponent<GunContainer>().containedGun.GetComponent<GunBase>().gunName;
        }
    }

    public void SpawnProjectile(string projectilePrefabName, ProjectileBase.ProjectileProperties properties)
    {
        serverCommands.CmdSpawnServerProjectile(projectilePrefabName, properties);
    }

    public bool SprayPlayerSpray()
    {
        RaycastHit hit;
        Ray sprayRay = new Ray(GetBulletSpawnLocation(), GetLookRotation());
        bool sprayHit = Physics.Raycast(sprayRay, out hit, 4);

        if (sprayHit)
        {
            Vector3 sprayPosition = hit.point;
            float faceRot = 0;
            float zRot = 0;
            float upRot = hit.normal.y * 90;

            if (hit.normal.x > 0)
            {
                faceRot = -90;
                sprayPosition += new Vector3(.0005f, 0, 0);
            }
            else if (hit.normal.x < 0)
            {
                faceRot = 90;
                sprayPosition += new Vector3(-0.0005f, 0, 0);
            }

            if (hit.normal.z > 0)
            {
                faceRot = 180;
                sprayPosition += new Vector3(0, 0, .0005f);
            }
            else if (hit.normal.z < 0)
            {
                faceRot = 0;
                sprayPosition += new Vector3(0, 0, -.0005f);
            }

            if (hit.normal.y > 0)
            {
                upRot = 90;
                zRot = transform.rotation.eulerAngles.y;
                sprayPosition += new Vector3(0, .0005f, 0);

            }
            else if (hit.normal.y < 0)
            {
                zRot = transform.rotation.eulerAngles.y;
                sprayPosition += new Vector3(0, -.0005f, 0);
            }

            Debug.LogWarning(zRot);
            Quaternion rotation = Quaternion.Euler(upRot, faceRot, -zRot);

            serverCommands.CmdSpawnPlayerSprayOnServer("PlayerSpray", GetComponentInChildren<SettingsManager>().spraySource, sprayPosition, rotation);
            Debug.LogWarning(GetComponentInChildren<SettingsManager>().spraySource);
        }

        return sprayHit;
    }

    private IEnumerator SwapWeaponCoroutine(int holdTime)
    {
        int currentTime = 0;

        while(Input.GetKey("e") && swapableGunContainer != null && currentTime < holdTime)
        {
            currentTime++;

            yield return new WaitForSecondsRealtime(.1f);
        }

        if (currentTime >= holdTime)
        {
            CmdSwapGun();
        }

    }

}
