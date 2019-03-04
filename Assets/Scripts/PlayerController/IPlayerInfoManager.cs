using UnityEngine.Networking;

public interface IPlayerInfoManager
{
    void Init(IPlayerInfoManager playerInfoManager);

    void SetPlayerID(int ID);

    int GetPlayerID();

    void SetName(string name);

    string GetName();

    void SetPlayerObjectID(NetworkInstanceId playerObjectID);

    NetworkInstanceId GetPlayerObjectID();

    void SetPlayerConnection(NetworkConnection networkConnection);

    NetworkConnection GetPlayerConnection();

}
