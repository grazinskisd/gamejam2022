using UnityEngine;

public static class ExtensionMethods
{
    public static bool IsInLayerMask(this GameObject gameObject, int layerMasks)
    {
        return (layerMasks == (layerMasks | (1 << gameObject.layer)));
    }
}
