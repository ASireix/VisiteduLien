using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;



public class Interactable : MonoBehaviour, IPointerClickHandler, IEndDragHandler
{
    protected enum DraggedDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    protected DraggedDirection GetDraggedDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);

        if (positiveX > positiveY)
        {
            return (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            return (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnTouch();
    }

    public virtual void OnTouch()
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
