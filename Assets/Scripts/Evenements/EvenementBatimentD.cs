using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenementBatimentD : Evenement
{
    [Header("Dialogues")]
    public Dialogue introDialogue;
    public Dialogue introDialogueGuidee;
    public override void GuideeStart()
    {
        introDialogueGuidee.TriggerDialogue();
    }

    public override void JoueeStart()
    {
        introDialogue.TriggerDialogue();
    }

    public override void CompletedStart()
    {
        if (SETTINGS.isGuidee)
        {
            GuideeStart();
        }
        else
        {
            JoueeStart();
        }
    }
}


