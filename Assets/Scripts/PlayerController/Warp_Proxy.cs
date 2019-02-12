using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Warp_Proxy : NetworkBehaviour, Warp
{
    private Warp realWarp;

    // Use this for initialization
    void Start ()
    {
	    if(isServer)
        {
            realWarp = gameObject.AddComponent<Warp_Server>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void WarpPlayer()
    {
        if (isServer)
        {
            realWarp.WarpPlayer();
        }
        else
        {
            CmdWarpPlayer();
        }
    }

    public void WarpPlayerToLocation(WarpLocations.Location location)
    {
        if (isServer)
        {
            realWarp.WarpPlayerToLocation(location);
        }
        else
        {
            CmdWarpPlayerToLocation(location);
        }
    }

    [Command]
    private void CmdWarpPlayer() //<<<<<<<<<<<<<<<<<<<<<<<<<<<<< THIS WORKS
    {
        WarpPlayer();
    }

    [Command]
    private void CmdWarpPlayerToLocation(WarpLocations.Location location)
    {
        WarpPlayerToLocation(location);
    }

}
