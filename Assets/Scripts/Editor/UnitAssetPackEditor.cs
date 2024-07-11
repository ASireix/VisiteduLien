using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitAssetPack))]
public class UnitAssetPackEditor : Editor
{
    UnitAssetPack myAssetPack;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        myAssetPack = (UnitAssetPack)target;

        if (GUILayout.Button("Update Text Files"))
        {
            myAssetPack.UpdateTextFiles();
        }

        if (GUILayout.Button("Update Event Datas"))
        {
            myAssetPack.UpdateEventDatas();
        }

        if (GUILayout.Button("Update Events Datas & Text Files"))
        {
            myAssetPack.UpdateTextFiles();
            myAssetPack.UpdateEventDatas();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Setup Linking Array"))
        {
            myAssetPack.SetupArrayURLs();
        }

        if (GUILayout.Button("Update textfiles URLs") && myAssetPack.URLs.Length == myAssetPack.TextFiles.Length)
        {
            myAssetPack.UpdateTextFilesURLs();
        }

        if (GUILayout.Button("Delete Linking Array"))
        {
            myAssetPack.DeleteLinkingArray();
        }
    }
}
