using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    void SetupGame(List<PlayerManager> players, int timeLimit, int KillLimit);

    void StartGame();

    int GetGameTime();
}
