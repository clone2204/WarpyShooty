using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TargetDummy : NetworkBehaviour
{
    private GameManager gameManager;
    private Transform weaponPort;

    [SerializeField] private GameObject startWeapon;
    [SerializeField] private float respawnTime;
    [SerializeField] private float refireTime;

    private IWeapon dummyWeapon;
    private int dummyHealth;
    private bool dummyAlive;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("_SCRIPTS_").GetComponentInChildren<GameManager>();
        weaponPort = transform.Find("WeaponPort");

        if(startWeapon != null)
        {
            GameObject weapon = Instantiate<GameObject>(startWeapon);
            dummyWeapon = weapon.GetComponent<IWeapon>();
            dummyWeapon.SetWeaponPosition(weaponPort);
        }

        dummyHealth = 100;
        StartCoroutine(DummyFire());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageDummy(GamePlayerManager attacker, int damage)
    {
        dummyHealth -= damage;

        if(dummyHealth <= 0)
        {
            gameManager.PlayerScore(attacker, 1);
            StartCoroutine(DummyRespawn());
        }
    }

    private IEnumerator DummyRespawn()
    {
        Vector3 normalPosition = transform.localPosition;
        transform.localPosition = normalPosition + new Vector3(0, -100, 0);

        dummyAlive = false;

        yield return new WaitForSecondsRealtime(respawnTime);

        dummyAlive = true;

        dummyHealth = 100;
        transform.localPosition = normalPosition;
    }

    private IEnumerator DummyFire()
    {
        while(true)
        {
            yield return new WaitUntil(() => dummyAlive);

            while(dummyAlive)
            {
                dummyWeapon.StartPrimaryFire(null, GetSpawnLocation, GetSpawnDirection);

                yield return new WaitForSecondsRealtime(.2f);

                dummyWeapon.StopPrimaryFire();

                yield return new WaitForSecondsRealtime(refireTime);
            }
        }
    }

    private Vector3 GetSpawnDirection()
    {
        float xrot = Mathf.Sin(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y);
        float zrot = Mathf.Cos(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y);
        float yrot = -Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.x);

        return new Vector3(xrot, yrot, zrot);
    }

    private Vector3 GetSpawnLocation()
    {
        float initialX = this.transform.position.x;
        float initialY = this.transform.position.y + .7f;
        float initialZ = this.transform.position.z;

        Vector3 rotation = GetSpawnDirection();

        return new Vector3(initialX + rotation.x, initialY + rotation.y, initialZ + rotation.z);
    }
}
