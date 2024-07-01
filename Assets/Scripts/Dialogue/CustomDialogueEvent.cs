using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomDialogueEvent
{
    public UnityEvent<Dialogue> eventToTrigger;
    public string eventIdentifier; //ex : Do a barrel roll -> It needs to be put in the txt file in this format : [EVENT="eventIdentifier"]
}
