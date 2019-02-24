using UnityEngine;
using System.Collections;
using System;

public class SettingsState : State
{
    private MenuStates menuStates;

    public SettingsState(MenuStates menuStates)
    {
        this.menuStates = menuStates;
    }

    public void OpenSettings()
    {

    }

    public void OpenBrowser()
    {

    }

    public void OpenHost()
    {

    }

    public void SearchServers()
    {
        throw new NotImplementedException();
    }

    public void CompleteServerSearch()
    {
        throw new NotImplementedException();
    }

    public void EnterGame()
    {
        throw new NotImplementedException();
    }

    public void NeedsPassword()
    {
        throw new NotImplementedException();
    }

    public void CorrectPassword()
    {
        throw new NotImplementedException();
    }

    public void IncorrectPassword()
    {
        throw new NotImplementedException();
    }

    public void Disconnected()
    {
        throw new NotImplementedException();
    }

    public void OpenIngameMenu()
    {
        throw new NotImplementedException();
    }

    public void LeaveGame()
    {
        throw new NotImplementedException();
    }

    public void OpenIngameSettings()
    {
        throw new NotImplementedException();
    }
    
    public void Back()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("SettingsCanvas").GetComponent<Canvas>().enabled = false;

        menuStates.SetState(menuStates.GetMainMenuState());
    }

    public void HostGame()
    {
        throw new NotImplementedException();
    }

    public void EnterLobby()
    {
        throw new NotImplementedException();
    }

    public void BackToLobby()
    {
        throw new NotImplementedException();
    }
}
