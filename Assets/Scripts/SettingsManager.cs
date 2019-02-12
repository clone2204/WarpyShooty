using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsManager : MonoBehaviour {

    private string playerName;
    public string spraySource;
    private WWW path;

    public Texture playerSpray;



    // Use this for initialization
    void Start ()
    {
        if(PlayerPrefs.HasKey("PlayerName"))
        {
            playerName = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", "Player");
        }

        if(PlayerPrefs.HasKey("SpraySource"))
        {
            spraySource = PlayerPrefs.GetString("SpraySource");
            path = new WWW(spraySource);            
        }
        else
        {
            PlayerPrefs.SetString("SpraySource", "");
        }
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        CreateSprayWhenImageDownloaded();
    }

    public void UpdatePlayerName()
    {
        string newName = GameObject.Find("NameField").GetComponent<InputField>().text;

        PlayerPrefs.SetString("PlayerName", newName);
        PlayerPrefs.Save();
    }

    public void UpdateSpraySource()
    {
        string source = GameObject.Find("SprayField").GetComponent<InputField>().text;
        path = new WWW(source);

        PlayerPrefs.SetString("SpraySource", source);
        PlayerPrefs.Save();        
    }

    private void CreateSprayWhenImageDownloaded()
    {
        if (playerSpray == null)
            return;

        if (path != null && path.isDone)
        {
            Debug.LogWarning("DING");
            RawImage preview = GameObject.Find("SprayPreview").GetComponent<RawImage>();
            preview.texture = path.texture;

            this.playerSpray = path.texture;
        }
    }

}
