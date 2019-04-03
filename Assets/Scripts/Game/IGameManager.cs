using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    void SetupGame(List<LobbyPlayerManager> players, int timeLimit, int KillLimit);

    void LoadPlayer(LobbyPlayerManager lobbyPlayer, GamePlayerManager gamePlayer);

    void StartGame();

    int GetGameTime();
}
