using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFading : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] bool fade_In;
    [SerializeField] bool fade_Out;

    [Header("Parameters")]
    [SerializeField] float delay;
    [SerializeField] float duration;

    [SerializeField] bool onEnable;

    CanvasGroup alphaModifier;

    private void Start()
    {
        GetAlpha();
    }

    void GetAlpha()
    {
        if (!TryGetComponent<CanvasGroup>(out alphaModifier))
        {
            alphaModifier = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void Disable()
    {
        if (!alphaModifier)
        {
            GetAlpha();
        }

        StartCoroutine(Fade(0f, true));
    }

    public void Enable()
    {
        if (!alphaModifier)
        {
            GetAlpha();
        }

        StartCoroutine(Fade(1f));
    }

    IEnumerator Fade(float to, bool disable = false)
    {
        alphaModifier.alpha = 1f-to;
        for (float i = 0f; i < 1f; i += Time.deltaTime / duration)
        {
            alphaModifier.alpha = Mathf.Abs(1f-to-i);
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(!disable);
    }

    private void OnEnable()
    {
        if (!alphaModifier)
        {
            GetAlpha();
        }

        StartCoroutine(Fade(1f));
    }

}
