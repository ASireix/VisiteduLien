using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIBook))]
public class UIBookEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UIBook myBook = (UIBook)target;

        if (GUILayout.Button("Reset"))
        {
            myBook.SwitchPage(myBook.currentPage);
        }

        if (GUILayout.Button("Next"))
        {
            myBook.NextPage();
        }

        if (GUILayout.Button("Previous"))
        {
            myBook.PreviousPage();
        }
    }
}
