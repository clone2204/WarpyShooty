using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public interface LobbyManager
{
    void AddPlayer(NetworkConnection playerConnection);

    void RemovePlayer();

    void SwapPlayers();

    void KickPlayer(NetworkConnection playerConnection);

    void BanPlayer();
}
