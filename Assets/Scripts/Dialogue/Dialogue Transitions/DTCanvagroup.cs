using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTCanvagroup : DialogueTransition
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;
    public override void TriggerCloseTransition(DialogueBox dBox)
    {
        LeanTween.moveLocal(canvasGroup.gameObject, startPos, 1f);
        LeanTween.value(gameObject, 1f, 0f, 1f).setOnUpdate((value) =>
        {
            canvasGroup.alpha = value;
        }).setOnComplete(()=>dBox.gameObject.SetActive(false));
    }

    public override void TriggerOpenTransition(DialogueBox dBox)
    {
        dBox.gameObject.SetActive(true);
        LeanTween.moveLocal(canvasGroup.gameObject, endPos, 1f);
        LeanTween.value(gameObject, 0f, 1f, 1f).setOnUpdate((value) =>
        {
            canvasGroup.alpha = value;
        });
    }
}
