using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] float fadeDuration;
    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
       // StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(delay);
        for (float i = 0f; i < 1f; i += Time.deltaTime / fadeDuration)
        {
            canvasGroup.alpha = 1f - i;
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1f;
        StartCoroutine(Fade());
    }
}
