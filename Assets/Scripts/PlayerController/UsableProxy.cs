using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UsableProxy : NetworkBehaviour, Usable
{
    private Usable realUsable;

	// Use this for initialization
	void Start ()
    {
	    if(isServer)
        {
            realUsable = gameObject.AddComponent<UsableServer>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartUsingUsable()
    {
        if(isServer)
        {
            realUsable.StartUsingUsable();
        }
        else
        {
            CmdStartUsingUsable();
        }
        
    }

    public void StopUsingUsable()
    {
        if(isServer)
        {
            realUsable.StopUsingUsable();
        }
        else
        {
            CmdStopUsingUsable();
        }
    }

    [Command]
    private void CmdStartUsingUsable()
    {
        StartUsingUsable();
    }

    [Command]
    private void CmdStopUsingUsable()
    {
        StopUsingUsable();
    }
}
