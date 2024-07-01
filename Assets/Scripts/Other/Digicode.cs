using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class Digicode : MonoBehaviour
{
    [SerializeField] int maxNumberOfLetters;
    [SerializeField] TextMeshProUGUI uiVisualisation;
    [Tooltip("This is what's written on the digicode when there's nothing typed")]
    [SerializeField] string clearVisual;
    [SerializeField] Color normalColor;
    [SerializeField] string errorMsg;
    [SerializeField] Color errorColor;
    [SerializeField] string correctMsg;
    [SerializeField] Color correctColor;
    int codeIndex;
    string[] code;
    DigicodeButton[] buttons;
    public static UnityEvent<string, Digicode> onValidate = new UnityEvent<string, Digicode>();

    [Tooltip("Leave empty for global recognition")]
    [SerializeField] CodeListener listener;
    bool _global;

    private void Start()
    {
        _global = listener == null;

        buttons = GetComponentsInChildren<DigicodeButton>();
        uiVisualisation.color = normalColor;
        InitButtons();
        ClearCode();
    }

    void InitButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onButtonPressed.AddListener(AddCode);
        }
    }

    void AddCode(string letter, bool reset = false, bool validate = false)
    {
        if (validate) CheckCode(string.Join("", code));
        if (reset)
        {
            ClearCode();
            return;
        }

        code[codeIndex] = letter;
        codeIndex++;
        uiVisualisation.text = string.Join(" ", code);
        if (codeIndex > maxNumberOfLetters - 1)
        {
            CheckCode(string.Join("", code));
        }


    }

    IEnumerator BlinkMessage(Color col, string msg = "", bool clearCode = true)
    {
        int max = uiVisualisation.text.ToCharArray().Length;
        if (!string.IsNullOrEmpty(msg))
        {
            uiVisualisation.text = msg;
        }

        uiVisualisation.color = col;

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);
            uiVisualisation.maxVisibleCharacters = 0;
            yield return new WaitForSeconds(0.5f);
            uiVisualisation.maxVisibleCharacters = max;
        }

        if (clearCode) ClearCode();
    }

    void CheckCode(string code)
    {
        Debug.Log("Trying to find the code " + code);
        if (_global)
        {
            onValidate?.Invoke(code, this);
        }
        else
        {
            AcquireCodeResult(listener.CrackCode(this,code));
        }
    }

    public void AcquireCodeResult(bool correct)
    {
        if (correct)
        {
            StartCoroutine(BlinkMessage(correctColor, correctMsg));
        }
        else
        {
            StartCoroutine(BlinkMessage(errorColor, errorMsg));
        }
    }

    void ClearCode()
    {
        code = new string[maxNumberOfLetters];
        codeIndex = 0;
        for (int i = 0; i < maxNumberOfLetters; i++)
        {
            code[i] = " ";
        }
        uiVisualisation.color = normalColor;
        uiVisualisation.text = clearVisual;
    }
}
