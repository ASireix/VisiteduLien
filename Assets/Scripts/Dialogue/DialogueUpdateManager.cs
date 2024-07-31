using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueUpdateManager : MonoBehaviour
{
    [SerializeField] UnitAssetPack unitAssetPack;
    void Start()
    {
        //StartCoroutine(UpdateFiles());
    }

    public IEnumerator UpdateFiles(System.Action<float> onProgress)
    {
        bool finished = false;
        string txt = "";
        int completedUpdates = 0;
        int errors = 0;
        for (int i = 0; i < unitAssetPack.TextFiles.Length; i++)
        {

            yield return StartCoroutine(WebTextHandler.
            GetText(unitAssetPack.TextFiles[i].pastebinURL, (bool d, string t) =>
            {
                finished = d;
                txt = t;
            }));

            DialogueData dialogueData = unitAssetPack.TextFiles[i];
            string path = Path.Combine(Application.persistentDataPath, dialogueData.textFile.name + ".txt");

            Debug.Log("Asset is : " + unitAssetPack.TextFiles[i].name);

            if (!string.IsNullOrEmpty(txt))
            {
                Debug.Log("Writing to path : " + path);

                yield return File.WriteAllTextAsync(path, txt);

                dialogueData._persistantTextFile = new StreamReader(path).ReadToEnd();

                //AssetDatabase.Refresh();

                completedUpdates++;
            }
            else
            {
                if (File.Exists(path))
                {
                    Debug.Log("Text file already exist...");
                    Debug.Log("Updating dialogue data");
                    dialogueData._persistantTextFile = new StreamReader(path).ReadToEnd();

                    //AssetDatabase.Refresh();
                }
                errors++;
            }
            float p = Mathf.Clamp01((float)(i + 1) / unitAssetPack.TextFiles.Length);
            onProgress?.Invoke(p);
            yield return null;
        }
        Debug.Log($"Successfully updated {completedUpdates} text files with {errors} errors");
    }

}
