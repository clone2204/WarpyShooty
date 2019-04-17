using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHUDManager : MonoBehaviour
{
    private Camera playerCamera;
    private Camera observerCamera;

    private Canvas hudCanvas;
    private Transform gameInfoHUD;
    private Transform worldInfoHUD;
    private Transform playerInfoHUD;
    private Transform gunInfoHUD;
    private Transform centerHUD;
    
	// Use this for initialization
	void Start ()
    {
        hudCanvas = GetComponent<Canvas>();
        gameInfoHUD = transform.Find("GameInfoHUD");
        worldInfoHUD = transform.Find("WorldInfoHUD");
        playerInfoHUD = transform.Find("PlayerInfoHUD");
        gunInfoHUD = transform.Find("GunInfoHUD");
        centerHUD = transform.Find("CenterHUD");
    }

    public void SetupCameras(Camera playerCamera)
    {
        this.playerCamera = playerCamera;
        observerCamera = GameObject.Find("ObserverCamSystem").GetComponentInChildren<Camera>();
    }

    public void UpdateGameTime(int time)
    {
        Text gameTime = gameInfoHUD.Find("GameTimer").GetComponent<Text>();
        
        gameTime.text = "Time: " + (time / 60) + ":" + ((time % 60) < 10 ? "0" + (time % 60) : "" + (time % 60));
    }

    public void SetPlayerName(string name)
    {
        Text playerName = playerInfoHUD.Find("Player Name").GetComponent<Text>();
        playerName.text = name;
    }

    public void SetPlayerTeam(GameManager.Team playerTeam)
    {
        if (playerTeam == GameManager.Team.Blue)
        {
            playerInfoHUD.gameObject.GetComponent<Image>().color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, .35f);
        }
        else if (playerTeam == GameManager.Team.Red)
        {
            playerInfoHUD.gameObject.GetComponent<Image>().color = new Color(Color.red.r, Color.red.g, Color.red.b, .35f); ;
        }
    }

    public void SetPlayerHealth(int health)
    {
        playerInfoHUD.Find("Player Health").GetComponent<Text>().text = "Health: " + health;
    }

    public void SetWeaponName(string name)
    {
        gunInfoHUD.Find("Gun Name").GetComponent<Text>().text = name;
    }

    public void SetWeaponAmmo(int ammo, int ammoPool)
    {
        gunInfoHUD.Find("Ammo Count").GetComponent<Text>().text = ammo + " / " + ammoPool;
    }

    public void SetWeaponPickupName(string name)
    {
        Debug.LogWarning("NAME: " + name);
        worldInfoHUD.Find("Player Interact").GetComponent<Text>().text = "Pickup " + name + ".";
    }

    public void ClearWeaponPickupName()
    {
        worldInfoHUD.Find("Player Interact").GetComponent<Text>().text = " ";
    }

    public void SetToPlayerView()
    {
        observerCamera.enabled = false;
        playerCamera.enabled = true;
        
        hudCanvas.worldCamera = playerCamera;

        worldInfoHUD.gameObject.SetActive(true); 
        playerInfoHUD.gameObject.SetActive(true);
        gunInfoHUD.gameObject.SetActive(true);
        centerHUD.gameObject.SetActive(true);
    }

    public void SetToObserverView()
    {
        playerCamera.enabled = false;
        observerCamera.enabled = true;

        hudCanvas.worldCamera = observerCamera;

        worldInfoHUD.gameObject.SetActive(false);
        playerInfoHUD.gameObject.SetActive(false);
        gunInfoHUD.gameObject.SetActive(false);
        centerHUD.gameObject.SetActive(false);
    }
}
