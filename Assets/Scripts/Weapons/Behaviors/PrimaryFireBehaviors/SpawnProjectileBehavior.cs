using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnProjectileBehavior : NetworkBehaviour, IPrimaryFireBehavior
{
    [SerializeField] private GameObject projectile;

    [SerializeField] private float refireTime;
    [SerializeField] private int projectileDamage;
    [SerializeField] private float projectileVelocity;

    private bool primaryFireActive;
    private bool primaryFireOnCooldown;

    private IAltFireBehavior altFireBehavior;
    private IAmmoBehavior ammoBehavior;

    private GamePlayer player;
    private Func<Vector3> GetSpawnLocation;
    private Func<Vector3> GetSpawnDirection;


    public void Init(IAltFireBehavior altFireBehavior, IAmmoBehavior ammoBehavior)
    {
        this.altFireBehavior = altFireBehavior;
        this.ammoBehavior = ammoBehavior;

        primaryFireActive = false;
        primaryFireOnCooldown = false;

        StartCoroutine(RefireCooldownCoroutine());
    }

    public void PrimaryFireStart(GamePlayer player, System.Func<Vector3> GetSpawnLocation, System.Func<Vector3> GetSpawnDirection)
    {
        primaryFireActive = true;

        this.player = player;
        this.GetSpawnLocation = GetSpawnLocation;
        this.GetSpawnDirection = GetSpawnDirection;
    }

    public void PrimaryFireStop()
    {
        primaryFireActive = false;

        this.player = null;
        this.GetSpawnLocation = null;
    }

    public bool GetPrimaryFireActive()
    {
        return primaryFireActive;
    }

    private IEnumerator RefireCooldownCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => primaryFireActive);

            while (primaryFireActive && !ammoBehavior.GetReloadActive())
            {
                primaryFireOnCooldown = true;

                if(ammoBehavior.ConsumeAmmo(1))
                {
                    GameObject newProjectile = Instantiate<GameObject>(projectile, GetSpawnLocation(), new Quaternion());
                    IProjectile projectileScript = newProjectile.GetComponent<IProjectile>();
                    projectileScript.Init(player, GetSpawnDirection(), projectileVelocity, projectileDamage);

                    NetworkServer.Spawn(newProjectile);
                }
                
                yield return new WaitForSecondsRealtime(refireTime);

                primaryFireOnCooldown = false;
            }
        }
    }
}
