using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DummyPlayer : GamePlayer
{
    [SerializeField] private int respawnTime;
    [SerializeField] private int fireTime;

    
    // Start is called before the first frame update
    void Start()
    {
        

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void DamagePlayer(int damage, GamePlayer damageSource)
    {
        if (damageSource.GetTeam() == playerTeam)
            return;

        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            playerHealth = 100;
            gameManager.PlayerScore((RealPlayer)damageSource);

            StartCoroutine(TargetRespawn());
        }
    }

    public override Vector3 GetPlayerLookDirection()
    {
        return transform.localRotation.eulerAngles;
    }

    private IEnumerator TargetRespawn()
    {
        Vector3 position = transform.localPosition;
        transform.localPosition = new Vector3();

        yield return new WaitForSecondsRealtime(respawnTime);

        transform.localPosition = position;
    }
    
}
