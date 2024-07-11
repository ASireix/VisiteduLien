using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuMiniGamesAdd : MonoBehaviour
{
    const string enigmeAssetPath = "Assets/Prefab/Mini games/Enigme.prefab";
    const string justePrixAssetPath = "Assets/Prefab/Mini games/Juste prix.prefab";
    const string questionnaireAssetPath = "Assets/Prefab/Mini games/Questionnaire.prefab";

    static void CreateMiniGame(string path, MenuCommand command)
    {
        GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GameObject spawnObject = Instantiate(asset);
        spawnObject.name = asset.name;
        GameObjectUtility.SetParentAndAlign(spawnObject, command.context as GameObject);
        Undo.RegisterCompleteObjectUndo(spawnObject,"Create "+spawnObject.name);
        Selection.activeObject = spawnObject;
    }

    [MenuItem("GameObject/Minigame/Enigme")]
    static void CreateMiniGameEnigme(MenuCommand command)
    {
        CreateMiniGame(enigmeAssetPath, command);
        Debug.Log("Enigme");
    }

    [MenuItem("GameObject/Minigame/Juste Prix")]
    static void CreateMiniGameJustePrix(MenuCommand command)
    {
        CreateMiniGame(justePrixAssetPath, command);
        Debug.Log("Juste Prix");
    }

    [MenuItem("GameObject/Minigame/Questionnaire")]
    static void CreateMiniGameQuestionnaire(MenuCommand command)
    {
        CreateMiniGame(questionnaireAssetPath, command);
        Debug.Log("Questionnaire");
    }
}
