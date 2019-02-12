using UnityEngine;
using System.Collections;
using System;

public class HostGameState : State
{

    private MenuStates menuStates;

    public HostGameState(MenuStates menuStates)
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
        GameObject.Find("HostGameCanvas").GetComponent<Canvas>().enabled = false;

        GameObject.Find("_SCRIPTS_").GetComponent<NetworkManager>().StopServerBrowser();
        menuStates.SetState(menuStates.GetIngameState());
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
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("HostGameCanvas").GetComponent<Canvas>().enabled = false;

        menuStates.SetState(menuStates.GetBrowserState());
    }
}
