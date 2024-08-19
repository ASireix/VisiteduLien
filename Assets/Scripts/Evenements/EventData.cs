using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Event Data", menuName = "Data/Event Data")]
public class EventData : ScriptableObject
{
    [ScriptableObjectId]
    public string Id;
    public bool isCompleted;
    public bool isIntroCompleted;
    //public string code;
    [Tooltip("Objet 2d à afficher depuis la carte")]
    public GameObject objectToShow;

    public void OnCodeCracked(){
        InfoManager.instance.AddInfo(Id,objectToShow);
    }

    public void ResetData(){
        isCompleted = false;
        isIntroCompleted = false;
    }
}
