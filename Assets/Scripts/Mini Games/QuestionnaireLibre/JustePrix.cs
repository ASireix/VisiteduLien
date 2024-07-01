using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JustePrix : MonoBehaviour
{
    //This need to be place on a QuestionLibre object

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject indicationPlus;
    [SerializeField] GameObject indicationMinus;

    public void UpperIndication()
    {
        indicationPlus.SetActive(false);
        indicationPlus.SetActive(true);
    }

    public void LowerIndication()
    {
        indicationMinus.SetActive(false);
        indicationMinus.SetActive(true);
    }
}
