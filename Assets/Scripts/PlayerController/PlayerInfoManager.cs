using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfoManager : NetworkBehaviour, IPlayerInfoManager
{
    private IPlayerInfoManager realInfoManager;

    public struct PlayerInfo
    {
        
    }

    void Start()
    {
        Debug.LogWarning("Player INIT!");

        if (isServer)
        {
            realInfoManager = gameObject.GetComponent<PlayerInfoManager_Server>();
            IPlayerInfoManager client = gameObject.GetComponent<PlayerInfoManager_Client>();

            realInfoManager.Init(client);
            client.Init(realInfoManager);
        }
        else
        {
            realInfoManager = gameObject.GetComponent<PlayerInfoManager_Client>();
            IPlayerInfoManager server = gameObject.GetComponent<PlayerInfoManager_Server>();

            realInfoManager.Init(server);
            server.Init(realInfoManager);
        }

        if(isLocalPlayer)
        {
            gameObject.tag = "localPlayer";

            string name = GameObject.Find("_SCRIPTS_").GetComponent<SettingsManager>().GetPlayerName();
            SetName(name);

            
        }
    }

    public void Init(IPlayerInfoManager playerInfoManager)
    {
        Start();
    }

    public void SetName(string name)
    {
        realInfoManager.SetName(name);
    }

    public string GetName()
    {
        return realInfoManager.GetName();
    }

    public void SetPlayerID(int ID)
    {
        realInfoManager.SetPlayerID(ID);
    }

    public int GetPlayerID()
    {
        return realInfoManager.GetPlayerID();
    }

    public void SetPlayerObjectID(NetworkInstanceId playerObjectID)
    {
        realInfoManager.SetPlayerObjectID(playerObjectID);
    }

    public NetworkInstanceId GetPlayerObjectID()
    {
        return realInfoManager.GetPlayerObjectID();
    }

    public void SetPlayerConnection(NetworkConnection networkConnection)
    {
        realInfoManager.SetPlayerConnection(networkConnection);
    }

    public NetworkConnection GetPlayerConnection()
    {
        return realInfoManager.GetPlayerConnection();
    }
}
