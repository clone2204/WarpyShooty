using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfoManager : NetworkBehaviour
{
    private LobbyManager lobbyManager;

    private PlayerHUDManager hudManager;
    private GunManager gunManager;
    
    private PlayerServerCommands playerCommands;
    
    public struct PlayerInfo
    {
        public string playerName;
        //public LobbyManager.PlayerTeam playerTeam;

        public PlayerInfo(string playerName )
        {
            this.playerName = playerName;
            //this.playerTeam = playerTeam;
        }
    }

    public PlayerInfo playerInfo;
    public int playerFullHealth;
    
    // Use this for initialization
    void Start ()
    {
        GameObject scriptObject = GameObject.Find("_SCRIPTS_");
        lobbyManager = scriptObject.GetComponentInChildren<LobbyManager>();
        playerCommands = GetComponent<PlayerServerCommands>();

        hudManager = transform.FindChild("PlayerHud").gameObject.GetComponent<PlayerHUDManager>();

        if(isServer)
        {
            //playerInfo = matchManager.GetPlayerInfo(connectionToClient);
            //TargetSendClientPlayerInfo(connectionToClient, playerInfo);
            //InitPlayerInfoHUD();
        }
        
        
    }

    [TargetRpc]
    private void TargetSendClientPlayerInfo(NetworkConnection connection, PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        InitPlayerInfoHUD();
    }

    public void InitPlayerInfoHUD()
    {
        hudManager.UpdatePlayerHealthHUD(playerFullHealth);
        //hudManager.getTextElementByName("Player Name").text = playerInfo.playerName + "  ||  " + playerInfo.playerTeam;
    }

    public int getPlayerFullHealth()
    {
        return playerFullHealth;
    }

    
}
