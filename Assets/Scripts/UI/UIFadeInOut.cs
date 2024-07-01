using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIFadeInOut : MonoBehaviour
{
    [SerializeField] bool fadeOnStart;
    [SerializeField] float fadeSpeed;
    [SerializeField] bool fadeIn;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (fadeOnStart)
        {
            StartCoroutine(FadeInOut());
        }
    }

    public void CallFade()
    {
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        Color c = image.color;
        if (fadeIn)
        {
            for (float i = 0f; i < 1f; i += Time.deltaTime / fadeSpeed)
            {
                c.a = Mathf.Lerp(1f, 0f, i);
                image.color = c;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            for (float i = 0f; i < 1f; i += Time.deltaTime / fadeSpeed)
            {
                c.a = Mathf.Lerp(0f, 1f, i);
                image.color = c;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
