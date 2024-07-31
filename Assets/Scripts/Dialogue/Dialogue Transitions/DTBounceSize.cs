using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTBounceSize : DialogueTransition
{
    public override void TriggerCloseTransition(DialogueBox dBox)
    {
        LeanTween.scale(dBox.gameObject, Vector3.zero, 1f).setEaseOutBounce().setOnComplete(() =>
        {
            dBox.gameObject.SetActive(false);
        });
    }

    public override void TriggerOpenTransition(DialogueBox dBox)
    {
        dBox.gameObject.SetActive(false);
        LeanTween.scale(dBox.gameObject, Vector3.zero, 0f).setOnComplete(() =>
        {
            dBox.gameObject.SetActive(true);
            LeanTween.scale(dBox.gameObject, Vector3.one, 1f).setEaseInBounce();
        });
    }
}
