using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class LobbyPlayerManager : NetworkBehaviour
{
    private LobbyManager lobbyManager;
    private NetworkLobbyPlayer lobbyPlayer;

    private LobbyServerCommands lobbyCommands;
    private PlayerServerCommands playerCommands;

    public PlayerInfoManager.PlayerInfo playerInfo;


    // Use this for initialization
    void Start ()
    {
        lobbyPlayer = GetComponent<NetworkLobbyPlayer>();

        if (!isLocalPlayer)
            return;

        GameObject scriptObject = GameObject.Find("_SCRIPTS_");
        lobbyManager = scriptObject.GetComponent<LobbyManager>();
        
        
        lobbyCommands = scriptObject.GetComponentInChildren<LobbyServerCommands>();
        playerCommands = GetComponent<PlayerServerCommands>();

        Debug.LogWarning("DING 1");
        

        string tempName = "" + Random.Range(0, 100);
        tag = "localLobbyPlayer";
        
        playerInfo.playerName = tempName;
        
        //playerCommands.CmdAddPlayerToMatch(GetComponent<NetworkIdentity>().netId, tempName);
    }
    
    
    
    public void ToggleReadyState()
    {
        //playerCommands.CmdTogglePlayerReady(GetComponent<NetworkIdentity>().netId);
    }


}
