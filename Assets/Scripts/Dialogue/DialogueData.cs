using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue 1", menuName = "Data/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string pastebinURL;
    public TextAsset textFile;

    [HideInInspector]
    public string _persistantTextFile = "";
    public string GetText(){
        if (string.IsNullOrEmpty(_persistantTextFile)){
            return textFile.text;
        }else{
            return _persistantTextFile;
        }
    }
}
