using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CodeListener : MonoBehaviour
{
    [SerializeField] List<string> codes;

    public UnityEvent onCodeCracked = new UnityEvent();
    public bool CrackCode(Digicode digicode, string code)
    {
        bool result = codes.Contains(code);
        if (result) onCodeCracked?.Invoke();

        return result;
    }
}
