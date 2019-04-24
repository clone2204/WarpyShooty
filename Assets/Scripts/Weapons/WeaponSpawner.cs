using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject containerPrefab;
    
    public GameObject weapon;

	// Use this for initialization
	void Start ()
    {
        NetworkLobbyManager lobbyManager = GameObject.Find("_SCRIPTS_").GetComponent<NetworkLobbyManager>();

        foreach (GameObject prefab in lobbyManager.spawnPrefabs)
        {
            if(prefab.name == "GunContainer")
            {
                containerPrefab = prefab;
            }
        }
        
        SpawnWeapon();
	}

    public void SpawnWeapon()
    {
        GameObject gunContainer = (GameObject)Instantiate(containerPrefab, transform.position, new Quaternion());
        GunContainer gunContainerScript = gunContainer.GetComponent<GunContainer>();

        gunContainerScript.SetContainedGun(((GameObject)Instantiate(weapon)).GetComponent<IWeapon>());

        //NetworkServer.Spawn(gunContainer);
    }
	
	
}
