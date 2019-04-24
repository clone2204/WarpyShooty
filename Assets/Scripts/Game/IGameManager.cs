using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    void SetupGame(List<LobbyPlayerManager> players, int timeLimit, int KillLimit);

    void LoadPlayer(LobbyPlayerManager lobbyPlayer, RealPlayer gamePlayer);

    void StartGame();

    int GetGameTime();

    void EndGame();
}
