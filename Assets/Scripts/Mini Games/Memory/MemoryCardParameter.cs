using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Card",menuName = "Memory Card")]
public class MemoryCardParameter : ScriptableObject
{
    public Texture2D face;
    public Vector2 tiling;
    public Vector2 offset;
}
