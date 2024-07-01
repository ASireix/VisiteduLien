using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeManager : MonoBehaviour
{
    [SerializeField] string masterCode;
    Dictionary<string, EventData> codeEventDico = new Dictionary<string, EventData>();
    [SerializeField] SaveSystem saveSystem;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in saveSystem.eventDatas)
        {
            codeEventDico.TryAdd(item.code, item);
        }
        Digicode.onValidate.AddListener(CheckCodeFromManager);
        Evenement.onEventCompleted.AddListener(CheckCodeEvent);
        CrackAllSavedCode();
    }

    void CheckCodeEvent(EventData da){
        CheckCodeFromManager(da.code);
    }


    void CheckCodeFromManager(string code, Digicode digicode)
    {
        if (code.Equals(masterCode))
        {
            UnlockAll();
            digicode.AcquireCodeResult(true);
            return;
        }
        else if (codeEventDico.TryGetValue(code, out EventData eventData))
        {
            eventData.OnCodeCracked();
            digicode.AcquireCodeResult(true);
        }
        else
        {
            Debug.Log("Incorrect code");
            digicode.AcquireCodeResult(false);
        }

    }

    void CheckCodeFromManager(string code)
    {
        if (code.Equals(masterCode))
        {
            UnlockAll();
            return;
        }
        else if (codeEventDico.TryGetValue(code, out EventData eventData))
        {
            eventData.OnCodeCracked();
        }
        else
        {
            Debug.Log("Incorrect code");
        }

    }

    void CrackAllSavedCode()
    {
        for (int i = 0; i < saveSystem.eventDatas.Length; i++)
        {
            if (saveSystem.eventDatas[i].isCompleted)
            {
                CheckCodeFromManager(saveSystem.eventDatas[i].code);
            }
        }
    }

    void UnlockAll()
    {
        foreach (var item in saveSystem.eventDatas)
        {
            item.OnCodeCracked();
        }
    }
}
