using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public interface ILobbyManager
{
    void Init(ILobbyManager lobbyManager);

    void Clear();

    void AddPlayer(NetworkConnection playerConnection, short controllerID);

    void RemovePlayer(NetworkConnection playerConnection);

    void SwapPlayers();

    void KickPlayer(NetworkConnection playerConnection);

    void BanPlayer();
}
