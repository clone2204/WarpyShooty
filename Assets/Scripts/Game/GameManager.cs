using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour, IGameManager
{
    
    private List<LobbyPlayerManager> lobbyPlayers;
    private int playerCount;
    private Dictionary<RealPlayer, int> gamePlayers;

    private int blueKills;
    private int redKills;
    private int killLimit;

    private Dictionary<GameManager.Team, List<Vector3>> spawnPoints;

    [SyncVar] private int gameTime;
    private bool gameActive;

    [SerializeField] private int respawnTime;
    [SerializeField] private int endGameTime;

    private PlayerHUDManager hudManager;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hudManager != null)
            hudManager.UpdateGameTime(GetGameTime());
    }

    //=================================================================================================
    //Server Functions
    //=================================================================================================

    public void SetupGame(List<LobbyPlayerManager> players, int timeLimit, int killLimit)
    {
        this.killLimit = killLimit;
        this.blueKills = 0;
        this.redKills = 0;

        this.gameTime = timeLimit * 60;
        this.lobbyPlayers = players;
        this.gamePlayers = new Dictionary<RealPlayer, int>();

        playerCount = 0;
        foreach (LobbyPlayerManager player in lobbyPlayers)
        {
            if (player != null)
                playerCount++;
        }
    }

    public void LoadPlayer(LobbyPlayerManager lobbyPlayer, RealPlayer gamePlayer)
    {
        StartCoroutine(WaitForPlayerLoad(this.lobbyPlayers, this.gamePlayers, lobbyPlayer, gamePlayer));
    }


    public void StartGame()
    {
        Debug.LogWarning("START GAME");
        gameActive = true;
        StartCoroutine(Timer());

        this.spawnPoints = LoadSpawnPoints();

        Debug.LogWarning("Gameplayers: " + gamePlayers.Keys.Count);
        foreach (RealPlayer gamePlayer in gamePlayers.Keys)
        {
            Debug.LogWarning("Spawn Player");
            SpawnPlayer(gamePlayer);
        }

        RpcStartGameClock();
    }

    public int GetGameTime()
    {
        return gameTime;
    }

    private void SpawnPlayer(RealPlayer player)
    {
        GameManager.Team playerTeam = player.GetTeam();

        System.Random rand = new System.Random();
        int random = rand.Next(spawnPoints[playerTeam].Count);
        Vector3 respawnPoint = spawnPoints[playerTeam][random];

        player.transform.position = respawnPoint;
        player.SetHealth(100);

        player.EnablePlayer();
    }

    private void DespawnPlayer(RealPlayer player)
    {
        player.transform.position = new Vector3(0, 10, 0);

        player.DisablePlayer();
    }

    public void KillPlayer(RealPlayer player, GamePlayer killer)
    {
        if (killer != null)
        {
            if (player.Equals(killer))
            {
                gamePlayers[player]--;
                DespawnPlayer(player);
                return;
            }

            PlayerScore(killer, 1);

            if (blueKills >= killLimit || redKills >= killLimit)
            {
                EndGame();
            }
        }

<<<<<<< HEAD
        DespawnPlayer(player);
        StartCoroutine(RespawnTimer(player));
    }

    public void PlayerScore(GamePlayerManager player, int scoreInc)
    {
        if (player == null)
            return;

        gamePlayers[player]++;
        if (player.GetTeam() == Team.Blue)
        {
            blueKills++;
        }
        else if (player.GetTeam() == Team.Red)
        {
            redKills++;
        }
=======
        PlayerScore(killer);

        if(blueKills >= killLimit || redKills >= killLimit)
        {
            EndGame();
        }

        DespawnPlayer(player);
        StartCoroutine(RespawnTimer(player));
>>>>>>> c472e59ab410daa365ce01237f6b64f3b06c7e48
    }

    public void PlayerScore(GamePlayer player)
    {
        gamePlayers[player]++;
        if (player.GetTeam() == Team.Blue)
        {
            blueKills++;
        }
        else if (player.GetTeam() == Team.Red)
        {
            redKills++;
        }
    }

    private IEnumerator RespawnTimer(RealPlayer player)
    {
        yield return new WaitForSecondsRealtime(respawnTime);

        SpawnPlayer(player);
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

        foreach (RealPlayer player in gamePlayers.Keys)
        {
            string name = player.GetName();
            int score = gamePlayers[player];

            GameManager.Team team = player.GetTeam();
            if (team == GameManager.Team.Blue)
            {
                blueScore += score;
                blueNames.Add(name);
                blueScores.Add(score);
            }
            else if (team == GameManager.Team.Red)
            {
                redScore += score;
                redNames.Add(name);
                redScores.Add(score);
            }


            DespawnPlayer(player);
        }

        GameManager.Team winner;
        if (blueScore > redScore)
        {
            winner = GameManager.Team.Blue;
        }
        else if (redScore > blueScore)
        {
            winner = GameManager.Team.Red;
        }
        else
        {
            winner = GameManager.Team.NONE;
        }

        RpcEndGame(winner, blueScore, redScore, blueNames.ToArray(), blueScores.ToArray(), redNames.ToArray(), redScores.ToArray());
        StartCoroutine(EndGameTimer());
    }

    
    //=================================================================================================
    //Client Functions
    //=================================================================================================

    [ClientRpc]
    private void RpcStartGameClock()
    {
        hudManager = GameObject.Find("PlayerHud").GetComponent<PlayerHUDManager>();
    }

    [ClientRpc]
    private void RpcEndGame(GameManager.Team winningTeam, int blueScore, int redScore, string[] blueNames, int[] blueScores, string[]redNames, int[] redScores)
    {
        MenuStateManager menuStates = MenuStateManager.GetMenuStateManager();
        menuStates.EndGame();

        Dictionary<string, int> blueTeam = new Dictionary<string, int>();
        for(int loop = 0; loop < blueNames.Length; loop++)
        {
            blueTeam.Add(blueNames[loop], blueScores[loop]);
        }

        Dictionary<string, int> redTeam = new Dictionary<string, int>();
        for (int loop = 0; loop < redNames.Length; loop++)
        {
            redTeam.Add(redNames[loop], redScores[loop]);
        }

        GameObject.Find("EndGameCanvas").GetComponent<EndGameScoreManager>().SetScores(winningTeam, blueScore, redScore, blueTeam, redTeam);
    }

    [ClientRpc]
    private void RpcLobbyReturn()
    {
        MenuStateManager menuStates = MenuStateManager.GetMenuStateManager();
        menuStates.BackToLobby(isServer);
    }

    //=================================================================================================
    //Helper Functions
    //=================================================================================================

    public enum Team
    {
        Red,
        Blue,
        NONE
    }

    private IEnumerator Timer()
    {
        while (gameActive && this.gameTime > 0)
        {
            yield return new WaitForSecondsRealtime(1f);

            this.gameTime -= 1;
        }

        if (gameActive)
            EndGame();
    }

    private IEnumerator WaitForPlayerLoad(List<LobbyPlayerManager> lobbyPlayers, Dictionary<RealPlayer, int> gamePlayers, LobbyPlayerManager lobbyPlayer, RealPlayer gamePlayer)
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

    private Dictionary<GameManager.Team, List<Vector3>> LoadSpawnPoints()
    {
        Dictionary<GameManager.Team, List<Vector3>> spawnPoints = new Dictionary<GameManager.Team, List<Vector3>>();
        spawnPoints.Add(GameManager.Team.Red, new List<Vector3>());
        spawnPoints.Add(GameManager.Team.Blue, new List<Vector3>());
        spawnPoints.Add(GameManager.Team.NONE, new List<Vector3>());

        foreach (GameObject point in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            if (point.GetComponent<SpawnPoint>().team == GameManager.Team.Red)
            {
                spawnPoints[GameManager.Team.Red].Add(point.transform.position);
            }
            else if (point.GetComponent<SpawnPoint>().team == GameManager.Team.Blue)
            {
                spawnPoints[GameManager.Team.Blue].Add(point.transform.position);
            }
            else if (point.GetComponent<SpawnPoint>().team == GameManager.Team.NONE)
            {
                spawnPoints[GameManager.Team.NONE].Add(point.transform.position);
            }
        }

        return spawnPoints;
    }

    private IEnumerator EndGameTimer()
    {
        int endgame = endGameTime;
        while (endgame > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            endgame -= 1;
        }

        RpcLobbyReturn();
        GameObject.Find("_SCRIPTS_").GetComponent<NetworkLobbyManager>().ServerReturnToLobby();
    }


}
