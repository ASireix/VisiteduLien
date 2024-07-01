using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Category
{
    Alimentation,
    Mobilite,
    Nature,
    Diversite,
    Lowtech,
    Economie,
    Architecture
}

public class CategoryBrowser : MonoBehaviour
{
    [Tooltip("Drag all the categories here with all the ui attached to it and DISABLED")]
    [SerializeField] GameObject[] categories;
    public static UnityEvent<Category, int> onCategorySelected = new UnityEvent<Category, int>();

    // Start is called before the first frame update
    void Start()
    {
        onCategorySelected.AddListener(UpdateCategory);
    }

    void UpdateCategory(Category category, int id)
    {

    }
}
