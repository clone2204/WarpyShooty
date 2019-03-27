using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour, IGameManager
{
    private IGameManager realGameManager;

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
        
    }

    public void SetupGame(List<PlayerManager> players, int timeLimit, int KillLimit)
    {
        if (!isServer)
            return;

        realGameManager.SetupGame(players, timeLimit, KillLimit);
    }

    public void StartGame()
    {
        if (!isServer)
            return;

        realGameManager.StartGame();
    }
}
