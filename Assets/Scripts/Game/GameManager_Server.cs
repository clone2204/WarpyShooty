using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager_Server : NetworkBehaviour, IGameManager
{
    private GameManager clientManager;

    [SyncVar] private int gameTime;
    private bool gameActive;

    private List<LobbyPlayerManager> lobbyPlayers;
    private int playerCount;
    private Dictionary<GamePlayerManager, int> gamePlayers;
    private int killLimit;

    private Dictionary<GameManager.Team, List<Vector3>> spawnPoints;

    [SerializeField] private int endGameTime;

    // Start is called before the first frame update
    void Start()
    {
        clientManager = GetComponent<GameManager>();
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

    public void SetupGame(List<LobbyPlayerManager> players, int timeLimit, int KillLimit)
    {
        this.killLimit = killLimit;
        timeLimit = 1;
        this.gameTime = timeLimit * 60;
        this.lobbyPlayers = players;
        this.gamePlayers = new Dictionary<GamePlayerManager, int>();

        playerCount = 0;
        foreach(LobbyPlayerManager player in lobbyPlayers)
        {
            if (player != null)
                playerCount++;
        }
    }

    public void LoadPlayer(LobbyPlayerManager lobbyPlayer, GamePlayerManager gamePlayer)
    {
        StartCoroutine(WaitForPlayerLoad(this.lobbyPlayers, this.gamePlayers, lobbyPlayer, gamePlayer));
    }

    private IEnumerator WaitForPlayerLoad(List<LobbyPlayerManager> lobbyPlayers, Dictionary<GamePlayerManager, int> gamePlayers, LobbyPlayerManager lobbyPlayer, GamePlayerManager gamePlayer)
    {
        yield return new WaitUntil(() => gamePlayer.isServer);

        Debug.LogWarning("DING");
        int playerIndex = lobbyPlayers.IndexOf(lobbyPlayer);

        GameManager.Team playerTeam = (playerIndex < 6 ? GameManager.Team.Blue : GameManager.Team.Red);

        gamePlayer.SetupPlayer(lobbyPlayer.GetName(), playerTeam);
        Debug.LogWarning("Gameplayers add");
        gamePlayers.Add(gamePlayer, 0);
        Debug.LogWarning("Gamelayers after add: " + gamePlayers.Keys.Count);

        if (gamePlayers.Keys.Count == playerCount)
            StartGame();
    }

    public void StartGame()
    {
        Debug.LogWarning("START GAME");
        gameActive = true;
        StartCoroutine(Timer());

        this.spawnPoints = LoadSpawnPoints();

        Debug.LogWarning("Gameplayers: " + gamePlayers.Keys.Count);
        foreach (GamePlayerManager gamePlayer in gamePlayers.Keys)
        {
            Debug.LogWarning("Spawn START");
            SpawnPlayer(gamePlayer);
        }

        clientManager.RpcStartGameClock();
    }

    private IEnumerator Timer()
    {
        while (gameActive && this.gameTime > 0)
        {
            yield return new WaitForSecondsRealtime(1f);

            this.gameTime -= 1;
        }

        if(gameActive)
            EndGame();
    }

    public int GetGameTime()
    {
        return gameTime;
    }

    private void SpawnPlayer(GamePlayerManager player)
    {
        GameManager.Team playerTeam = player.GetTeam();

        System.Random rand = new System.Random();
        int random = rand.Next(spawnPoints[playerTeam].Count);
        Vector3 respawnPoint = spawnPoints[playerTeam][random];

        player.SpawnPlayer(respawnPoint);
    }

    public void EndGame()
    {
        gameActive = false;

        int blueScore = 0;
        int redScore = 0;
        List<string> blueNames = new List<string>();
        List<string> redNames = new List<string>();
        List<int> blueScores = new List<int>();
        List<int> redScores = new List<int>();

        foreach(GamePlayerManager player in gamePlayers.Keys)
        {
            string name = player.GetName();
            int score = gamePlayers[player];

            GameManager.Team team = player.GetTeam();
            if(team == GameManager.Team.Blue)
            {
                blueScore += score;
                blueNames.Add(name);
                blueScores.Add(score);
            }
            else if(team == GameManager.Team.Red)
            {
                redScore += score;
                redNames.Add(name);
                redScores.Add(score);
            }

            player.DespawnPlayer();
        }

        GameManager.Team winner;
        if(blueScore > redScore)
        {
            winner = GameManager.Team.Blue;
        }
        else if(redScore > blueScore)
        {
            winner = GameManager.Team.Red;
        }
        else
        {
            winner = GameManager.Team.NONE;
        }

        clientManager.RpcEndGame(winner, blueScore, redScore, blueNames.ToArray(), blueScores.ToArray(), redNames.ToArray(), redScores.ToArray());
        StartCoroutine(EndGameTimer());
    }

    private IEnumerator EndGameTimer()
    {
        int endgame = endGameTime;
        while(endgame > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            endgame -= 1;
        }

        clientManager.RpcLobbyReturn();
        GameObject.Find("_SCRIPTS_").GetComponent<NetworkLobbyManager>().ServerReturnToLobby();
    }
}
