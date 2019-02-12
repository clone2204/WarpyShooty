using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;



public class LobbyServerCommands : NetworkBehaviour {

    private LobbyManager lobbyManager;
    private LobbyUIManager lobbyUIManager;
    
    
	// Use this for initialization
	void Start ()
    {
        lobbyManager = GetComponentInParent<LobbyManager>();   
    }

    public void StartGame()
    {

        //networkLobbyManager.CheckReadyToBegin();
        //RpcDecoupleLobbyPlayersFromLists();
    }

    [ClientRpc]
    public void RpcUpdatePlayerLists(Dictionary<NetworkConnection, PlayerInfoManager.PlayerInfo> connectedPlayers)
    {
        if(lobbyUIManager == null)
            lobbyUIManager = GameObject.Find("InfoBoard").GetComponent<LobbyUIManager>();

        //lobbyUIManager.UpdatePlayerLists(connectedPlayers);
    }
    
}
