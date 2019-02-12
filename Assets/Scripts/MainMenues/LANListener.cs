using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LANListener : NetworkDiscovery
{
    ServerBrowser serverBrowser;

    // Use this for initialization
    void Start ()
    {

        this.serverBrowser = GameObject.Find("_SCRIPTS_").GetComponent<ServerBrowser>();
        Initialize();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);

        //TODO: Fix this LAN shit

        //Debug.LogWarning(fromAddress + " || " + data);
        //serverBrowser.AddNewServer(fromAddress);
    }
    
}
