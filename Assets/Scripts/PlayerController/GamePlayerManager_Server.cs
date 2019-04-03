using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayerManager_Server : NetworkBehaviour, IGamePlayerManager
{
    private GamePlayerManager clientPlayer;

    private string playerName;
    private GameManager.Team playerTeam;

    private int playerHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        clientPlayer = GetComponent<GamePlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupPlayer(string playerName, GameManager.Team playerTeam)
    {
        Debug.LogWarning("PLAYER SETUP: " + playerName + " || " + playerTeam);
        this.playerName = playerName;
        this.playerTeam = playerTeam;

        clientPlayer.TargetSetupPlayer(this.connectionToClient);
    }

    public string GetName()
    {
        return playerName;
    }

    public GameManager.Team GetTeam()
    {
        return playerTeam;
    }

    public void SetHealth(int health)
    {
        this.playerHealth = health;
    }

    public int GetHealth()
    {
        return this.playerHealth;
    }

    public void SpawnPlayer(Vector3 spawnPoint)
    {
        Debug.LogWarning("Spawn Player");
        transform.position = spawnPoint;
        this.playerHealth = 100;

        clientPlayer.TargetClientSpawnTasks(this.connectionToClient);
    }
}
