using UnityEngine;
using System.Collections;
using System;

public class LobbyState : State
{
    private MenuStates menuStates;

    public LobbyState(MenuStates menuStates)
    {
        this.menuStates = menuStates;
    }

    public void OpenSettings()
    {

    }

    public void OpenBrowser()
    {

    }

    public void HostGame()
    {

    }

    public void EnterLobby()
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
        GameObject.Find("HostLobbyCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("LobbyCanvas").GetComponent<Canvas>().enabled = false;

        GameObject.Find("_SCRIPTS_").GetComponent<NetworkManager>().StopServerBrowser();
        menuStates.SetState(menuStates.GetIngameState());
    }

    public void BackToLobby()
    {

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
        menuStates.SetState(menuStates.GetMainMenuState());
    }

    public void OpenIngameSettings()
    {
        throw new NotImplementedException();
    }

    public void Back()
    {
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("HostLobbyCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("LobbyCanvas").GetComponent<Canvas>().enabled = false;


        menuStates.SetState(menuStates.GetBrowserState());
    }
}
