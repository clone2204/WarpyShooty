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
            currentSettings.playerName = changeSettings.playerName;
        }
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
