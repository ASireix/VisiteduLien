using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Difference : MonoBehaviour, IPointerClickHandler
{
    [System.NonSerialized] public UnityEvent<string> onSelect = new UnityEvent<string>();
    [HideInInspector] public Image image;

    public void OnPointerClick(PointerEventData eventData)
    {
        onSelect?.Invoke(this.name);
    }
}
