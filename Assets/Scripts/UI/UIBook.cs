using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBook : MonoBehaviour
{
    [SerializeField] List<GameObject> pages;

    [SerializeField] Button nextPage;
    [SerializeField] Button previousPage;
    [SerializeField] int startPage;
    public int currentPage { get; private set; }

    private void Start()
    {
        currentPage = startPage;
        UpdateButtons();

        nextPage.onClick.AddListener(NextPage);
        previousPage.onClick.AddListener(PreviousPage);
    }

    public void NextPage()
    {
        if (currentPage < pages.Count-1)
        {
            SwitchPage(currentPage+1);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            SwitchPage(currentPage - 1);
        }
    }

    public void SwitchPage(int page)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == page);
        }
        currentPage = page;
        UpdateButtons();
    }

    void UpdateButtons()
    {
        if (pages == null) return;

        previousPage.gameObject.SetActive(currentPage != 0);
        nextPage.gameObject.SetActive(currentPage < pages.Count-1);
    }
}
