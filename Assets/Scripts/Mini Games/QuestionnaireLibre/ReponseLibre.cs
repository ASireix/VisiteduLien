using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReponseLibre : TextBoxContainer
{
    protected override void OnStart()
    {
        textBox.onClickEvent.AddListener(SelectBox);
    }

    public void HideReponse()
    {
        textBox.ToggleBox(false, BoxToggleType.Disable);
    }

    public void ShowReponse()
    {
        textBox.ToggleBox(true);
    }

    void SelectBox()
    {
        textBox.TrySelectInputField();
    }

    public string GetAnswer()
    {
        return textBox.GetInputText();
    }
    
    public void SetAnswer(string to)
    {
        textBox.TMP_inputField.text = to;
    }
}
