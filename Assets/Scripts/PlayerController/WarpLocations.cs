using UnityEngine;
using System.Collections;

public class WarpLocations
{
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
