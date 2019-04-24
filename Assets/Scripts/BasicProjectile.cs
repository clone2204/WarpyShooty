using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BasicProjectile : NetworkBehaviour, IProjectile
{
    [SerializeField] private int cullTime;

    private Rigidbody projectilePhysics;

    private RealPlayer owner;
    private Vector3 direction;
    private float velocity;
    private int damage;
    
    public void Init(RealPlayer owner, Vector3 direction, float velocity, int damage)
    {
        projectilePhysics = this.GetComponent<Rigidbody>();

        this.owner = owner;
        this.direction = direction;
        this.velocity = velocity;
        this.damage = damage;

        projectilePhysics.velocity = direction * velocity;
        DestroyBulletInSeconds(cullTime);
    }

    void OnTriggerEnter(Collider other)
    {
        IDamageEntity targetPlayer = other.GetComponent<IDamageEntity>();

        if (targetPlayer != null && isServer)
        {
            targetPlayer.DamagePlayer(damage, owner);
        }

        DestroyBulletInSeconds(0);
    }

    private void DestroyBulletInSeconds(int cullTime)
    {
        GameObject.Destroy(gameObject, cullTime);
    }
}
