using UnityEngine;
using System.Collections;

public interface State
{
    void OpenSettings();

    void OpenBrowser();

    void OpenHost();

    void SearchServers();

    void CompleteServerSearch();

    void EnterGame();

    void NeedsPassword();

    void Disconnected();

    void OpenIngameMenu();

    void LeaveGame();

    void OpenIngameSettings();

    void Back();
}
