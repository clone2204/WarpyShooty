using UnityEngine.Networking;

public interface IPlayerManager
{
    void Init(IPlayerManager playerInfoManager);
    
    void SetName(string name);

    string GetName();

    void SetTeam(GameManager.Team team);

    GameManager.Team GetTeam();

    void SetPlayerObjectID(NetworkInstanceId playerObjectID);

    NetworkInstanceId GetPlayerObjectID();

    void SetPlayerConnection(NetworkConnection networkConnection);

    NetworkConnection GetPlayerConnection();

    void SpawnPlayer();
}
