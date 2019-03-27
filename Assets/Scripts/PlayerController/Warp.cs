using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Warp : NetworkBehaviour, IWarp
{
    private IWarp realWarp;

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

    public void WarpPlayerToLocation(Location location)
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
    private void CmdWarpPlayer() 
    {
        WarpPlayer();
    }

    [Command]
    private void CmdWarpPlayerToLocation(Location location)
    {
        WarpPlayerToLocation(location);
    }

    public enum Location
    {
        Red,
        Blue,
        NONE
    }

    public static Location GetLocation(Transform playerTransform)
    {
        if (playerTransform.position.x > 500)
        {
            return Location.Blue;
        }
        if (playerTransform.position.x < -500)
        {
            return Location.Red;
        }

        return Location.NONE;
    }
}
