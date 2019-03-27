using UnityEngine;
using System.Collections;

public interface IWarp
{
    void WarpPlayer();

    void WarpPlayerToLocation(Warp.Location location);
}
