using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenFade : MonoBehaviour
{
    [SerializeField] TweenScriptableObject param;
    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        if (canvasGroup)
        {
            FadeCanvasGroup(0f,1f);
        }
        else
        {
            try
            {
                canvasGroup = GetComponent<CanvasGroup>();
                FadeCanvasGroup(0f,1f);
            }
            catch (System.Exception)
            {
                gameObject.SetActive(true);
            }
        }
    }

    void FadeCanvasGroup(float from, float to){
        LeanTween.value(from, to, param.duration).
            setOnUpdate((float value) =>
            {
                canvasGroup.alpha = value;
            });
    }

    public void Disable()
    {
        if (canvasGroup)
        {
            LeanTween.value(1f, 0f, param.duration).
            setOnUpdate((float value) =>
            {
                canvasGroup.alpha = value;
            }).setOnComplete(() => { gameObject.SetActive(false); });
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
