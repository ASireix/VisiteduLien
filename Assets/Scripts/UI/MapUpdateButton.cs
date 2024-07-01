using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapUpdateButton : MonoBehaviour, IPointerClickHandler
{
    public int pinHolderID;
    public delegate void CallPinChangeDelegate(int id);
    public CallPinChangeDelegate callPinChange;
    UnityEngine.UI.Outline outline;

    void Awake()
    {
        outline = gameObject.AddComponent<UnityEngine.UI.Outline>();
        outline.enabled = false;
    }

    public void SelectButton()
    {
        outline.enabled = true;
    }

    public void DeselectButton()
    {
        outline.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        callPinChange?.Invoke(pinHolderID);
    }



}
