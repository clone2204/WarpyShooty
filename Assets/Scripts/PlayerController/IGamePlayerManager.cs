using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGamePlayerManager
{
    void SetupPlayer(string playerName, GameManager.Team playerTeam);

    string GetName();

    GameManager.Team GetTeam();

    void SpawnPlayer(Vector3 spawnPoint);

    void SetHealth(int health);

    int GetHealth();
}
