using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ContinueButton : MonoBehaviour
{
    void Start(){
        string base64string = PlayerPrefs.GetString("savesys", "");
        if (string.IsNullOrEmpty(base64string)){
            GetComponent<Button>().interactable = false;
        }
    }
}
