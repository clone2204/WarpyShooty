using UnityEngine;
using System.Collections;
using System;

public class SearchServersState : State
{
    private MenuStates menuStates;

    public SearchServersState(MenuStates menuStates)
    {
        this.menuStates = menuStates;
    }

    public void OpenBrowser()
    {
        throw new NotImplementedException();
    }

    public void OpenHost()
    {
        throw new NotImplementedException();
    }

    public void OpenSettings()
    {
        throw new NotImplementedException();
    }

    public void SearchServers()
    {
        //Put Thing Here
    }

    public void CompleteServerSearch()
    {
        GameObject.Find("ServerSearch").transform.localPosition = new Vector3(10000, 0);
        menuStates.SetState(menuStates.GetBrowserState());
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
        GameObject.Find("ServerSearch").transform.localPosition = new Vector3(10000, 0);
        menuStates.SetState(menuStates.GetBrowserState());
    }
}
