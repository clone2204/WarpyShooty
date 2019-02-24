﻿using UnityEngine;
using System.Collections;
using System;

public class ErrorMessageState : State
{
    private MenuStates menuStates;

    public ErrorMessageState(MenuStates menuStates)
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        GameObject.Find("ErrorMessageBox").transform.localPosition = new Vector3(10000, 0);
        menuStates.SetState(menuStates.GetBrowserState());
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
