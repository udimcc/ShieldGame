using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

using System.Linq;
using UnityEngine;

class Utils
{
    public static bool IsInLayerMask(LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }

    public static LayerMask OmitLayer(LayerMask mask, int layer)
    {
        int layerToOmit = 1 << layer;
        return mask & (~layerToOmit);
    }

    public static Transform[] GetChildren(Transform transform)
    {
        var childrens = new List<Transform>();

        foreach(Transform child in transform)
        {
            childrens.Add(transform);
        }

        return childrens.ToArray();
    }
}

