using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    private MenuStateManager menuStates;
    private NetworkManager networkManager;

    private SettingsManager settingsManager;

    //Main Menu
    private Button findGameButton;
    private Button settingsButton;
    private Button quitGameButton;

    //Server Browser
    private Button hostServerButton;
    private Button searchServerButton;
    private Button joinServerButton;
    private Button serverBrowserBackButton;

    private InputField passwordInputField;
    private Button passwordConfirmButton;
    private Button passwordCancelButton;

    private InputField serverSearchField;
    private Button serverSearchButton;
    private Button serverSearchBackButton;

    private Button errorBackButton;

    //Settings
    private InputField settingsPlayerNameField;
    private Button settingsApplyButton;
    private Button settingsBackButton;

    //Lobby
    private Button lobbyBackButton;

    //Host Lobby
    private InputField serverNameField;
    private InputField serverPasswordField;
    private Toggle serverPasswordToggle;
    private Button hostGameButton;
    private Dropdown hostGameTimeDropdown;
    private Dropdown hostGameKillDropdown;
    private Button hostGameStartButton;
    private Button hostGameBackButton;
    private Button hostGameSwapButton;
    private Button hostGameKickButton;
    private Button hostGameBanButton;
    private Button hostGameConfirmButton;
    private Button hostGameCancelButton;

    // Use this for initialization
    void Start ()
    {
        networkManager = GetComponent<NetworkManager>();
        settingsManager = GetComponent<SettingsManager>();
        menuStates = new MenuStateManager();
        networkManager.SetMenuStates(menuStates);

        UIInit();
        SetUIActions();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        if (scene.name == "TitleScreen")
        {
            UIInit();
            SetUIActions();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            Debug.Log("State: " + menuStates.GetState().ToString());
        }
    }


    public void UIInit()
    {
        Debug.Log("Finding UI Objects");

        findGameButton = GameObject.Find("FindGameButton").GetComponent<Button>();
        settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        quitGameButton = GameObject.Find("QuitGameButton").GetComponent<Button>();

        hostServerButton = GameObject.Find("HostServerButton").GetComponent<Button>();
        searchServerButton = GameObject.Find("SearchServersButton").GetComponent<Button>();
        joinServerButton = GameObject.Find("JoinServerButton").GetComponent<Button>();
        serverBrowserBackButton = GameObject.Find("ServerBrowserBackButton").GetComponent<Button>();

        passwordInputField = GameObject.Find("PasswordEntryField").GetComponent<InputField>();
        passwordConfirmButton = GameObject.Find("PasswordConfirmButton").GetComponent<Button>();
        passwordCancelButton = GameObject.Find("PasswordCancelButton").GetComponent<Button>();

        serverSearchField = GameObject.Find("ServerSearchField").GetComponent<InputField>();
        serverSearchButton = GameObject.Find("ServerSearchConfirmButton").GetComponent<Button>();
        serverSearchBackButton = GameObject.Find("ServerSearchBackButton").GetComponent<Button>();

        errorBackButton = GameObject.Find("ErrorBackButton").GetComponent<Button>();

        settingsPlayerNameField = GameObject.Find("SettingsPlayerNameField").GetComponent<InputField>();
        settingsApplyButton = GameObject.Find("SettingsApplyButton").GetComponent<Button>();
        settingsBackButton = GameObject.Find("SettingsBackButton").GetComponent<Button>();

        lobbyBackButton = GameObject.Find("LobbyBackButton").GetComponent<Button>();

        serverNameField = GameObject.Find("ServerNameField").GetComponent<InputField>();
        serverPasswordField = GameObject.Find("ServerPasswordField").GetComponent<InputField>();
        serverPasswordToggle = GameObject.Find("ServerPasswordToggle").GetComponent<Toggle>();
        hostGameButton = GameObject.Find("HostGameButton").GetComponent<Button>();
        hostGameTimeDropdown = GameObject.Find("HostGameTimeDropdown").GetComponent<Dropdown>();
        hostGameKillDropdown = GameObject.Find("HostGameKillDropdown").GetComponent<Dropdown>();
        hostGameStartButton = GameObject.Find("StartGameButton").GetComponent<Button>();
        hostGameBackButton = GameObject.Find("HostLobbyBackButton").GetComponent<Button>();

        hostGameSwapButton = GameObject.Find("HostLobbySwapButton").GetComponent<Button>();
        hostGameKickButton = GameObject.Find("HostLobbyKickButton").GetComponent<Button>();
        hostGameBanButton = GameObject.Find("HostLobbyBanButton").GetComponent<Button>();
        hostGameConfirmButton = GameObject.Find("HostLobbyConfirmButton").GetComponent<Button>();
        hostGameCancelButton = GameObject.Find("HostLobbyCancelButton").GetComponent<Button>();
}

    public void SetUIActions()
    {
        Debug.Log("Setting UI Actions");

        //MainMenuCanvas
        //GameObject.Find("QuickPlayButton").GetComponent<Button>().onClick.AddListener();
        findGameButton.onClick.AddListener(FindGameButton);
        settingsButton.onClick.AddListener(SettingsButton);
        quitGameButton.onClick.AddListener(QuitGameButton);

        //ServerBrowserCanvas
        hostServerButton.onClick.AddListener(HostServerButton);
        searchServerButton.onClick.AddListener(SearchServersButton);
        //GameObject.Find("LANConnectButton").GetComponent<Button>().onClick.AddListener();
        joinServerButton.onClick.AddListener(JoinServerButton);
        serverBrowserBackButton.onClick.AddListener(ServerBrowserBack);

        //passwordInputField.onEndEdit.AddListener(PasswordEntryField);
        passwordConfirmButton.onClick.AddListener(PasswordConfirmButton);
        passwordCancelButton.onClick.AddListener(PasswordCancelButton);

        //serverSearchField.onEndEdit.AddListener();
        //serverSearchButton.onClick.AddListener();
        serverSearchBackButton.onClick.AddListener(Back);

        errorBackButton.onClick.AddListener(Back);

        //Settings Canvas
        settingsPlayerNameField.onEndEdit.AddListener(SettingsChangePlayerName);
        settingsApplyButton.onClick.AddListener(SettingsApplyButton);
        settingsBackButton.onClick.AddListener(Back);

        //LobbyCanvas
        lobbyBackButton.onClick.AddListener(LobbyBack);

        //HostLobbyCanvas
        //serverNameField.onEndEdit.AddListener(ServerNameField);
        //serverPasswordField.onEndEdit.AddListener(ServerPasswordField);
        serverPasswordToggle.onValueChanged.AddListener(ServerPasswordToggle);
        hostGameButton.onClick.AddListener(HostGameHostButton);

        hostGameTimeDropdown.onValueChanged.AddListener(ChangeGameSettings);
        hostGameKillDropdown.onValueChanged.AddListener(ChangeGameSettings);
        hostGameStartButton.onClick.AddListener(HostLobbyStartButton);
        hostGameBackButton.onClick.AddListener(LobbyHostBack);

        hostGameSwapButton.onClick.AddListener(StartSwap);
        hostGameKickButton.onClick.AddListener(StartKick);
        hostGameBanButton.onClick.AddListener(StartBan);
        hostGameConfirmButton.onClick.AddListener(ConfirmLobbyHostAction);
        hostGameCancelButton.onClick.AddListener(CancelLobbyHostAction);
    }



    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    public void Back()
    {
        menuStates.Back();
    }

    public void ServerBrowserBack()
    {
        menuStates.Back();
        networkManager.StopServerBrowser();
    }

    public void LobbyHostBack()
    {
        networkManager.LeaveHostLobby();
    }

    public void LobbyBack()
    {
        networkManager.LeaveLobby();
    }

    public void QuickPlayButton()
    {
        
    }

    public void FindGameButton()
    {
        menuStates.OpenBrowser();
        networkManager.StartServerBrowser();
    }

    public void SettingsButton()
    {
        menuStates.OpenSettings();
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void HostServerButton()
    {
        menuStates.HostGame();
    }

    public void SearchServersButton()
    {
        menuStates.SearchServers();
    }

    public void LANConnectButton()
    {
        
    }

    public void JoinServerButton()
    {
        networkManager.JoinLobby();
    }
    
    public void PasswordConfirmButton()
    {
        string password = passwordInputField.text;
        passwordInputField.text = "";

        networkManager.EnterPassword(password);
    }

    public void PasswordCancelButton()
    {
        passwordInputField.text = "";

        menuStates.Back();
    }
    
    public void ServerSearchConfrimButton()
    {
        menuStates.SearchServers();
    }
    
    public void HostGameHostButton()
    {
        string serverName = serverNameField.text;
        string serverPassword = "";

        if (serverPasswordToggle.isOn)
            serverPassword = serverPasswordField.text;

        serverNameField.interactable = false;
        serverPasswordToggle.interactable = false;
        serverPasswordField.interactable = false;
        hostGameButton.interactable = false;

        hostGameTimeDropdown.interactable = true;
        hostGameKillDropdown.interactable = true;
        hostGameStartButton.interactable = true;
        
        networkManager.HostServer(serverName, serverPassword);
    }

    public void ServerPasswordToggle(bool value)
    {
        serverPasswordField.interactable = value;
    }
    
    public void SettingsChangePlayerName(string newName)
    {
        settingsManager.UpdatePlayerName(newName);
    }

    public void SettingsApplyButton()
    {
        settingsManager.UpdatePlayerSettings();
        menuStates.Back();
    }

    public void ChangeGameSettings(int newValue)
    {
        int timeLimit = 10 + (5 * hostGameTimeDropdown.value);
        int killLimit = 20 + (15 * hostGameKillDropdown.value);

        networkManager.ChangeGameSettings(timeLimit, killLimit);
    }
    
    public void HostLobbyStartButton()
    {
        networkManager.StartGame();
    }

    public void StartSwap()
    {
        SetPlayerListTogglesActive(true);
        SetHostActionsActive(false);

        networkManager.StartSwap(GetSelectedPlayers);
    }

    public void StartKick()
    {
        SetPlayerListTogglesActive(true);
        SetHostActionsActive(false);

        networkManager.StartKick(GetSelectedPlayers);
    }

    public void StartBan()
    {
        SetPlayerListTogglesActive(true);
        SetHostActionsActive(false);

        networkManager.StartBan(GetSelectedPlayers);
    }

    public void ConfirmLobbyHostAction()
    {
        SetPlayerListTogglesActive(false);
        SetHostActionsActive(true);

        networkManager.ConfirmHostAction();
    }

    public void CancelLobbyHostAction()
    {
        SetPlayerListTogglesActive(false);
        SetHostActionsActive(true);

        networkManager.CancelHostAction();
    }
    
    private void SetHostActionsActive(bool active)
    {
        hostGameConfirmButton.interactable = !active;
        hostGameCancelButton.interactable = !active;

        hostGameSwapButton.interactable = active;
        hostGameKickButton.interactable = active;
        hostGameBanButton.interactable = active;
    }

    private void SetPlayerListTogglesActive(bool active)
    {
        Transform playerList = GameObject.Find("HostLobbyCanvas").transform.Find("PlayerList");

        foreach(Toggle player in playerList.GetComponentsInChildren<Toggle>())
        {
            player.interactable = active;
            player.transform.Find("Background").GetComponent<Image>().enabled = active;

            //player.isOn = false;
        }
    }

    private List<int> GetSelectedPlayers()
    {
        Transform playerList = GameObject.Find("HostLobbyCanvas").transform.Find("PlayerList");
        List<int> selectedPlayers = new List<int>();

        foreach (Toggle player in playerList.GetComponentsInChildren<Toggle>())
        {
            if (player.isOn)
            {
                selectedPlayers.Add(int.Parse(player.transform.name.Substring(6)));

                player.isOn = false;
            }
        }

        return selectedPlayers;
    }
}
