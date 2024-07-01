using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenementEcole : Evenement
{
    [Header("Dialogues Guidée")]
    public Dialogue introDialogueG;
    [Header("Dialogue Jouée")]
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
