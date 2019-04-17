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

    private Func<int, bool> ConsumeAmmo;
    private IAltFireBehavior altFireBehavior;
    private IReloadBehavior reloadBehavior;

    private GamePlayerManager player;
    private Func<Vector3> GetSpawnLocation;
    private Func<Vector3> GetLookDirection;

    public void Init(Func<int, bool> ConsumeAmmo, IAltFireBehavior altFireBehavior, IReloadBehavior reloadBehavior)
    {
        this.ConsumeAmmo = ConsumeAmmo;
        this.altFireBehavior = altFireBehavior;
        this.reloadBehavior = reloadBehavior;

        primaryFireActive = false;
        primaryFireOnCooldown = false;

        StartCoroutine(RefireCooldownCoroutine());
    }

    public void PrimaryFireStart(GamePlayerManager player, System.Func<Vector3> GetSpawnLocation, Func<Vector3> GetLookDirection)
    {
        primaryFireActive = true;

        this.player = player;
        this.GetSpawnLocation = GetSpawnLocation;
        this.GetLookDirection = GetLookDirection;
    }

    public void PrimaryFireStop()
    {
        primaryFireActive = false;

        this.player = null;
        this.GetSpawnLocation = null;
        this.GetLookDirection = null;
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

            while (primaryFireActive && !reloadBehavior.GetReloadActive())
            {
                primaryFireOnCooldown = true;

                if(!ConsumeAmmo(1))
                {
                    StartCoroutine(StartReload());
                }
                else
                {
                    //This method is not working
                    //Probably need to init projectile after spawning it
                    GameObject newProjectile = Instantiate<GameObject>(projectile, GetSpawnLocation(), new Quaternion());
                    IProjectile projectileScript = newProjectile.GetComponent<IProjectile>();
                    projectileScript.Init(player, GetLookDirection(), projectileVelocity, projectileDamage);

                    NetworkServer.Spawn(newProjectile);
                }

                yield return new WaitForSecondsRealtime(refireTime);

                primaryFireOnCooldown = false;
            }
        }
    }

    private IEnumerator StartReload()
    {
        reloadBehavior.StartReload();

        yield return new WaitForSecondsRealtime(.2f);

        reloadBehavior.StopReload();
    }
}
