using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueTransition : MonoBehaviour
{
    public abstract void TriggerOpenTransition(DialogueBox dBox);
    public abstract void TriggerCloseTransition(DialogueBox dBox);
}
