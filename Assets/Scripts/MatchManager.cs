using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MatchManager : MonoBehaviour
{

    LobbyManager lobbyManager;
    PlayerHUDManager hudManager;
    
   
    
    
    
	// Use this for initialization
	void Start ()
    {
        lobbyManager = GetComponent<LobbyManager>();

        
	}
	
    void Awake()
    {
        Start();
    }

    public void FixedUpdate()
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //SERVER FUNCTIONS
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------------------------------
    
    
   

    
    
    
}
