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

    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}
