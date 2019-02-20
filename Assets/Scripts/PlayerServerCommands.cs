using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PlayerServerCommands : NetworkBehaviour
{
    private LobbyManager lobbyManager;
    
    private LevelServerCommands levelCommands;

    private List<GameObject> prefabList;

    public void Start()
    {
        Debug.Log("PlayerServerCommands INIT");

        GameObject scriptObject = GameObject.Find("_SCRIPTS_");
        lobbyManager = scriptObject.GetComponent<LobbyManager>();
        levelCommands = scriptObject.GetComponentInChildren<LevelServerCommands>();

        //this.prefabList = this.lobbyManager.spawnPrefabs;
        
    }

   
    //---------------------------------------------------------------------------------------------------
    //Takes in the name of an object and returns the original prefab of that object
    private GameObject GetPrefabByName(string name)
    {
        if (this.prefabList == null)
            Start();


        foreach (GameObject gameObject in this.prefabList)
        {
            if(gameObject.name == name)
            {
                return gameObject;
            }
        }

        return null;
    }

    
    

    [ClientRpc]
    public void RpcDisplayReadyPlayer(NetworkInstanceId id, bool ready)
    {
        GameObject lobbyPlayer = ClientScene.FindLocalObject(id);
        lobbyPlayer.GetComponent<NetworkLobbyPlayer>().readyToBegin = ready;
        //lobbyPlayer.GetComponent<LobbyPlayerManager>().DisplayPlayerReadyText();
    }

    [TargetRpc]
    public void TargetReceivePlayerDataFromServer(NetworkConnection connection, NetworkInstanceId playerID, string playerName)
    {
        GameObject player = ClientScene.FindLocalObject(playerID);

        PlayerInfoManager playerInfoManager = player.GetComponent<PlayerInfoManager>();
        //playerInfoManager.playerInfo = new PlayerInfoManager.PlayerInfo(playerName, playerTeam); ; //<== WARNING: The server may be sending an empty PlayerInfo to the TargetRPC, investigate


    }

    //------------------------------------------------------------------------------------------------------------------------------------------------
    //Takes in the sprayPrefabs name, the URL source of the spray, the position and look direction of the spray
    [Command]
    public void CmdSpawnPlayerSprayOnServer(string prefabName, string spraySource, Vector3 position, Quaternion rotation)
    {
        GameObject playerSpray = (GameObject)Instantiate(GetPrefabByName(prefabName));
        SprayManager sprayScript = playerSpray.GetComponent<SprayManager>();

        sprayScript.spraySource = new WWW(spraySource);
        sprayScript.SetSprayPosition(position);
        sprayScript.SetSprayRotation(rotation);

        Debug.LogWarning("PING");
        NetworkServer.Spawn(playerSpray);

        RpcSpawnPlayerSprayOnClient(playerSpray.GetComponent<NetworkIdentity>().netId, spraySource);
    }

    [ClientRpc]
    public void RpcSpawnPlayerSprayOnClient(NetworkInstanceId sprayID, string spraySource)
    {
        if (isServer)
            return;

        GameObject playerSpray = ClientScene.FindLocalObject(sprayID);
        SprayManager sprayScript = playerSpray.GetComponent<SprayManager>();

        sprayScript.spraySource = new WWW(spraySource);
    }

    //----------------------------------------------------------------------------------
    //Called by a client on connection to the server
    //Server cylces through all weapons currently in the scene, then sends their parent to the client to be paired by the client
    [Command]
    public void CmdSyncWeaponParents()
    {
        foreach(GameObject weapon in GameObject.FindGameObjectsWithTag("Weapon"))
        {
            TargetSyncWeaponParents(connectionToClient, weapon.transform.parent.GetComponent<NetworkIdentity>().netId, weapon.GetComponent<NetworkIdentity>().netId);
        }
    }

    [TargetRpc]
    public void TargetSyncWeaponParents(NetworkConnection target, NetworkInstanceId parentID, NetworkInstanceId weaponID)
    {
        GameObject parentObject = ClientScene.FindLocalObject(parentID);
        GameObject weapon = ClientScene.FindLocalObject(weaponID);

        if(parentObject.GetComponent<GunManager>() != null)
        {
            GunManager playerParent = parentObject.GetComponent<GunManager>();
            playerParent.SetPlayersGun(weapon);
        }
        if (parentObject.GetComponent<GunContainer>() != null)
        {
            GunContainer playerParent = parentObject.GetComponent<GunContainer>();
            playerParent.SetContainedGun(weapon);
        }
    }


    //-----------------------------------------------------------------------------------------------------
    //Takes in the players netID and name of their current gun when called on the players initialization
    //Sets the players gun on the server, then sends out the players netID and guns netID to be set on each client

    [Command]
    public void CmdInitializePlayerGunOnServer(NetworkInstanceId playerID, string gunName)
    {
        GameObject player = NetworkServer.FindLocalObject(playerID);
        GunManager playerGunManager = player.GetComponent<GunManager>();

        GameObject gun = (GameObject)Instantiate(GetPrefabByName(gunName));
        playerGunManager.SetPlayersGun(gun);
        
        NetworkServer.Spawn(gun);
        

        RpcInitializePlayerGunOnClient(playerID, gun.GetComponent<NetworkIdentity>().netId);
    }

    [ClientRpc]
    public void RpcInitializePlayerGunOnClient(NetworkInstanceId playerID, NetworkInstanceId gunID)
    {
        if (isServer)
            return;

        GameObject player = ClientScene.FindLocalObject(playerID);
        GunManager playerGunManager = player.GetComponent<GunManager>();

        GameObject gun = ClientScene.FindLocalObject(gunID);
        playerGunManager.SetPlayersGun(gun);
    }

    //---------------------------------------------------------------------------------------------------------------
    //Server and client command sets for spawning projectiles
    //Projectile prefab name and properties are provided, projectile is constructed on the server, and properties are sent out and set on each client

    [Command]
    public void CmdSpawnServerProjectile(string projectileName, ProjectileBase.ProjectileProperties properties)
    {
        GameObject serverBullet = (GameObject)Instantiate(GetPrefabByName(projectileName), properties.spawnLocation, Quaternion.Euler(properties.spawnLocation));

        ProjectileBase bulletScript = serverBullet.GetComponent<ProjectileBase>();
        bulletScript.SetProjectileProperties(properties);

        NetworkServer.Spawn(serverBullet);

        RpcSetClientProjectileProperties(serverBullet.GetComponent<NetworkIdentity>().netId, properties);
    }

    [ClientRpc]
    public void RpcSetClientProjectileProperties(NetworkInstanceId id, ProjectileBase.ProjectileProperties projectileProperties)
    {
        if (isServer)
            return;

        GameObject spawnPrefab = (GameObject)ClientScene.FindLocalObject(id);

        ProjectileBase bulletScript = spawnPrefab.GetComponent<ProjectileBase>();
        bulletScript.SetProjectileProperties(projectileProperties);
    }

    //---------------------------------------------------------------------------------------------------------------
    //Server and client command sets for spawning a new Gun Container
    //Location to spawn the container in, the owners networkID, and the guns networkID and ammo properties are provided
    //Server constructs a new Gun container object, and transfers the players gun from the player to the container
    //Client is sent the containers ID, the guns ID, and ammo data, where the containers gun is set, and the guns ammo data is set

    [Command]
    public void CmdTransferGunToContainer(Vector3 spawnLocation, NetworkInstanceId ownerID, NetworkInstanceId gunID, GunBase.AmmoData ammoData)
    {
        GameObject gunContainerPrefab = (GameObject)Instantiate(GetPrefabByName("GunContainer"), spawnLocation, new Quaternion());
        GunContainer gunContainerScript = gunContainerPrefab.GetComponent<GunContainer>();
        
        GameObject ownerGun = NetworkServer.FindLocalObject(gunID);
        gunContainerScript.SetContainedGun(ownerGun);

        GunBase gunScript = gunContainerScript.containedGun.GetComponent<GunBase>();
        gunScript.ammoData = ammoData;

        gunContainerPrefab.GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 100), ForceMode.Acceleration);

        NetworkServer.Spawn(gunContainerPrefab);

        NetworkInstanceId containerID = gunContainerPrefab.GetComponent<NetworkIdentity>().netId;
        
        RpcTranferGunToContainer(containerID, gunID, ammoData);
    }

    [ClientRpc]
    public void RpcTranferGunToContainer(NetworkInstanceId containerID, NetworkInstanceId gunID, GunBase.AmmoData ammoData)
    {
        if (isServer)
            return;

        GameObject gunContainerPrefab = ClientScene.FindLocalObject(containerID);
        GunContainer gunContainerScript = gunContainerPrefab.GetComponent<GunContainer>();

        gunContainerScript.SetContainedGun(ClientScene.FindLocalObject(gunID));

        GunBase gunScript = gunContainerScript.containedGun.GetComponent<GunBase>();
        gunScript.ammoData = ammoData;
    }

    //---------------------------------------------------------------------------------------------------------------
    //Takes in the player recieving the weapon and the container it is being transfered from
    //The players netID and the gunID they are picking up is sent to the client, where the players current gun is assigned
    
    [Command]
    public void CmdTranferGunToPlayer(NetworkInstanceId playerID, NetworkInstanceId containerID)
    {
        GameObject player = NetworkServer.FindLocalObject(playerID);
        GunManager playerGunManagerScript = player.GetComponent<GunManager>();

        GameObject gunContainer = NetworkServer.FindLocalObject(containerID);
        GunContainer gunContainerScript = gunContainer.GetComponent<GunContainer>();

        playerGunManagerScript.SetPlayersGun(gunContainerScript.containedGun);

        RpcTransferGunToPlayerOnClient(playerID, gunContainerScript.containedGun.GetComponent<NetworkIdentity>().netId);

        NetworkServer.Destroy(NetworkServer.FindLocalObject(containerID));
    }

    [ClientRpc]
    public void RpcTransferGunToPlayerOnClient(NetworkInstanceId playerID, NetworkInstanceId gunID)
    {
        if (isServer)
            return;

        GameObject player = ClientScene.FindLocalObject(playerID);
        GunManager playerGunManagerScript = player.GetComponent<GunManager>();

        GameObject gun = ClientScene.FindLocalObject(gunID);
        GunBase gunScript = gun.GetComponent<GunBase>();

        playerGunManagerScript.SetPlayersGun(gun);
    }

    //----------------------------------------------------------------------------------------------------------------
    //Takes in the players netID
    //Calls the PlayerInfoManager.KillPlayer() command on both the server and client, killing the player and starting the respawn process on the clients side
    [Command]
    public void CmdKillPlayerOnServer(NetworkInstanceId playerID)
    {
        GameObject player = NetworkServer.FindLocalObject(playerID);
        ServerHitDetection hitDetection = player.GetComponent<ServerHitDetection>();

        hitDetection.InflictDamageOnPlayer(100000);

        //RpcKillPlayerOnClient(playerID);   
    }

    [ClientRpc]
    public void RpcKillPlayerOnClient(NetworkInstanceId playerID)
    {
        if (isServer)
            return;

        GameObject player = ClientScene.FindLocalObject(playerID);
        PlayerInfoManager playerScript = player.GetComponent<PlayerInfoManager>();

        //playerScript.KillPlayer();
    }

    //----------------------------------------------------------------------------------------------------------------
    //Takes in the players netID
    //Calls the PlayerInfoManager.RespawnPlayer() on both the server and the client, respawning the player
    [Command]
    public void CmdRespawnPlayerOnServer(NetworkInstanceId playerID)
    {
        GameObject player = NetworkServer.FindLocalObject(playerID);
        PlayerInfoManager playerScript = player.GetComponent<PlayerInfoManager>();

        //playerScript.RespawnPlayer();

        RpcRespawnPlayerOnClient(playerID);
    }

    [ClientRpc]
    public void RpcRespawnPlayerOnClient(NetworkInstanceId playerID)
    {
        if (isServer)
            return;

        GameObject player = ClientScene.FindLocalObject(playerID);
        PlayerInfoManager playerScript = player.GetComponent<PlayerInfoManager>();

        //playerScript.RespawnPlayer();
    }


    //-----------------------------------------------------------------------------------------------------------------
    [ClientRpc]
    public void RpcDebugMessage(string message)
    {
        Debug.LogWarning(message);
    }
}
