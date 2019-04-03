using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class LobbyUIManager : MonoBehaviour
{

    ILobbyManager lobbyManager;
    Canvas lobbyCanvas;

    private bool canEditSettings;

    // Use this for initialization
    void Start ()
    {
        Debug.Log("LOBBYUIMANAGER INIT");
        GameObject scriptObject = GameObject.Find("_SCRIPTS_");
        //lobbyManager = scriptObject.GetComponent<LobbyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e") && canEditSettings)
        {
           UpdateSettings();
        }
    }

    public void SetLobbyName(string name)
    {
        //lobbyManager.SetLobbyName(name);
    }

    public void SetLobbyPassword(string password)
    {

    }

    public void Back()
    {
        ToggleSettingsUI(false);
    }

    public void LaunchGame()
    {

    }

    public void OpenLobby()
    {
        
        //lobbyManager.OpenLobby();
    }

    public void UpdateSettings()
    {
        ToggleSettingsUI(true);
    }

    private void ToggleSettingsUI(bool currentlyEditing)
    {
        GameObject player = GameObject.FindGameObjectWithTag("localPlayer");
        player.GetComponent<LocalPlayerController>().enabled = !currentlyEditing;
        player.transform.Find("PlayerHud").GetComponent<Canvas>().enabled = !currentlyEditing;
        player.transform.Find("PlayerPOV").GetComponent<BlurOptimized>().enabled = currentlyEditing;

        GameObject.Find("InfoBoard").transform.Find("LobbyEditUICanvas").GetComponent<Canvas>().enabled = currentlyEditing;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name != "LobbyPlayer(Clone)")
            return;

       if (col.GetComponent<NetworkIdentity>().isServer)
            canEditSettings = true;

    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name != "LobbyPlayer(Clone)")
            return;

        ;

        if (col.GetComponent<NetworkIdentity>().isServer)
            canEditSettings = false;

    }

    /*
    public void UpdatePlayerLists(Dictionary<NetworkConnection, PlayerInfoManager.PlayerInfo> connectedPlayers)
    {
        //Dictionary<NetworkConnection, PlayerInfoManager.PlayerInfo> connectedPlayers = lobbyManager.connectedPlayers;

        Debug.LogWarning("UPDATE LIST");
        foreach (GameObject playerList in GameObject.FindGameObjectsWithTag("PlayerList"))
        {
            int playerCount = 0;
            foreach (PlayerInfoManager.PlayerInfo playerInfo in connectedPlayers.Values)
            {
                if (playerInfo.playerTeam == LobbyManager.PlayerTeam.Blue)
                {
                    GameObject blueList = playerList.transform.FindChild("BlueTeamList").gameObject;
                    GameObject namePanel = blueList.transform.FindChild("Blue" + playerCount % 6).gameObject;
                    namePanel.GetComponentInChildren<Text>().text = playerInfo.playerName;

                }
                else if (playerInfo.playerTeam == LobbyManager.PlayerTeam.Red)
                {
                    GameObject redList = playerList.transform.FindChild("RedTeamList").gameObject;
                    GameObject namePanel = redList.transform.FindChild("Red" + playerCount % 6).gameObject;
                    namePanel.GetComponentInChildren<Text>().text = playerInfo.playerName;
                }

            }
        }
    }
    
    /*
    [ClientRpc]
    private void RpcDecoupleLobbyPlayersFromLists()
    {
        List<GameObject> lobbyPlayers = new List<GameObject>();
        lobbyPlayers.AddRange(GameObject.FindGameObjectsWithTag("LobbyPlayer"));
        lobbyPlayers.AddRange(GameObject.FindGameObjectsWithTag("localLobbyPlayer"));

        foreach (GameObject lobbyPlayer in lobbyPlayers)
        {
            lobbyPlayer.transform.SetParent(null);
        }
    }


    //----------------------------------------------------------------------------------------------------------------------------------------------------
    //Takes in nothing
    //Runs through all current lobby players on the server, determines their team, and re-orders them in their team lists on the lobby canvas
    //Lastly commands all clients to update their lobbyPlayers one player at a time (in each iteration of the loop)
    
    [Command]
    public void CmdUpdateLobbyPlayerListPanels()
    {
        int redOffset = 0;
        int blueOffset = 0;
        int neutralOffset = 0;

        foreach (GameObject playerList in GameObject.FindGameObjectsWithTag("PlayerList"))
        {
            Dictionary<NetworkConnection, PlayerInfoManager.PlayerInfo> connectedPlayers = matchManager.GetConnectedPlayersList();

            int playerCount = 0;
            foreach (PlayerInfoManager.PlayerInfo playerInfo in connectedPlayers.Values)
            {
                
                if (playerInfo.playerTeam == MatchManager.PlayerTeam.Blue)
                {
                    GameObject namePanel = GameObject.Find("Blue" + playerCount % 6);
                    namePanel.GetComponent<Text>().text = playerInfo.playerName;
                   
                }
                else if(playerInfo.playerTeam == MatchManager.PlayerTeam.Red)
                {
                    GameObject namePanel = GameObject.Find("Red" + playerCount % 6);
                    namePanel.GetComponent<Text>().text = playerInfo.playerName;
                }

            }

            //RpcAddPlayerToLobbyList(lobbyPlayer.GetComponent<NetworkIdentity>().netId, nameListPanel.name, yOffset, lobbyPlayer.GetComponent<LobbyPlayerManager>().playerInfo);
        }

    }

    [ClientRpc]
    public void RpcAddPlayerToLobbyList(NetworkInstanceId lobbyPlayerID, string parentPanel, int parentOffset, PlayerInfoManager.PlayerInfo playerInfo)
    {
        
        GameObject lobbyPlayer = ClientScene.FindLocalObject(lobbyPlayerID);

        MainMenu mainMenuScript = GameObject.Find("Menues").GetComponent<MainMenu>();
        Canvas currentMenu = mainMenuScript.GetActiveCanvas();
        Transform nameListPanel = currentMenu.transform.FindChild(parentPanel);

        lobbyPlayer.GetComponentInChildren<Text>().text = playerInfo.playerName;
        lobbyPlayer.transform.SetParent(nameListPanel);
        lobbyPlayer.transform.localPosition = new Vector3(0, parentOffset, 0);

        lobbyPlayer.GetComponent<LobbyPlayerManager>().playerInfo = playerInfo;
    }
    */
    //----------------------------------------------------------------------------------------------------------------------------------------------------

}
