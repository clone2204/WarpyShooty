using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ProjectileBase : NetworkBehaviour {

    private LevelServerCommands serverCommands;
    public ProjectileProperties properties;

    public struct ProjectileProperties
    {
        public Vector3 spawnLocation;
        public Vector3 spawnDirection;
        public float velocity;
        public int damage;

        public ProjectileProperties(Vector3 spawnLocation, Vector3 spawnDirection, float velocity, int damage)
        {
            this.spawnLocation = spawnLocation;
            this.spawnDirection = spawnDirection;
            this.velocity = velocity;
            this.damage = damage;
        }

        public ProjectileProperties(float velocity, int damage)
        {
            this.spawnLocation = new Vector3();
            this.spawnDirection = new Vector3();
            this.velocity = velocity;
            this.damage = damage;
        }
    }

    Rigidbody bulletPhysics;
    public int cullTime;


    // Use this for initialization
    void Start()
    {
        serverCommands = GameObject.Find("_SCRIPTS_").GetComponentInChildren<LevelServerCommands>();

        bulletPhysics = this.GetComponent<Rigidbody>();
        bulletPhysics.velocity = this.properties.spawnDirection * this.properties.velocity;

        DestroyBulletInSeconds(cullTime);
    }

    //Detects when bullet collides with something, damage is only done if bullet hits on the server
    void OnTriggerEnter(Collider other)
    {
       
        if ((other.gameObject.tag == "localPlayer" || other.gameObject.tag == "Player") && isServer)
        {
            //Debug.LogWarning("PLAYER " + other.gameObject.GetComponent<PlayerInfoManager>().playerInfo.playerName + " HIT on server: " + isServer);
            other.gameObject.GetComponent<ServerHitDetection>().InflictDamageOnPlayer(this.properties.damage);
        }
        DestroyBulletInSeconds(0);
    }

    //Allows Gun to set direction Vector
    public void SetProjectileProperties(ProjectileProperties properties)
    {
        this.properties = properties;
    }

    //superfluous method to destroy bullet because fuck you
    private void DestroyBulletInSeconds(int cullTime)
    {
        GameObject.Destroy(gameObject, cullTime);
    }

}
