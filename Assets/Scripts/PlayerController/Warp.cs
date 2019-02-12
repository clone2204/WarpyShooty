using UnityEngine;
using System.Collections;

public interface Warp
{
    void WarpPlayer();

    void WarpPlayerToLocation(WarpLocations.Location location);
}
