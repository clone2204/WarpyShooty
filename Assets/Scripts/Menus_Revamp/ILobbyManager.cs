using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public interface ILobbyManager
{
    void Init(ILobbyManager lobbyManager);

    void Clear();

    void AddPlayer(NetworkConnection playerConnection, short controllerID);

    void RemovePlayer(NetworkConnection playerConnection);

    void SwapPlayers(List<int> players);

    void KickPlayers(List<int> players);

    void BanPlayers(List<int> players);
}
