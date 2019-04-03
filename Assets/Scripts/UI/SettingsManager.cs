using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    private struct Settings
    {
        public string playerName;

    }

    private Settings currentSettings;
    private Settings changeSettings;
    
    // Use this for initialization
    void Start ()
    {
        currentSettings = new Settings();
        changeSettings = new Settings();
        currentSettings.playerName = "Player";
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void LoadPlayerSettings()
    {

    }

    public void UpdatePlayerSettings()
    {
        if(changeSettings.playerName != "")
        {
            Debug.Log("Update Name");
            currentSettings.playerName = changeSettings.playerName;
        }


        changeSettings = new Settings();
    }

    public void SavePlayerSettings()
    {

    }
    
    //-------------------------------

    public void UpdatePlayerName(string newName)
    {
        changeSettings.playerName = newName;
    }

    //-------------------------------

    public string GetPlayerName()
    {
        return currentSettings.playerName;
    }

}
