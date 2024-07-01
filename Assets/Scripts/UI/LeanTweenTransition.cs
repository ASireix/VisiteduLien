using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LeantweenTransitionType
{
    Linear,
    Cubic,
    Bounce
}

public enum FadeType
{
    None,
    FadeIn,
    FadeOut
}

public enum TransitionDirection
{
    None,
    Left,
    Right,
    Up,
    Down
}

[RequireComponent(typeof(CanvasGroup))]
public class LeanTweenTransition : MonoBehaviour
{
    [SerializeField] float transitionSpeed;
    [SerializeField] float distance;
    [SerializeField][Range(-2f,2f)] float distanceInPercentage;
    [SerializeField] bool useScreenPercentage;
    [SerializeField] FadeType fadeType;
    [SerializeField] TransitionDirection transitionDirection;
    [SerializeField] LeantweenTransitionType leantweenTransitionType;
    CanvasGroup canvasGroup;

    Vector3 startPos;
    bool _isTweening;

    CanvasScaler canvasScaler;
    Vector3 screenProportion;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        startPos = transform.localPosition;
        canvasScaler = GetComponentInParent<CanvasScaler>();
        screenProportion = new Vector3(Screen.width, Screen.height, 0f);
    }

    public void TriggerTransition(TransitionDirection direction = TransitionDirection.None)
    {
        if (direction == TransitionDirection.None)
        {
            direction = transitionDirection;
        }
        switch (direction)
        {
            
            case TransitionDirection.Left:
                Debug.Log("Addition is " + (screenProportion * distanceInPercentage).ScaledBy(-1f, 0f, 0f));
                TransitWithDirection(useScreenPercentage ?startPos + (screenProportion * distanceInPercentage).ScaledBy(-1f,0f,0f) : 
                    (startPos + Vector3.left * distance));
                break;
            case TransitionDirection.Right:
                Debug.Log("Addition is " + (screenProportion * distanceInPercentage).ScaledBy(1f, 0f, 0f));
                TransitWithDirection(useScreenPercentage ?startPos + (screenProportion * distanceInPercentage).ScaledBy(1f,0f,0f) : 
                    (startPos + Vector3.right * distance));
                break;
            case TransitionDirection.Up:
                TransitWithDirection(startPos + Vector3.up * (useScreenPercentage ? distanceInPercentage/ 100f * Camera.main.pixelHeight : distance));
                break;
            case TransitionDirection.Down:
                TransitWithDirection(startPos + Vector3.down * (useScreenPercentage ? distanceInPercentage/100f * Camera.main.pixelHeight : distance));
                break;
            default:
                break;
        }
    }
    public void TriggerTransition(Vector3 endDest)
    {
        TransitWithDirection(endDest);
    }

    void TransitWithDirection(Vector3 endPoint)
    {
        //if (_isTweening) return;
        _isTweening = true;

        LTSeq sequence = LeanTween.sequence();

        switch (fadeType)
        {
            case FadeType.FadeIn:
                sequence.append(LeanTween.value(0f, 1f, transitionSpeed).setOnUpdate((float value) =>
                {
                    canvasGroup.alpha = value;
                }));
                break;
            case FadeType.FadeOut:
                sequence.append(LeanTween.value(1f, 0f, transitionSpeed).setOnUpdate((float value) =>
                {
                    canvasGroup.alpha = value;
                }));
                break;
            default:
                break;
        }
        switch (leantweenTransitionType)
        {
            case LeantweenTransitionType.Linear:

                sequence.append(LeanTween.move((RectTransform)transform, endPoint, transitionSpeed).
                setEase(LeanTweenType.linear));
                break;
            case LeantweenTransitionType.Cubic:
                sequence.append(LeanTween.move((RectTransform)transform, endPoint, transitionSpeed).
                setEase(LeanTweenType.easeInOutCubic));
                break;
            case LeantweenTransitionType.Bounce:
                sequence.append(LeanTween.move((RectTransform)transform, endPoint, transitionSpeed).
                setEase(LeanTweenType.easeOutBounce));
                break;
            default:
                break;
        }
        startPos = endPoint;
        sequence.append(LeanTween.alpha(gameObject,1,0f).setOnComplete(()=>{_isTweening = false;
        }));
    }
}
