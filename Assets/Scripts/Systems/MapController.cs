using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] InputReader inputReader;
    [Tooltip("This rect transform will get scaled to zoom in and out")]
    [SerializeField] RectTransform rectTransform;
    [Tooltip("Will get disabled when zooming in or out")]
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float minimumScale = 1f;
    [SerializeField] float maximumScale = 5f;
    [SerializeField] float startScale = 3f;
    [SerializeField] float pinchInfluence = 1f;
    [SerializeField] float fadeSpeed = 1f;
    [Header("3d MAP")]
    [SerializeField] Animator mapAnimator;
    [SerializeField] float startPosX = -60f;
    [SerializeField] float endPosX = 19f;
    float animationTimeout = 5f;
    bool isOpen = false;

    float timer = 0.1f;
    float currentTimer = 0f;
    float targetScale;
    [SerializeField] float zoomSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        inputReader.pinchEvent += Zoom;
        rectTransform.localScale = Vector3.one;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        LeanTween.moveLocalX(mapAnimator.gameObject, startPosX,0f);
        rectTransform.localScale = Vector3.one * startScale;
        targetScale = startScale;
    }

    private void Update()
    {
        if (currentTimer > timer)
        {
            scrollRect.enabled = true;
        }
        else
        {
            currentTimer += Time.deltaTime;
        }
        if (isOpen)
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, Vector3.one * targetScale, Time.deltaTime * zoomSpeed);
        }
    }

    void Zoom(float amount)
    {
        currentTimer = 0f;
        scrollRect.enabled = false;
        amount = amount * 0.01f;
        Vector3 newScale = rectTransform.localScale + Vector3.one * amount * pinchInfluence;
        float newScaleFloat = rectTransform.localScale.x + amount * pinchInfluence;
        if (newScaleFloat > minimumScale && newScaleFloat < maximumScale)
        {
            targetScale = newScaleFloat;
            //rectTransform.localScale = newScale;
        }
        //Debug.Log($"SCROLLING BY {amount}");
    }

    public void Open()
    {
        if (mapAnimator != null)
        {
            LeanTween.moveLocalX(mapAnimator.gameObject, endPosX, 0.3f);
            rectTransform.localScale = Vector3.one;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            mapAnimator.SetTrigger("Open");
            StartCoroutine(WaitEndOfAnimation(mapAnimator));
        }
    }

    public void Close()
    {
        isOpen = false;
        if (mapAnimator != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            LTSeq seq = LeanTween.sequence();
            seq.append(LeanTween.scale(rectTransform, Vector3.one, 1f));
            seq.append(LeanTween.value(1f, 0f, fadeSpeed).setOnUpdate((float val) =>
            {
                canvasGroup.alpha = val;
            }).setOnComplete(() =>
            {
                mapAnimator.SetTrigger("Close");
                LeanTween.moveLocalX(mapAnimator.gameObject, startPosX, 0.3f);
                rectTransform.localScale = Vector3.one;
            }));
        }
    }

    public void ToggleMap()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    IEnumerator WaitEndOfAnimation(Animator anim)
    {
        float time = 0f;
        while ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || anim.IsInTransition(0)) && time < animationTimeout)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        LeanTween.value(0f, 1f, fadeSpeed).setOnUpdate((float val) =>
        {
            canvasGroup.alpha = val;
        }).setOnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            LeanTween.scale(rectTransform, startScale * Vector3.one, 1f).setOnComplete(() => { isOpen = true; });
        });
    }

    private void OnDisable()
    {
        inputReader.pinchEvent -= Zoom;
    }
}
