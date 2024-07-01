using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectExtension
{
    public static void MoveLinear(this RectTransform rectTransform)
    {
        
    }

    public static Vector3 ScaledBy(this Vector3 v, float x = 1f, float y = 1f, float z = 1f)
  => new Vector3(x * v.x, y * v.y, z * v.z);
}
