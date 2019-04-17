using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScoreManager : MonoBehaviour
{
    Text winner;
    Text blueScore;
    Text redScore;

    Transform playerList;
    
    // Start is called before the first frame update
    void Start()
    {
        winner = transform.Find("Winner").GetComponent<Text>();
        blueScore = transform.Find("BlueScore").GetComponent<Text>();
        redScore = transform.Find("RedScore").GetComponent<Text>();

        playerList = transform.Find("PlayerList").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScores(GameManager.Team winningTeam, int blueScore, int redScore, Dictionary<string, int> blueScores, Dictionary<string, int> redScores)
    {
        winner.text = winningTeam.ToString() + " Team Wins!";

        this.blueScore.text = "" + blueScore;
        this.redScore.text = "" + redScore;

        int nameOffset = 0;
        foreach(string name in blueScores.Keys)
        {
            Transform player = playerList.Find("player" + nameOffset);
            player.Find("Name").GetComponent<Text>().text = name;
            player.Find("Score").GetComponent<Text>().text = blueScores[name] + "";

            nameOffset++;
        }

        nameOffset = 6;
        foreach (string name in redScores.Keys)
        {
            Transform player = playerList.Find("Player" + nameOffset);
            player.Find("Name").GetComponent<Text>().text = name;
            player.Find("Score").GetComponent<Text>().text = redScores[name] + "";

            nameOffset++;
        }
    }
}
