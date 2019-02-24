using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    private MenuStates menuStates;
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
    private Button lobbyReadyButton;
    private Button lobbyBackButton;

    //Host Lobby
    private InputField serverNameField;
    private InputField serverPasswordField;
    private Toggle serverPasswordToggle;
    private Button hostGameButton;
    private Button hostGameStartButton;
    private Button hostGameBackButton;

	// Use this for initialization
	void Start ()
    {
        networkManager = GetComponent<NetworkManager>();
        settingsManager = GetComponent<SettingsManager>();
        menuStates = new MenuStates(this);
        networkManager.SetMenuStates(menuStates);

        UIInit();
        SetUIActions();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void UIInit()
    {
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

        lobbyReadyButton = GameObject.Find("LobbyReadyButton").GetComponent<Button>();
        lobbyBackButton = GameObject.Find("LobbyBackButton").GetComponent<Button>();

        serverNameField = GameObject.Find("ServerNameField").GetComponent<InputField>();
        serverPasswordField = GameObject.Find("ServerPasswordField").GetComponent<InputField>();
        serverPasswordToggle = GameObject.Find("ServerPasswordToggle").GetComponent<Toggle>();
        hostGameButton = GameObject.Find("HostGameButton").GetComponent<Button>();
        hostGameStartButton = GameObject.Find("HostLobbyStartGameButton").GetComponent<Button>();
        hostGameBackButton = GameObject.Find("HostLobbyBackButton").GetComponent<Button>();
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
        serverBrowserBackButton.onClick.AddListener(Back);

        passwordInputField.onEndEdit.AddListener(PasswordEntryField);
        passwordConfirmButton.onClick.AddListener(PasswordConfirmButton);
        passwordCancelButton.onClick.AddListener(Back);

        //serverSearchField.onEndEdit.AddListener();
        //serverSearchButton.onClick.AddListener();
        serverSearchBackButton.onClick.AddListener(Back);

        errorBackButton.onClick.AddListener(Back);

        //Settings Canvas
        settingsPlayerNameField.onEndEdit.AddListener(SettingsChangePlayerName);
        settingsApplyButton.onClick.AddListener(SettingsApplyButton);
        settingsBackButton.onClick.AddListener(Back);

        //LobbyCanvas
        lobbyReadyButton.onClick.AddListener(LobbyReadyButton);
        lobbyBackButton.onClick.AddListener(Back);

        //HostLobbyCanvas
        serverNameField.onEndEdit.AddListener(ServerNameField);
        serverPasswordField.onEndEdit.AddListener(ServerPasswordField);
        serverPasswordToggle.onValueChanged.AddListener(ServerPasswordToggle);
        hostGameButton.onClick.AddListener(HostGameHostButton);
        hostGameStartButton.onClick.AddListener(HostLobbyStartButton);
        hostGameBackButton.onClick.AddListener(Back);



        //GameObject.Find("").GetComponent<Button>().onClick.AddListener();

        //TODO
        //Add Rest of the UI buttons here
    }



    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    public void Back()
    {
        menuStates.Back();
    }

    public void QuickPlayButton()
    {
        
    }

    public void FindGameButton()
    {
        menuStates.OpenBrowser();
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
        networkManager.JoinMatch();
    }

    public void PasswordEntryField(string password)
    {
        networkManager.SetJoinPassword(password);
    }
    
    public void PasswordConfirmButton()
    {
        networkManager.SetPasswordEntered(true);
    }
    
    public void ServerSearchConfrimButton()
    {
        menuStates.SearchServers();
    }
    
    public void PasswordJoinButton()
    {
        passwordInputField.text = "";
        networkManager.SetPasswordEntered(true);
    }
    
    public void HostGameHostButton()
    {
        networkManager.HostServer();
    }

    public void ServerNameField(string name)
    {
        networkManager.SetServerName(name);
    }

    public void ServerPasswordField(string password)
    {
        networkManager.SetServerPassword(password);
    }

    public void ServerPasswordToggle(bool value)
    {
        serverPasswordField.interactable = value;
        networkManager.SetRequirePassword(value);
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
    
    public void HostLobbyStartButton()
    {

    }

    public void LobbyReadyButton()
    {

    }
}
