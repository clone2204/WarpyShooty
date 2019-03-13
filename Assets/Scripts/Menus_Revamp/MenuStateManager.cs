using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class State
{
    protected MenuStateManager menuStates;

    public State(MenuStateManager menuStates) { this.menuStates = menuStates; }
    public virtual void OpenSettings() { throw new NotImplementedException(); }
    public virtual void OpenBrowser() { throw new NotImplementedException(); }
    public virtual void HostGame() { throw new NotImplementedException(); }
    public virtual void EnterLobby() { throw new NotImplementedException(); }
    public virtual void SearchServers() { throw new NotImplementedException(); }
    public virtual void CompleteServerSearch() { throw new NotImplementedException(); }
    public virtual void EnterGame() { throw new NotImplementedException(); }
    public virtual void NeedsPassword() { throw new NotImplementedException(); }
    public virtual void Disconnected(string errorType, string error) { throw new NotImplementedException(); }
    public virtual void OpenIngameMenu() { throw new NotImplementedException(); }
    public virtual void OpenIngameSettings() { throw new NotImplementedException(); }
    public virtual void LeaveGame() { throw new NotImplementedException(); }
    public virtual void BackToLobby() { throw new NotImplementedException(); }
    public virtual void Back() { throw new NotImplementedException(); }
}

public class MenuStateManager
{
    private State currentState;

    // Use this for initialization
    public MenuStateManager()
    {
        currentState = new MainMenuState(this);
    }

    
    public void SetState(State state)
    {
        Debug.Log("New State: " + state.ToString());
        currentState = state;
    }

    public State GetState()
    {
        return currentState;
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

    public void Disconnected(string errorType, string error)
    {
        currentState.Disconnected(errorType, error);
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

class MainMenuState : State
{
    public MainMenuState(MenuStateManager menuStates) : base(menuStates) { }

    public override void OpenSettings()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("SettingsCanvas").GetComponent<Canvas>().enabled = true;

        menuStates.SetState(new SettingsState(menuStates));
    }

    public override void OpenBrowser()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = true;

        menuStates.SetState(new ServerBrowserState(menuStates));
    }
}

class ServerBrowserState : State
{
    public ServerBrowserState(MenuStateManager menuStates) : base(menuStates) { }

    public override void HostGame()
    {
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("HostLobbyCanvas").GetComponent<Canvas>().enabled = true;

        menuStates.SetState(new LobbyState(menuStates));
    }

    public override void EnterLobby()
    {
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("LobbyCanvas").GetComponent<Canvas>().enabled = true;

        menuStates.SetState(new LobbyState(menuStates));
    }

    public override void SearchServers()
    {
        GameObject.Find("ServerSearch").transform.localPosition = new Vector3();
        menuStates.SetState(new SearchServersState(menuStates));
    }

    public override void NeedsPassword()
    {
        GameObject.Find("EnterPassword").transform.localPosition = new Vector3();
        menuStates.SetState(new EnterPasswordState(menuStates));
    }

    public override void Disconnected(string errorType, string error)
    {
        GameObject.Find("ErrorCanvas").GetComponent<ErrorMessageManager>().SetErrorMessage(errorType, error);
        
        menuStates.SetState(new ErrorMessageState(menuStates));
    }

    public override void Back()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = false;

        menuStates.SetState(new MainMenuState(menuStates));
    }
}

class SearchServersState : State
{
    public SearchServersState(MenuStateManager menuStates) : base(menuStates) { }

    public override void SearchServers()
    {
        //Put Thing Here
    }

    public override void CompleteServerSearch()
    {
        GameObject.Find("ServerSearch").transform.localPosition = new Vector3(10000, 0);
        menuStates.SetState(new ServerBrowserState(menuStates));
    }

    public override void Back()
    {
        GameObject.Find("ServerSearch").transform.localPosition = new Vector3(10000, 0);
        menuStates.SetState(new ServerBrowserState(menuStates));
    }
}

class EnterPasswordState : State
{
    public EnterPasswordState(MenuStateManager menuStates) : base(menuStates) { }

    public override void Disconnected(string errorType, string error)
    {
        GameObject.Find("EnterPassword").transform.localPosition = new Vector3(10000, 0);

        GameObject.Find("ErrorCanvas").GetComponent<ErrorMessageManager>().SetErrorMessage(errorType, error);

        menuStates.SetState(new ErrorMessageState(menuStates));
    }

    public override void EnterLobby()
    {
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("LobbyCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("EnterPassword").transform.localPosition = new Vector3(10000, 0);

        menuStates.SetState(new LobbyState(menuStates));
    }

    public override void Back()
    {
        GameObject.Find("EnterPassword").transform.localPosition = new Vector3(10000, 0);
        menuStates.SetState(new ServerBrowserState(menuStates));
    }
}

class LobbyState : State
{
    public LobbyState(MenuStateManager menuStates) : base(menuStates) { }

    public override void EnterGame()
    {
        GameObject.Find("HostLobbyCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("LobbyCanvas").GetComponent<Canvas>().enabled = false;

        menuStates.SetState(new IngameState(menuStates));
    }

    public override void LeaveGame()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("HostLobbyCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("LobbyCanvas").GetComponent<Canvas>().enabled = false;

        menuStates.SetState(new MainMenuState(menuStates));
    }

    public override void Disconnected(string errorType, string error)
    {
        GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("HostLobbyCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("LobbyCanvas").GetComponent<Canvas>().enabled = false;

        GameObject.Find("ErrorCanvas").GetComponent<ErrorMessageManager>().SetErrorMessage(errorType, error);

        menuStates.SetState(new ErrorMessageState(menuStates));
    }

    public override void Back()
    {
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("HostLobbyCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("LobbyCanvas").GetComponent<Canvas>().enabled = false;

        menuStates.SetState(new ServerBrowserState(menuStates));
    }
}

class IngameState : State
{
    public IngameState(MenuStateManager menuStates) : base(menuStates) { }

    public override void Disconnected(string errorType, string error)
    {
        ServerBrowser serverBrowser = GameObject.Find("ServerBrowserCanvas").GetComponent<ServerBrowser>();
        serverBrowser.SetErrorMessage(errorType, error);
        GameObject.Find("ErrorMessageBox").transform.localPosition = new Vector3();

        menuStates.SetState(new ErrorMessageState(menuStates));
    }

    public override void LeaveGame()
    {
        menuStates.SetState(new MainMenuState(menuStates));
    }
}

class IngameMenuState : State
{
    public IngameMenuState(MenuStateManager menuStates) : base(menuStates) { }

}

class IngameSettingsState : State
{
    public IngameSettingsState(MenuStateManager menuStates) : base(menuStates) { }

}

class ErrorMessageState : State
{
    public ErrorMessageState(MenuStateManager menuStates) : base(menuStates) { }

    public override void Back()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = false;

        GameObject.Find("ErrorCanvas").GetComponent<ErrorMessageManager>().ClearError();

        menuStates.SetState(new MainMenuState(menuStates));
    }
}

class SettingsState : State
{
    public SettingsState(MenuStateManager menuStates) : base(menuStates) { }

    public override void Back()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("SettingsCanvas").GetComponent<Canvas>().enabled = false;

        menuStates.SetState(new MainMenuState(menuStates));
    }
}
