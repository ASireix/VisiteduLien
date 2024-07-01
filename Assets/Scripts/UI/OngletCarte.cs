using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OngletCarte : Onglet
{
    [SerializeField] CanvasGroup overlay;
    [SerializeField] GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        leanTweenTransition = GetComponent<LeanTweenTransition>();
        onOngletOpened.AddListener(OpenCarte);
    }

    void OpenCarte(bool open, Onglet onglet)
    {
        map.SetActive(open);

        if (open)
        {
            this.enabled = false;
            overlay.gameObject.SetActive(true);
            LeanTween.value(0f, 1f, 1f).setOnUpdate((float value) =>
            {
                overlay.alpha = value;
            });
        }
        else
        {
            LeanTween.value(1f, 0f, 0.5f).setOnUpdate((float value) =>
            {
                overlay.alpha = value;
            }).setOnComplete(() => { overlay.gameObject.SetActive(false); });
        }
    }
}
