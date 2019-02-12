using UnityEngine;
using System.Collections;

public class ProjectileGunBase : GunBase
{
    public GameObject projectilePrefab;
    public float velocity;

    public override void FireAction()
    {
        this.gunManager.SpawnProjectile(this.projectilePrefab.name, new ProjectileBase.ProjectileProperties(gunManager.GetBulletSpawnLocation(), gunManager.GetLookRotation(), this.velocity, this.damage));
    }

    public override void AltFireAction()
    {

    }
}
