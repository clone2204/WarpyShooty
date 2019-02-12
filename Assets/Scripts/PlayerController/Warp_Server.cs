using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Warp_Server : NetworkBehaviour, Warp
{
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void WarpPlayer()
    {
        WarpLocations.Location playerLocation = WarpLocations.GetLocation(transform);
        WarpLocations.Location warpLocation;

        if(playerLocation == WarpLocations.Location.Blue)
        {
            warpLocation = WarpLocations.Location.Red;
        }
        else if(playerLocation == WarpLocations.Location.Red)
        {
            warpLocation = WarpLocations.Location.Blue;
        }
        else
        {
            warpLocation = WarpLocations.Location.NONE;
        }

        Debug.Log("Warp Player To: " + warpLocation.ToString());
        WarpPlayerToLocation(warpLocation);
    }
    
    public void WarpPlayerToLocation(WarpLocations.Location location)
    {
        int warpOffset = 2000;
        float xCoord = transform.position.x;
        float zCoord = transform.position.z;

        if (WarpLocations.GetLocation(transform) == location || location == WarpLocations.Location.NONE)
            return;

        if (location == WarpLocations.Location.Blue)
        {
            xCoord -= warpOffset;
        }
        else if (location == WarpLocations.Location.Red)
        {
            xCoord += warpOffset;
        }

        transform.position = new Vector3(xCoord, transform.position.y, zCoord);
    }
}
