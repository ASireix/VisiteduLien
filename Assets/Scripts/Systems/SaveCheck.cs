using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveCheck : MonoBehaviour
{
    [SerializeField] Button continueButton;

    private void Start()
    {
        continueButton.interactable = SaveManager.instance.firstSave;
    }
}
