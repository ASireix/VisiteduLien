using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenementCoworking : Evenement
{
    [SerializeField] Transform root;
    [SerializeField] float heightOffset;

    [Header("Dialogues Guid�e")]
    public Dialogue introDialogueG;
    [Header("Dialogue Jou�e")]
    public Dialogue introDialogueJ;

    [SerializeField] Canvas canvaDigicode;

    [Header("Animations")]
    [SerializeField] AnimationClip introClip;
    [SerializeField] AnimationClip outroClip;

    public void usePatrick(string animationName)
    {
        patrick.LaunchAnimation(animationName);
    }

    public override void GuideeStart()
    {
        introDialogueG.TriggerDialogue();
        canvaDigicode.worldCamera = Camera.main;
    }

    public override void JoueeStart()
    {
        introDialogueJ.TriggerDialogue();
        canvaDigicode.worldCamera = Camera.main;
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
