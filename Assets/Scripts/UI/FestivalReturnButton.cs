using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FestivalReturnButton : MonoBehaviour
{
    [SerializeField] GameObject focusCam;
    [SerializeField] GameObject unfocusCam;
    [SerializeField] CanvasGroup mainCanva;
    [SerializeField] GameObject environment;
    [SerializeField] CanvasGroup festivalCanva;
    [Header("Tutorial")]
    [SerializeField] GameObject touchTuto;

    int _state;
    bool _inTransition;

    public void GoBack()
    {
        switch (_state)
        {
            case 0:
                ShowMainMenu();
                break;
            case 1:
                Unfocus();
                break;
            default:
                break;
        }
    }

    public void Focus()
    {
        touchTuto.SetActive(false);
        ChangeCam(true);
        _state = 1;
    }

    public void Unfocus()
    {
        ChangeCam(false);
        _state = 0;
    }

    void ChangeCam(bool focus)
    {
        focusCam.SetActive(focus);
        unfocusCam.SetActive(!focus);
    }

    void ShowMainMenu()
    {
        if (_inTransition) return;
        _inTransition = true;
        mainCanva.alpha = 0;
        mainCanva.gameObject.SetActive(true);
        LeanTween.value(1f, 0f, 2f).setOnUpdate((float val) =>
        {
            mainCanva.alpha = 1 - val;
            festivalCanva.alpha = val;
        }).setOnComplete(() =>
        {
            _inTransition = false;
            environment.SetActive(false);
            festivalCanva.gameObject.SetActive(false);
        });
    }

    public void ShowFestivalEvent()
    {
        if (_inTransition) return;
        _inTransition = true;
        festivalCanva.alpha = 0;
        festivalCanva.gameObject.SetActive(true);
        environment.SetActive(true);
        LeanTween.value(1f, 0f, 2f).setOnUpdate((float val) =>
        {
            mainCanva.alpha = val;
            festivalCanva.alpha = 1 - val;
        }).setOnComplete(() =>
        {
            _inTransition = false;
            mainCanva.gameObject.SetActive(false);
        });
    }
}
