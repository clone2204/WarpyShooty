using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GunContainer : NetworkBehaviour {

    //private GameObject localPlayer;
    public GameObject containedGun;
    public NetworkInstanceId gunID;


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
        if (col.gameObject.name != "Player(Clone)")
            return;

        col.gameObject.GetComponent<GunManager>().SetSwappableGunContainer(this.gameObject);
        
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name != "Player(Clone)")
            return;

        col.gameObject.GetComponent<GunManager>().SetSwappableGunContainer(null);
        
    }

    public void SetContainedGun(GameObject gun)
    {
        if (gun == null)
            return;

        containedGun = gun;
        containedGun.transform.parent = transform;
        containedGun.transform.localPosition = new Vector3(0, .7f, 0);
    }

    public void DestroyContainer()
    {
        containedGun = null;
        GameObject.Destroy(this.gameObject);
    }
}
