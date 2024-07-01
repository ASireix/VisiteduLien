using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueBox : MonoBehaviour, IPointerClickHandler
{
    Dialogue _dialogue;
    [SerializeField] BoucingSize skipDot; // appear when you can advance the dialogue

    [SerializeField] GameObject[] additionalBoxes;
    public void UseDialogueBox(Dialogue dial, BoucingSize skip = null)
    {
        if (skip) skipDot = skip;
        if (skipDot) skipDot.gameObject.SetActive(false);

        _dialogue = dial;
        _dialogue.onSentenceTyped.AddListener(SkitDotEnabler);

        if (additionalBoxes == null) return;

        for (int i = 0; i < additionalBoxes.Length; i++)
        {
            if (additionalBoxes[i].TryGetComponent(out DialogueBox dialo))
            {
                dialo.UseDialogueBox(_dialogue, skip);
            }
            else
            {
                additionalBoxes[i].AddComponent<DialogueBox>().UseDialogueBox(_dialogue, skipDot);
            }
        }
    }

    public void DisposeDialogueBox(Dialogue dial)
    {
        if (skipDot) skipDot.gameObject.SetActive(false);

        _dialogue.onSentenceTyped.RemoveListener(SkitDotEnabler);
        _dialogue = null;
    }

    void SkitDotEnabler()
    {
        skipDot.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (skipDot)
        {
            if (skipDot.gameObject.activeSelf) skipDot.gameObject.SetActive(false);
        }
        _dialogue.AdvanceDialogue();
    }



}
