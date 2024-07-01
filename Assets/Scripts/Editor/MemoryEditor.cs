using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Memory))]
public class MemoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Memory myTarget = (Memory)target;

        if (GUILayout.Button("Show Preview")){
            myTarget.EditorDisplay();
        }

        if (GUILayout.Button("Remove Preview")){
            myTarget.EditorRemove();
        }
    }
}
