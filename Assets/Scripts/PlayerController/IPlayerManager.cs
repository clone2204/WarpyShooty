using UnityEngine;
using UnityEngine.Networking;

public interface IPlayerManager
{
    void Init();
    
    void SetName(string name);

    string GetName();

    void SetTeam(GameManager.Team team);

    GameManager.Team GetTeam();

    void SetPlayerObjectID(NetworkInstanceId playerObjectID);

    NetworkInstanceId GetPlayerObjectID();

    void SetPlayerConnection(NetworkConnection networkConnection);

    NetworkConnection GetPlayerConnection();

    void SetPlayerObject(GameObject playerObject);

    GameObject GetPlayerObject();

    void SpawnPlayer(Vector3 location);
}
