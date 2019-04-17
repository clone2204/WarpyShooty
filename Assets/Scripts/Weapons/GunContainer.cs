using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GunContainer : NetworkBehaviour {

    [SerializeField] private Gun containedGun;
    
    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (this.containedGun != null)
            MoveGun();
	}

    private void MoveGun()
    {
        containedGun.transform.Rotate(Vector3.up, 1f);
        containedGun.transform.localPosition = new Vector3(0, .7f + Mathf.Sin(Time.time) / 8, 0); 
    }

    void OnTriggerEnter(Collider col)
    {
        GunManager playerGunManager = col.GetComponent<GunManager>();

        if (playerGunManager == null)
            return;


        playerGunManager.SetSwappableGunContainer(this);
    }

    void OnTriggerExit(Collider col)
    {
        GunManager playerGunManager = col.GetComponent<GunManager>();

        if (playerGunManager == null)
            return;


        playerGunManager.SetSwappableGunContainer(null);

    }

    public void SetContainedGun(Gun gun)
    {
        if (gun == null)
            return;

        containedGun = gun;
        containedGun.transform.parent = transform;
        containedGun.transform.localPosition = new Vector3(0, .7f, 0);
    }

    public Gun GetContainedGun()
    {
        return this.containedGun;
    }

    public void DestroyContainer()
    {
        containedGun = null;
        GameObject.Destroy(this.gameObject);
    }
}
