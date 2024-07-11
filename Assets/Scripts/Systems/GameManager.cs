using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UnitAssetPack assetPack;

    [SerializeField] int amountEventsRequiredToWin;
    [SerializeField] TextMeshProUGUI alertText;
    [SerializeField] int currentAmount = 0;

    [Space]
    [SerializeField] ImageTracking imageTracking;

    private void Awake()
    {
        currentAmount = 0;
        for (int i = 0; i < assetPack.EventDatas.Length; i++)
        {
            if (assetPack.EventDatas[i].isCompleted) currentAmount++;
        }
        Evenement.onEventCompleted.AddListener(CheckGameWin);
        //ShowAlert();
    }

    void CheckGameWin(EventData eventData)
    {
        currentAmount = 0;
        for (int i = 0; i < assetPack.EventDatas.Length; i++)
        {
            if (assetPack.EventDatas[i].isCompleted) currentAmount++;
        }
        if (currentAmount >= amountEventsRequiredToWin)
        {
            WinGame();
        }
        //ShowAlert();
    }

    void WinGame()
    {
        Debug.Log("you win");
        imageTracking.ToggleTracking(false);
        imageTracking.LaunchLastEvent();
    }

    void ShowAlert()
    {
        alertText.text = "You completed " + currentAmount + " events";
        alertText.gameObject.SetActive(true);
    }
}
