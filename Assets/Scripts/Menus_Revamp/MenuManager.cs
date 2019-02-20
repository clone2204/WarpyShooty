using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    private MenuStates menuStates;
    private NetworkManager networkManager;

    private SettingsManager settingsManager;


    private Button findGameButton;
    private Button settingsButton;
    private Button quitGameButton;

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

    private InputField settingsPlayerNameField;
    private Button settingsBackButton;

    private InputField serverNameField;
    private InputField serverPasswordField;
    private Toggle serverPasswordToggle;
    private Button hostGameButton;
    private Button hostGameBackButton;

	// Use this for initialization
	void Start ()
    {
        networkManager = GetComponent<NetworkManager>();
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
        settingsBackButton = GameObject.Find("SettingsBackButton").GetComponent<Button>();

        serverNameField = GameObject.Find("ServerNameField").GetComponent<InputField>();
        serverPasswordField = GameObject.Find("ServerPasswordField").GetComponent<InputField>();
        serverPasswordToggle = GameObject.Find("ServerPasswordToggle").GetComponent<Toggle>();
        hostGameButton = GameObject.Find("HostGameButton").GetComponent<Button>();
        hostGameBackButton = GameObject.Find("HostGameBackButton").GetComponent<Button>();
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
        serverBrowserBackButton.onClick.AddListener(ServerBrowserBackButton);

        passwordInputField.onEndEdit.AddListener(PasswordEntryField);
        passwordConfirmButton.onClick.AddListener(PasswordConfirmButton);
        passwordCancelButton.onClick.AddListener(PasswordCancelButton);

        //serverSearchField.onEndEdit.AddListener();
        //serverSearchButton.onClick.AddListener();
        serverSearchBackButton.onClick.AddListener(ServerSearchBackButton);

        errorBackButton.onClick.AddListener(ErrorBackButton);

        //Settings Canvas
        settingsPlayerNameField.onEndEdit.AddListener(SettingsChangePlayerName);
        settingsBackButton.onClick.AddListener(SettingsBackButton);

        //HostGameCanvas
        serverNameField.onEndEdit.AddListener(ServerNameField);
        serverPasswordField.onEndEdit.AddListener(ServerPasswordField);
        serverPasswordToggle.onValueChanged.AddListener(ServerPasswordToggle);
        hostGameButton.onClick.AddListener(HostGameHostButton);
        hostGameBackButton.onClick.AddListener(HostGameBackButton);



        //GameObject.Find("").GetComponent<Button>().onClick.AddListener();

        //TODO
        //Add Rest of the UI buttons here
    }



    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
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
        menuStates.OpenHost();
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

    public void ServerBrowserBackButton()
    {
        menuStates.Back();
    }

    public void PasswordConfirmButton()
    {
        networkManager.SetPasswordEntered(true);
    }

    public void PasswordCancelButton()
    {
        menuStates.Back();
    }

    public void ServerSearchConfrimButton()
    {
        menuStates.SearchServers();
    }

    public void ServerSearchBackButton()
    {
        menuStates.Back();
    }

    public void PasswordJoinButton()
    {
        passwordInputField.text = "";
        networkManager.SetPasswordEntered(true);
    }

    public void PasswordBackButton()
    {
        menuStates.Back();
    }

    public void ErrorBackButton()
    {
        menuStates.Back();
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

    public void HostGameBackButton()
    {
        menuStates.Back();
    }

    public void SettingsChangePlayerName(string newName)
    {
        settingsManager.UpdatePlayerName(newName);
        menuStates.Back();
    }

    public void SettingsApplyButton()
    {
        settingsManager.UpdatePlayerSettings();
        menuStates.Back();
    }

    public void SettingsBackButton()
    {
        menuStates.Back();
    }

}
