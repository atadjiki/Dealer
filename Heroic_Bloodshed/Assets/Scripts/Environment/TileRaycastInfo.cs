using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public struct TileRaycastInfo
{
    public RaycastHit HitInfo;
    public EnvironmentLayer Layer;

    public static TileRaycastInfo Build()
    {
        return new TileRaycastInfo()
        {
            Layer = EnvironmentLayer.NONE,
        };
    }
}
