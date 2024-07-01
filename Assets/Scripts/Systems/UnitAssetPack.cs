using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "UnitAssetPack",menuName = "Data/Unit asset pack")]
public class UnitAssetPack : ScriptableObject
{
    [SerializeField]
    public DialogueData[] TextFiles;

    [SerializeField]
    public EventData[] EventDatas;

    public string[] URLs;

    public string textFilesPath;
    public string eventDatasPath;

#if UNITY_EDITOR



    public void UpdateTextFiles()
    {
        UpdateTableWithElements<DialogueData>(textFilesPath, out TextFiles);
    }

    public void UpdateEventDatas()
    {
        UpdateTableWithElements<EventData>(eventDatasPath, out EventDatas);
    }

    public void SetupArrayURLs()
    {
        URLs = new string[TextFiles.Length];
        for (int i = 0; i < URLs.Length; i++)
        {
            URLs[i] = TextFiles[i].pastebinURL;
        }
    }

    public void UpdateTextFilesURLs()
    {
        try
        {
            for (int i = 0; i < TextFiles.Length; i++)
            {
                if (!string.IsNullOrEmpty(URLs[i]))
                {
                    TextFiles[i].pastebinURL = URLs[i];
                    EditorUtility.SetDirty(TextFiles[i]);
                }
                else
                {
                    Debug.Log("Invalid URL");
                }
            }
            AssetDatabase.SaveAssets();
        }
        catch (Exception)
        {
            Debug.LogError("Length mismath. Try to setup linking array again");
            throw;
        }
        
    }

    public void DeleteLinkingArray()
    {
        URLs = new string[] { };
    }

    void UpdateTableWithElements<T>(string path, out T[] folder) where T : UnityEngine.Object
    {
        string[] folders = AssetDatabase.GetSubFolders(path);

        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", 
            new string[] { path }.Concat(folders).ToArray());

        folder = new T[guids.Length];
        
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T dialogueData = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (!folder.Contains(dialogueData))
            {
                folder[i] = dialogueData;
            }
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

#endif
}
