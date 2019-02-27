using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfoManager_Proxy : NetworkBehaviour, PlayerInfoManager
{
    private PlayerInfoManager realInfoManager;

    void Awake()
    {
        if (isServer)
        {
            realInfoManager = gameObject.GetComponent<PlayerInfoManager_Server>();
            PlayerInfoManager client = gameObject.GetComponent<PlayerInfoManager_Client>();

            realInfoManager.Init(client);
            client.Init(realInfoManager);
        }
        else
        {
            realInfoManager = gameObject.GetComponent<PlayerInfoManager_Client>();
            PlayerInfoManager server = gameObject.GetComponent<PlayerInfoManager_Server>();

            realInfoManager.Init(server);
            server.Init(realInfoManager);
        }

        if(isLocalPlayer)
        {
            string name = GameObject.Find("_SCRIPTS_").GetComponent<SettingsManager>().GetPlayerName();
            SetName(name);

            gameObject.tag = "localPlayer";
        }
    }

    public void Init(PlayerInfoManager playerInfoManager)
    {
        Awake();
    }

    public void SetName(string name)
    {
        Debug.LogWarning("Set Name: " + name);
        realInfoManager.SetName(name);
    }

    public string GetName()
    {
        return realInfoManager.GetName();
    }

}
