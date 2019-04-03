using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour, IGameManager
{
    private IGameManager realGameManager;

    private PlayerHUDManager hudManager;

    public enum Team
    {
        Red,
        Blue,
        NONE
    }
    // Start is called before the first frame update
    void Start()
    {
        realGameManager = GetComponent<GameManager_Server>();

    }

    // Update is called once per frame
    void Update()
    {
        if(hudManager != null)
            hudManager.UpdateGameTime(GetGameTime());
    }

    public void SetupGame(List<LobbyPlayerManager> players, int timeLimit, int KillLimit)
    {
        if (!isServer)
            return;

        
        realGameManager.SetupGame(players, timeLimit, KillLimit);
    }

    public void LoadPlayer(LobbyPlayerManager lobbyPlayer, GamePlayerManager gamePlayer)
    {
        if (!isServer)
            return;

        realGameManager.LoadPlayer(lobbyPlayer, gamePlayer);
    }

    public void StartGame()
    {
        if (!isServer)
            return;

        realGameManager.StartGame();
    }

    [ClientRpc]
    public void RpcStartGameClock()
    {
        hudManager = GameObject.Find("PlayerHud").GetComponent<PlayerHUDManager>();
    }

    public int GetGameTime()
    {
        return realGameManager.GetGameTime();
    }
}
