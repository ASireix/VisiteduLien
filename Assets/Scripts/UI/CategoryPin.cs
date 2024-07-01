using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CategoryPin : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Check in overlay canva the category and selecte the corresponding child id")]
    [SerializeField] int uiObjectId;

    [SerializeField] Category category;
    public void OnPointerClick(PointerEventData eventData)
    {
        CategoryBrowser.onCategorySelected?.Invoke(category,uiObjectId);
    }
}
