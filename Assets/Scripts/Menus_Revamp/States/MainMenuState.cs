using UnityEngine;
using System.Collections;
using System;

public class MainMenuState : State
{
    private MenuStates menuStates;

    public MainMenuState(MenuStates menuStates)
    {
        this.menuStates = menuStates;
    }

    public void OpenSettings()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("SettingsCanvas").GetComponent<Canvas>().enabled = true;

        menuStates.SetState(menuStates.GetSettingState());
    }

    public void OpenBrowser()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = true;

        GameObject.Find("_SCRIPTS_").GetComponent<NetworkManager>().StartServerBrowser();

        menuStates.SetState(menuStates.GetBrowserState());
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

    }
}
