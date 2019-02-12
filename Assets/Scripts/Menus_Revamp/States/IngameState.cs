using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class IngameState : State
{
    private MenuStates menuStates;

    public IngameState(MenuStates menuStates)
    {
        this.menuStates = menuStates;
    }

    public void CompleteServerSearch()
    {
        throw new NotImplementedException();
    }

    public void CorrectPassword()
    {
        throw new NotImplementedException();
    }

    public void Disconnected()
    {
        menuStates.SetState(menuStates.GetErrorMessageState());
    }

    public void EnterGame()
    {
        throw new NotImplementedException();
    }

    public void IncorrectPassword()
    {
        throw new NotImplementedException();
    }

    public void LeaveGame()
    {
        menuStates.SetState(menuStates.GetMainMenuState());
    }

    public void NeedsPassword()
    {
        throw new NotImplementedException();
    }

    public void OpenBrowser()
    {
        throw new NotImplementedException();
    }

    public void OpenHost()
    {
        throw new NotImplementedException();
    }

    public void OpenIngameMenu()
    {
        throw new NotImplementedException();
    }

    public void OpenIngameSettings()
    {
        throw new NotImplementedException();
    }

    public void OpenSettings()
    {
        throw new NotImplementedException();
    }

    public void SearchServers()
    {
        throw new NotImplementedException();
    }

    public void Back()
    {

    }
}
