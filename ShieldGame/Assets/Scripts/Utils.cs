using UnityEngine;

class Utils
{
    public static bool IsInLayerMask(LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }
}

