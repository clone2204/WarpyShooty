using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUDManager : MonoBehaviour {

    ArrayList textElements;

    Text gameTimer;

	// Use this for initialization
	void Start ()
    {
        textElements = new ArrayList();
        textElements.AddRange(this.GetComponentsInChildren<Text>());

        gameTimer = getTextElementByName("GameTimer");

        UpdatePlayerName(GetComponentInParent<PlayerInfoManager>().playerInfo.playerName);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public Text getTextElementByName(string name)
    {
        if(textElements == null)
        {
            textElements = new ArrayList();
            textElements.AddRange(this.GetComponentsInChildren<Text>());
        }

        foreach(Text element in textElements)
        {
            if (element.name == name)
            {
                return element;
            }
        }

        
        return null;
    }

    private void UpdatePlayerName(string name)
    {
        getTextElementByName("Player Name").text = name;
    }

    public void UpdateNewGunInfo(GameObject gun)
    {
        getTextElementByName("Gun Name").text = gun.GetComponent<GunBase>().gunName;
        getTextElementByName("Ammo Count").text = gun.GetComponent<GunBase>().ammoData.currentAmmoCount + " / " + gun.GetComponent<GunBase>().ammoData.currentAmmoPoolCount;
    }

    public void UpdatePlayerHealthHUD(int health)
    {
        getTextElementByName("Player Health").text = "Health: " + health;
        Debug.Log("HUD UPDATED");
    }

    public void UpdateGameTimer(int gameTime)
    {
        gameTimer.text = "" + gameTime;
    }

}
