using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LevelServerCommands : NetworkBehaviour {

    private NetworkLobbyManager networkManager;
    private MatchManager matchManager;
    private List<GameObject> prefabList;

    public void Start()
    {
        this.networkManager = GetComponentInParent<NetworkLobbyManager>();
        matchManager = GetComponentInParent<MatchManager>();
        this.prefabList = this.networkManager.spawnPrefabs;
    }

    private GameObject GetPrefabByName(string name)
    {
        if (this.prefabList == null)
            this.prefabList = GetComponentInParent<NetworkLobbyManager>().spawnPrefabs;


        foreach (GameObject gameObject in this.prefabList)
        {
            if (gameObject.name == name)
            {
                return gameObject;
            }
        }

        return null;
    }
    
    
    [Command]
    public void CmdInitializeContainerGunOnServer(NetworkInstanceId containerID, string gunName)
    {
        GameObject gunContainer = NetworkServer.FindLocalObject(containerID);
        GunContainer gunContainerScript = gunContainer.GetComponent<GunContainer>();

        GameObject gun = (GameObject)Instantiate(GetPrefabByName(gunName));
        gunContainerScript.SetContainedGun(gun);

        NetworkServer.Spawn(gun);

        RpcInitializeContainerGunOnClient(containerID, gun.GetComponent<NetworkIdentity>().netId);
    }

    [ClientRpc]
    public void RpcInitializeContainerGunOnClient(NetworkInstanceId containerID, NetworkInstanceId gunID)
    {
        GameObject gunContainer = ClientScene.FindLocalObject(containerID);
        GunContainer gunContainerScript = gunContainer.GetComponent<GunContainer>();

        GameObject gun = ClientScene.FindLocalObject(gunID);
        gunContainerScript.SetContainedGun(gun);
    }

    //----------------------------------------------------------------------------------------------------------------
    [Command]
    public void CmdDamagePlayerOnServer(NetworkInstanceId playerID, int damageAmount)
    {
        GameObject player = NetworkServer.FindLocalObject(playerID);
        PlayerInfoManager playerInfoScript = player.GetComponent<PlayerInfoManager>();

        //playerInfoScript.InflictDamageOnPlayer(damageAmount);

        Debug.LogWarning("SERVER DAMAGE");
        RpcDamagePlayerOnClient(playerID, damageAmount);
    }

    [ClientRpc]
    public void RpcDamagePlayerOnClient(NetworkInstanceId playerID, int damageAmount)
    {
        if (isServer)
            return;

        GameObject player = ClientScene.FindLocalObject(playerID);
        PlayerInfoManager playerInfoScript = player.GetComponent<PlayerInfoManager>();

        //playerInfoScript.InflictDamageOnPlayer(damageAmount);
    }
}
