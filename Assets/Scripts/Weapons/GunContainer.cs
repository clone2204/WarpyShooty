using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GunContainer : NetworkBehaviour {

    private IWeapon containerWeapon;
    
    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (this.containerWeapon != null)
            MoveGun();
	}

    private void MoveGun()
    {
        containerWeapon.GetWeaponObject().transform.Rotate(Vector3.up, 1f);
        containerWeapon.GetWeaponObject().transform.localPosition = new Vector3(0, .7f + Mathf.Sin(Time.time) / 8, 0); 
    }

    void OnTriggerEnter(Collider col)
    {
        WeaponManager playerGunManager = col.GetComponent<WeaponManager>();

        if (playerGunManager == null)
            return;


        playerGunManager.SetSwappableGunContainer(this);
    }

    void OnTriggerExit(Collider col)
    {
        WeaponManager playerGunManager = col.GetComponent<WeaponManager>();

        if (playerGunManager == null)
            return;


        playerGunManager.SetSwappableGunContainer(null);

    }

    public void SetContainedGun(IWeapon weapon)
    {
        if (weapon == null)
            return;

        containerWeapon = weapon;
        weapon.SetWeaponPosition(transform);
    }

    public IWeapon GetContainedGun()
    {
        return this.containerWeapon;
    }

    public void DestroyContainer()
    {
        containerWeapon = null;
        GameObject.Destroy(this.gameObject);
    }
}
