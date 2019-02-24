using UnityEngine;
using System.Collections;

public class MenuStates
{
   private State currentState;

    private State mainMenuState;
    private State settingsState;
    private State browserState;
    private State lobbyState;
    private State searchServersState;
    private State enterPasswordState;
    private State ingameState;
    private State errorMessageState;
    private State ingameMenuState;
    private State ingameSettingsState;


    // Use this for initialization
    public MenuStates(MenuManager menuManager)
    {
        mainMenuState = new MainMenuState(this);
        settingsState = new SettingsState(this);
        browserState = new ServerBrowserState(this);
        lobbyState = new LobbyState(this);
        searchServersState = new SearchServersState(this);
        enterPasswordState = new EnterPasswordState(this);
        ingameState = new IngameState(this);
        errorMessageState = new ErrorMessageState(this);
        ingameMenuState = new IngameMenuState(this);
        ingameSettingsState = new IngameSettingsState(this);

        currentState = mainMenuState;

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void SetState(State state)
    {
        Debug.Log("New State: " + state.ToString());
        currentState = state;
    }

    public State GetMainMenuState()
    {
        return mainMenuState;
    }

    public State GetSettingState()
    {
        return settingsState;
    }

    public State GetBrowserState()
    {
        return browserState;
    }

    public State GetLobbyState()
    {
        return lobbyState;
    }

    public State GetSearchServerState()
    {
        return searchServersState;
    }

    public State GetEnterPasswordState()
    {
        return enterPasswordState;
    }

    public State GetIngameState()
    {
        return ingameState;
    }

    public State GetErrorMessageState()
    {
        return errorMessageState;
    }

    public State GetIngameMenuState()
    {
        return ingameMenuState;
    }

    public State GetIngameSettingsState()
    {
        return ingameSettingsState;
    }

    //----------------------------------------------------------------

    public void OpenSettings()
    {
        currentState.OpenSettings();
    }

    public void OpenBrowser()
    {
        currentState.OpenBrowser();
    }

    public void HostGame()
    {
        currentState.HostGame();
    }

    public void EnterLobby()
    {
        currentState.EnterLobby();
    }

    public void BackToLobby()
    {
        currentState.BackToLobby();
    }

    public void SearchServers()
    {
        currentState.SearchServers();
    }

    public void CompleteServerSearch()
    {
        currentState.CompleteServerSearch();
    }

    public void EnterGame()
    {
        currentState.EnterGame();
    }

    public void NeedsPassword()
    {
        currentState.NeedsPassword();
    }

    public void Disconnected()
    {
        currentState.Disconnected();
    }

    public void OpenIngameMenu()
    {
        currentState.OpenIngameMenu();
    }

    public void LeaveGame()
    {
        currentState.LeaveGame();
    }

    public void OpenIngameSettings()
    {
        currentState.OpenIngameSettings();
    }

    public void Back()
    {
        currentState.Back();
    }
}
