using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ServerHitDetection : NetworkBehaviour
{
    ILobbyManager lobbyManager;
    IPlayerManager playerInfoManager;
    PlayerHUDManager playerHUD;
    GunManager gunManager;

    public float respawnCooldownSEC; // Respawn is in seconds
    
    private int playerHealth;

    // Use this for initialization
    void Start ()
    {
        if (!isServer)
        {
            this.enabled = false;
            return;
        }
        
        lobbyManager = GameObject.Find("_SCRIPTS_").GetComponent<ILobbyManager>();
        playerInfoManager = GetComponent<IPlayerManager>();
        playerHUD = transform.Find("PlayerHud").GetComponent<PlayerHUDManager>();
        gunManager = GetComponent<GunManager>();
        
        //playerHealth = GetComponent<PlayerInfoManager>().getPlayerFullHealth();
	}
    
    public void InflictDamageOnPlayer(int damage)
    {
        //Debug.Log("INFLICT DAMAGE ON " + playerInfoManager.playerInfo.playerName);

        playerHealth -= damage;
        
        if (playerHealth <= 0)
        {
            this.KillPlayer();
        }

        TargetUpdateClientDamage(GetComponent<NetworkIdentity>().connectionToClient, playerHealth);
    }

    [TargetRpc]
    public void TargetUpdateClientDamage(NetworkConnection connection, int playerHealth)
    {
        Debug.Log("CLIENT DAMAGE UPDATE");

    }

    private IEnumerator RespawnCoroutine(float respawnTime)
    {
        yield return new WaitForSecondsRealtime(respawnTime);

        this.RespawnPlayer();
    }


    public void KillPlayer()
    {
        //gunManager.CmdThrowGun();

        GameObject deathWaitPoint = GameObject.Find("RespawnCooldownArea");
        transform.position = deathWaitPoint.transform.position;

        //Disables Player Controller and enables death camera controls
        RpcUpdatePlayerPositionOnClient(transform.position);
        TargetEnableClientDeathCam(GetComponent<NetworkIdentity>().connectionToClient);

        //Respawn countdown
        StartCoroutine(RespawnCoroutine(respawnCooldownSEC));
    }

    [ClientRpc]
    public void RpcUpdatePlayerPositionOnClient(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void RespawnPlayer()
    {
        //playerHealth = GetComponent<PlayerInfoManager>().getPlayerFullHealth();

        gunManager.CmdDropCurrentWeapon();
        gunManager.CmdSetGunToStarter();
        
        transform.position = this.GetRespawnPosition();
        //GetComponent<Warp>().UpdatePlayerLocation();

        RpcUpdatePlayerPositionOnClient(transform.position);
        TargetUpdateClientDamage(GetComponent<NetworkIdentity>().connectionToClient, playerHealth);
        TargetDisableClientDeathCam(GetComponent<NetworkIdentity>().connectionToClient);
    }

    public Vector3 GetRespawnPosition()
    {
        Vector3 newPosition = new Vector3();

        List<GameObject> spawnPoints = new List<GameObject>();
        //spawnPoints = lobbyManager.FindTeamSpawnPoints(GetComponent<PlayerInfoManager>().playerInfo.playerTeam);

        newPosition = spawnPoints.ToArray()[Random.Range(0, spawnPoints.Count)].transform.position;

        return newPosition;
    }

    [TargetRpc]
    public void TargetEnableClientDeathCam(NetworkConnection target)
    {
        GetComponent<LocalPlayerController>().enabled = false;
        GetComponentInChildren<Camera>().enabled = false;

        GameObject.Find("ObserverCamSystem").GetComponentInChildren<Camera>().enabled = true;
        GameObject.Find("ObserverCamSystem").GetComponent<ObserverCamera>().enabled = true;
    }
   
    [TargetRpc]
    public void TargetDisableClientDeathCam(NetworkConnection target)
    {
        GameObject.Find("ObserverCamSystem").GetComponentInChildren<Camera>().enabled = false;
        GameObject.Find("ObserverCamSystem").GetComponent<ObserverCamera>().enabled = false;

        GetComponent<LocalPlayerController>().enabled = true;
        GetComponentInChildren<Camera>().enabled = true;
    }

    
    
}
