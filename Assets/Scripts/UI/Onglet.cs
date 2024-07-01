using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Onglet : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    bool _opened;
    [HideInInspector]
    public LeanTweenTransition leanTweenTransition;
    [System.NonSerialized]
    public UnityEvent<bool, Onglet> onOngletOpened = new UnityEvent<bool, Onglet>();

    void Start()
    {
        leanTweenTransition = GetComponent<LeanTweenTransition>();
    }

    private Vector2 GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        Vector2 draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? Vector2.right : Vector2.left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? Vector2.up : Vector2.down;
        }
        return draggedDir;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        if (Equals(GetDragDirection(dragVectorDirection), Vector2.right) && !_opened)
        {
            leanTweenTransition.TriggerTransition(TransitionDirection.Right);
            _opened = true;
            onOngletOpened?.Invoke(true, this);
        }
        else if (Equals(GetDragDirection(dragVectorDirection), Vector2.left) && _opened)
        {
            leanTweenTransition.TriggerTransition(TransitionDirection.Left);
            _opened = false;
            onOngletOpened?.Invoke(false, this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        GetDragDirection(dragVectorDirection);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
    }

    public void Open()
    {
        leanTweenTransition.TriggerTransition(TransitionDirection.Right);
        _opened = true;
        onOngletOpened?.Invoke(true, this);
    }

    public void Close()
    {
        leanTweenTransition.TriggerTransition(TransitionDirection.Left);
        _opened = false;
        onOngletOpened?.Invoke(false, this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_opened)
        {
            leanTweenTransition.TriggerTransition(TransitionDirection.Left);
            _opened = false;
            onOngletOpened?.Invoke(false, this);
        }
        else
        {
            leanTweenTransition.TriggerTransition(TransitionDirection.Right);
            _opened = true;
            onOngletOpened?.Invoke(true, this);
        }
    }
}
