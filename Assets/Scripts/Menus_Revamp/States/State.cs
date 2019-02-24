using UnityEngine;
using System.Collections;

public interface State
{
    void OpenSettings();

    void OpenBrowser();

    void HostGame();

    void EnterLobby();

    void SearchServers();

    void CompleteServerSearch();

    void EnterGame();

    void NeedsPassword();

    void Disconnected();

    void OpenIngameMenu();

    void OpenIngameSettings();

    void LeaveGame();

    void BackToLobby();

    void Back();
}
