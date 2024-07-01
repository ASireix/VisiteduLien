using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIVignetteFadeInOut : MonoBehaviour
{
    Vector2 widthHeight;
    RectTransform rect;
    [SerializeField] float speed;
    void Start()
    {
        rect = GetComponent<RectTransform>();
        widthHeight = rect.sizeDelta;
    }

    public IEnumerator FadeInVignette()
    {
        float w;
        float h;
        for (float i = 0f; i < 1f; i += Time.deltaTime / speed)
        {
            w = Mathf.Lerp(widthHeight.x,-50f,i);
            h = Mathf.Lerp(widthHeight.y,-50f,i);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,w);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,h);
            yield return null;
        }
    }

    public IEnumerator FadeOutVignette()
    {
        float w;
        float h;
        for (float i = 0f; i < 1f; i += Time.deltaTime / speed)
        {
            w = Mathf.Lerp(0f,widthHeight.x,i);
            h = Mathf.Lerp(0f,widthHeight.y,i);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,w);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,h);
            yield return null;
        }
    }
}
