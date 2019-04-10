using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Warp : NetworkBehaviour, IWarp
{
    
    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    
    //=================================================================================================
    //Interface Functions
    //=================================================================================================

    public void WarpPlayer()
    {
        CmdWarpPlayer();
    }

    public void WarpPlayerToLocation(Location location)
    {
        CmdWarpPlayerToLocation(location);
    }

    //=================================================================================================
    //Helper Functions
    //=================================================================================================

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

    //=================================================================================================
    //Server Functions
    //=================================================================================================

    [Command]
    private void CmdWarpPlayer() 
    {
        Warp.Location playerLocation = Warp.GetLocation(transform);
        Warp.Location warpLocation;

        if (playerLocation == Warp.Location.Blue)
        {
            warpLocation = Warp.Location.Red;
        }
        else if (playerLocation == Warp.Location.Red)
        {
            warpLocation = Warp.Location.Blue;
        }
        else
        {
            warpLocation = Warp.Location.NONE;
        }

        WarpPlayerToLocation(warpLocation);
    }

    [Command]
    private void CmdWarpPlayerToLocation(Location location)
    {
        int warpOffset = -2000;
        float xCoord = transform.position.x;
        float zCoord = transform.position.z;

        if (Warp.GetLocation(transform) == location || location == Warp.Location.NONE)
            return;

        if (location == Warp.Location.Blue)
        {
            xCoord -= warpOffset;
        }
        else if (location == Warp.Location.Red)
        {
            xCoord += warpOffset;
        }

        transform.position = new Vector3(xCoord, transform.position.y, zCoord);
    }

    //=================================================================================================
    //Client Functions
    //=================================================================================================

}
