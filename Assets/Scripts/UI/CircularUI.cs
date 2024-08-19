using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CircularUI : MonoBehaviour
{
    [SerializeField] RectTransform rotatingPart;
    [SerializeField] RectTransform burgerIcon;
    CanvasGroup rotatingCanvaGroup;
    [SerializeField] Button openButton;
    [SerializeField] Image closeButtonGraphic;
    [SerializeField] Transform buttonsParent;
    RectTransform[] buttons;
    bool _open;

    [Header("Settings")]
    [SerializeField] float globalSpeed = 1f;
    [SerializeField] float rotatingSpeed = 1f;
    //[SerializeField] float scaleSpeed = 0.8f;
    [SerializeField] float circularButtonsFadingSpeed = 0.7f;
    [SerializeField] float circularButtonsDelay = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        buttons = buttonsParent.GetComponentsInChildren<RectTransform>();
        Evenement.onEventStarted.AddListener(ToggleCircularMenu);
        openButton.onClick.AddListener(ToggleCircularMenu);
        Color c = closeButtonGraphic.color;
        c.a = 0f;
        closeButtonGraphic.color = c;
        if (!rotatingPart.GetComponent<CanvasGroup>())
        {
            rotatingCanvaGroup = rotatingPart.AddComponent<CanvasGroup>();
            rotatingCanvaGroup.alpha = 0f;
        }
        SetButtonsColor(0f);
    }

    public void ToggleCircularMenu(EventData evt=null)
    {
        if (_open)
        {
            CloseCircularMenu();
            _open = !_open;
        }
    }

    public void ToggleCircularMenu()
    {
        if (_open)
        {
            CloseCircularMenu();
        }
        else
        {
            OpenCircularMenu();
        }
        _open = !_open;

    }

    void OpenCircularMenu()
    {
        Debug.Log("Rotating");
        CallTweenTransitions(180f, rotatingSpeed, 1f);

        /*LeanTween.scale(openButton.gameObject, Vector3.one * 0.5f, scaleSpeed * globalSpeed).setOnComplete(() =>
        {
            LeanTween.imageColor(closeButtonGraphic.rectTransform,Color.white, 0.5f*globalSpeed);
        });
        */
        LeanTween.imageColor(burgerIcon, Color.clear, 0.5f);
        LeanTween.rotateAroundLocal(closeButtonGraphic.rectTransform, Vector3.forward, 90f, 0.7f);
        LeanTween.imageColor(closeButtonGraphic.rectTransform, Color.white, 0.4f * globalSpeed);
        StartCoroutine(FadeButton(1f,circularButtonsDelay));
    }

    void CloseCircularMenu()
    {
        CallTweenTransitions(0f, rotatingSpeed, 0.3f);
        //LeanTween.scale(openButton.gameObject, Vector3.one, scaleSpeed* globalSpeed).setEaseOutBounce();

        LeanTween.imageColor(burgerIcon, Color.white, 0.4f);
        LeanTween.rotateAroundLocal(closeButtonGraphic.rectTransform, Vector3.forward, 90f, 0.7f);
        LeanTween.imageColor(closeButtonGraphic.rectTransform, Color.clear, 0.5f * globalSpeed);
        StartCoroutine(FadeButton(0f));
    }

    void CallTweenTransitions(float rotation, float rotaSpeed, float rotaScale)
    {
        LeanTween.rotateZ(rotatingPart.gameObject, rotation, rotaSpeed * globalSpeed);
        LeanTween.scale(rotatingPart, Vector3.one * rotaScale, rotaSpeed / 2 * globalSpeed);

        LeanTween.value(rotatingPart.gameObject, 0f, 1f, rotaSpeed * globalSpeed).setOnUpdate((value) =>
        {
            rotatingCanvaGroup.alpha = value;
        });

    }

    IEnumerator FadeButton(float to,float delay=0f)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            float baseV = 1 - to;
            CanvasGroup canvasGroup = buttons[i].GetComponent<CanvasGroup>();
            LeanTween.value(buttons[i].gameObject, baseV, to, circularButtonsFadingSpeed*globalSpeed).setOnUpdate((value) =>
            {
                canvasGroup.alpha = value;
            });
            yield return new WaitForSeconds(delay * globalSpeed);
        }
    }

    void SetButtonsColor(float to)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].GetComponent<CanvasGroup>())
            {
                buttons[i].AddComponent<CanvasGroup>().alpha = to;
            }
        }
    }
}
