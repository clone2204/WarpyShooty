using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager_Server : NetworkBehaviour, IGameManager
{
    [SyncVar] private int gameTime;
    
    private Dictionary<PlayerManager, int> playerList;
    private int killLimit;

    private Dictionary<GameManager.Team, List<Vector3>> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Dictionary<GameManager.Team, List<Vector3>> LoadSpawnPoints()
    {
        Dictionary<GameManager.Team, List<Vector3>> spawnPoints = new Dictionary<GameManager.Team, List<Vector3>>();
        spawnPoints.Add(GameManager.Team.Red, new List<Vector3>());
        spawnPoints.Add(GameManager.Team.Blue, new List<Vector3>());
        spawnPoints.Add(GameManager.Team.NONE, new List<Vector3>());

        foreach(GameObject point in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            if(point.GetComponent<SpawnPoint>().team == GameManager.Team.Red)
            {
                spawnPoints[GameManager.Team.Red].Add(point.transform.position);
            }
            else if(point.GetComponent<SpawnPoint>().team == GameManager.Team.Blue)
            {
                spawnPoints[GameManager.Team.Blue].Add(point.transform.position);
            }
            else if(point.GetComponent<SpawnPoint>().team == GameManager.Team.NONE)
            {
                spawnPoints[GameManager.Team.NONE].Add(point.transform.position);
            }
        }

        return spawnPoints;
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
        StartCoroutine(Timer());

        
        this.spawnPoints = LoadSpawnPoints();
        foreach (PlayerManager player in playerList.Keys)
        {
            foreach(NetworkInstanceId playerObject in player.GetPlayerConnection().clientOwnedObjects)
            {
                player.SetPlayerObject(NetworkServer.FindLocalObject(playerObject));
            }

            SpawnPlayer(player);
        }
    }

    private IEnumerator Timer()
    {
        while (this.gameTime > 0)
        {
            yield return new WaitForSecondsRealtime(1f);

            this.gameTime--;
        }
    }

    public int GetGameTime()
    {
        return gameTime;
    }

    private void SpawnPlayer(PlayerManager player)
    {
        List<Vector3> points = this.spawnPoints[player.GetTeam()];
        System.Random rand = new System.Random();
        Vector3 respawnPoint = points[rand.Next(points.Count)];

        player.SpawnPlayer(respawnPoint);
    }

}
