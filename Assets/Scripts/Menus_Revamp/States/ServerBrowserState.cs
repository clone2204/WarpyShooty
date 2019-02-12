using UnityEngine;
using System.Collections;
using System;

public class ServerBrowserState : State
{

    private MenuStates menuStates;

    public ServerBrowserState(MenuStates menuStates)
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
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("HostGameCanvas").GetComponent<Canvas>().enabled = true;

        menuStates.SetState(menuStates.GetHostGameState());
    }

    public void SearchServers()
    {
        GameObject.Find("ServerSearch").transform.localPosition = new Vector3();
        menuStates.SetState(menuStates.GetSearchServerState());
    }

    public void CompleteServerSearch()
    {
        throw new NotImplementedException();
    }

    public void EnterGame()
    {
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("_SCRIPTS_").GetComponent<NetworkManager>().StopServerBrowser();
        menuStates.SetState(menuStates.GetIngameState());
    }

    public void NeedsPassword()
    {
        GameObject.Find("EnterPassword").transform.localPosition = new Vector3();
        menuStates.SetState(menuStates.GetEnterPasswordState());
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
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = false;

        GameObject.Find("_SCRIPTS_").GetComponent<NetworkManager>().StopServerBrowser();

        menuStates.SetState(menuStates.GetMainMenuState());
    }
}
