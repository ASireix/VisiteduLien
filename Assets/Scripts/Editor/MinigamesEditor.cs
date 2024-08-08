using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Minigame),true)]
[CanEditMultipleObjects]
public class MinigamesEditor : Editor
{
    Minigame myscript;

    public override void OnInspectorGUI()
    {
        myscript = (Minigame)target;
        if (GUILayout.Button("Play Mini Game") && Application.isPlaying)
        {
            myscript.StartMiniGame();
        }
        DrawDefaultInspector();
    }
}
