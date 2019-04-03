using UnityEngine;
using UnityEngine.Networking;

public interface ILobbyPlayerManager
{
    void Init();
    
    void SetName(string name);

    string GetName();

    void SetPlayerObjectID(NetworkInstanceId playerObjectID);

    NetworkInstanceId GetPlayerObjectID();

    void SetPlayerConnection(NetworkConnection networkConnection);

    NetworkConnection GetPlayerConnection();
}
