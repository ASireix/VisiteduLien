using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenementGeneric : Evenement
{
    [Header("Dialogues Guid�e")]
    public Dialogue introDialogueG;
    [Header("Dialogue Jou�e")]
    public Dialogue introDialogueJ;
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

    public override void GuideeStart()
    {
        introDialogueG.TriggerDialogue();
    }

    public override void JoueeStart()
    {
        introDialogueJ.TriggerDialogue();
    }
}
