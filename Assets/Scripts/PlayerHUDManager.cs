using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHUDManager : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerManager playerManager;

    private Transform gameInfoHUD;
    private Transform worldInfoHUD;
    private Transform playerInfoHUD;
    private Transform gunInfoHUD;
    private Transform centerHUD;
    
	// Use this for initialization
	void Start ()
    {
        gameInfoHUD = transform.Find("GameInfoHUD");
        worldInfoHUD = transform.Find("WorldInfoHUD");
        playerInfoHUD = transform.Find("PlayerInfoHUD");
        gunInfoHUD = transform.Find("GunInfoHUD");
        centerHUD = transform.Find("CenterHUD");

        gameManager = GameObject.Find("_SCRIPTS_").GetComponentInChildren<GameManager>();
        playerManager = GameObject.FindGameObjectWithTag("localPlayer").GetComponent<PlayerManager>();


        SetPlayerName(playerManager.GetName());
    }

   
	// Update is called once per frame
	void Update ()
    {
        UpdateGameTime();   
	}

    private void UpdateGameTime()
    {
        Text gameTime = gameInfoHUD.Find("GameTimer").GetComponent<Text>();
        int time = gameManager.GetGameTime();

        gameTime.text = "Time: " + (time / 60) + ":" + ((time % 60) < 10 ? "0" + (time % 60) : "" + (time % 60));
    }

    public void SetPlayerName(string name)
    {
        Text playerName = playerInfoHUD.Find("Player Name").GetComponent<Text>();
        playerName.text = name;
    }
}
