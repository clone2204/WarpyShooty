using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    private MenuStates menuStates;
    private NetworkManager networkManager;

	// Use this for initialization
	void Start ()
    {
        networkManager = GetComponent<NetworkManager>();
        menuStates = new MenuStates(this);
        networkManager.SetMenuStates(menuStates);

        SetUIActions();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SetUIActions()
    {
        Debug.Log("Setting UI Actions");

        //MainMenuCanvas
        //GameObject.Find("QuickPlayButton").GetComponent<Button>().onClick.AddListener();
        GameObject.Find("FindGameButton").GetComponent<Button>().onClick.AddListener(FindGameButton);
        GameObject.Find("SettingsButton").GetComponent<Button>().onClick.AddListener(SettingsButton);
        GameObject.Find("QuitGameButton").GetComponent<Button>().onClick.AddListener(QuitGameButton);

        //ServerBrowserCanvas
        GameObject.Find("HostServerButton").GetComponent<Button>().onClick.AddListener(HostServerButton);
        GameObject.Find("SearchServersButton").GetComponent<Button>().onClick.AddListener(SearchServersButton);
        //GameObject.Find("LANConnectButton").GetComponent<Button>().onClick.AddListener();
        GameObject.Find("JoinServerButton").GetComponent<Button>().onClick.AddListener(JoinServerButton);
        GameObject.Find("ServerBrowserBackButton").GetComponent<Button>().onClick.AddListener(ServerBrowserBackButton);

        GameObject.Find("PasswordEntryField").GetComponent<InputField>().onEndEdit.AddListener(PasswordEntryField);
        GameObject.Find("PasswordConfirmButton").GetComponent<Button>().onClick.AddListener(PasswordConfirmButton);
        GameObject.Find("PasswordCancelButton").GetComponent<Button>().onClick.AddListener(PasswordCancelButton);

        //GameObject.Find("ServerSearchField").GetComponent<Button>().onClick.AddListener();
        //GameObject.Find("ServerSearchConfrimButton").GetComponent<Button>().onClick.AddListener();
        GameObject.Find("ServerSearchBackButton").GetComponent<Button>().onClick.AddListener(ServerSearchBackButton);

        GameObject.Find("PasswordJoinButton").GetComponent<Button>().onClick.AddListener(PasswordJoinButton);
        GameObject.Find("PasswordBackButton").GetComponent<Button>().onClick.AddListener(PasswordBackButton);

        GameObject.Find("ErrorBackButton").GetComponent<Button>().onClick.AddListener(ErrorBackButton);
        
        //Settings Canvas
        GameObject.Find("SettingsBackButton").GetComponent<Button>().onClick.AddListener(SettingsBackButton);

        //HostGameCanvas
        GameObject.Find("HostGameHostButton").GetComponent<Button>().onClick.AddListener(HostGameHostButton);
        GameObject.Find("ServerNameField").GetComponent<InputField>().onEndEdit.AddListener(ServerNameField);
        GameObject.Find("ServerPasswordField").GetComponent<InputField>().onEndEdit.AddListener(ServerPasswordField);
        GameObject.Find("ServerPasswordToggle").GetComponent<Toggle>().onValueChanged.AddListener(ServerPasswordToggle);
        GameObject.Find("HostGameBackButton").GetComponent<Button>().onClick.AddListener(HostGameBackButton);



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
        GameObject.Find("PasswordEntryField").GetComponent<InputField>().text = "";
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

    public void SettingsBackButton()
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
        GameObject.Find("ServerPasswordField").GetComponent<InputField>().interactable = value;
        networkManager.SetRequirePassword(value);
    }

    public void HostGameBackButton()
    {
        menuStates.Back();
    }
}
