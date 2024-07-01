using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Linq;

public class DialogueDataEditor : EditorWindow
{
    [MenuItem("Assets/Create Dialogue Text Assets")]
    static void CreateDialogueTextAssets()
    {
        // Récupérer le dossier sélectionné dans le Project view
        string selectedFolderPath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        if (!AssetDatabase.IsValidFolder(selectedFolderPath))
        {
            Debug.LogError("Please select a valid folder in the Project view.");
            return;
        }

        string[] subFolders = AssetDatabase.GetSubFolders(selectedFolderPath);

        // Recherche tous les ScriptableObject de type DialogueData dans le dossier sélectionné
        string[] guids = AssetDatabase.FindAssets("t:DialogueData", new[] { selectedFolderPath }.Concat(subFolders).ToArray());
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            
            DialogueData dialogueData = AssetDatabase.LoadAssetAtPath<DialogueData>(assetPath);

            // Créer un fichier TextAsset pour chaque DialogueData
            if (!dialogueData.textFile)
            {
                string fileName = dialogueData.name + "_TEXTFILE.txt";
                string filePath = Path.Combine(Path.GetDirectoryName(assetPath), fileName);

                string defaultContent = "Placeholder";

                // Écriture du contenu dans le fichier avec encodage UTF-8
                File.WriteAllText(filePath, defaultContent, Encoding.UTF8);

                // Rafraîchir l'AssetDatabase pour que le nouveau fichier soit reconnu par Unity
                AssetDatabase.Refresh();

                // Charger le fichier comme TextAsset
                TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
                if (textAsset != null)
                {
                    // Assigner le fichier TextAsset au DialogueData
                    dialogueData.textFile = textAsset;

                    // Sauvegarder les changements
                    EditorUtility.SetDirty(dialogueData);
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    Debug.LogError("Échec de chargement du TextAsset à partir du chemin: " + filePath);
                }
            }
        }

        Debug.Log("Dialogue Text Assets created and moved successfully.");
    }
}