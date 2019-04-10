using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public interface ILobbyManager
{
    void Init();

    void Clear();

    void AddPlayer(NetworkConnection playerConnection, short controllerID);

    void RemovePlayer(NetworkConnection playerConnection);

    void SwapPlayers(List<int> players);

    void KickPlayers(List<int> players);

    void BanPlayers(List<int> players);

    void ChangeGameSettings(int timeLimit, int killLimit);

    void StartGame();

    void PlayerLoaded(LobbyPlayerManager lobbyPlayer, GamePlayerManager gameManager);

    void EndGame();
}
