using UnityEngine;
using System.Collections;
using System;

public class EnterPasswordState : State
{
    private MenuStates menuStates;

    public EnterPasswordState(MenuStates menuStates)
    {
        this.menuStates = menuStates;
    }

    public void CompleteServerSearch()
    {
        throw new NotImplementedException();
    }
    
    public void Disconnected()
    {
        GameObject.Find("EnterPassword").transform.localPosition = new Vector3(10000, 0);
        GameObject.Find("ErrorMessageBox").transform.localPosition = new Vector3();

        GameObject.Find("_SCRIPTS_").GetComponent<NetworkManager>().SetErrorMessage("Password", "Incorrect Password.");

        menuStates.SetState(menuStates.GetErrorMessageState());
    }

    public void EnterGame()
    {
        GameObject.Find("ServerBrowserCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("EnterPassword").transform.localPosition = new Vector3(10000, 0);
        menuStates.SetState(menuStates.GetIngameState());
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
        GameObject.Find("EnterPassword").transform.localPosition = new Vector3(10000, 0);
        menuStates.SetState(menuStates.GetBrowserState());
    }
}
