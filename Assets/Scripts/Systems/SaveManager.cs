using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public SaveSystem saveSystem;

    public static SaveManager instance;
    public bool firstSave { get; private set; }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        firstSave = saveSystem.LoadSave();
    }

    void OnEventComplete(){
        saveSystem.Save();
    }
}
