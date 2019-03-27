using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager_Server : NetworkBehaviour, IGameManager
{
    [SyncVar] private int gameTime;
    
    private Dictionary<PlayerManager, int> playerList;
    private int killLimit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupGame(List<PlayerManager> players, int timeLimit, int KillLimit)
    {
        this.killLimit = killLimit;
        this.gameTime = timeLimit * 60;
        this.playerList = new Dictionary<PlayerManager, int>();

        for(int loop = 0; loop < 12; loop++)
        {
            if(players[loop] == null)
            {
                //playerList.Add(null, 0);
                continue;
            }
            else if(loop < 6)
            {
                players[loop].SetTeam(GameManager.Team.Blue);
                playerList.Add(players[loop], 0);
            }
            else
            {
                players[loop].SetTeam(GameManager.Team.Red);
                playerList.Add(players[loop], 0);
            }
        }
    }

    public void StartGame()
    {
        Debug.LogWarning("START GAME");
    }
}
