using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Reponse : TextBoxContainer
{
    [TextArea]
    [SerializeField] string reponseText;
    Question question;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] int altMaterialIndex = 1;

    Material altMat;
    bool answered;
    Coroutine shineCoro;

    protected override void OnStart()
    {
        textBox.onClickEvent.AddListener(SelectAnswer);
        altMat = meshRenderer.materials[altMaterialIndex];
    }

    public void Display(Question q)
    {
        textBox.ToggleBox(true);
        textBox.WriteText(reponseText);
        question = q;
        altMat.SetColor("_Color", question.questionnaire.style.unselectedColor);
    }

    void SelectAnswer()
    {
        if (!answered)
        {
            //Debug.Log("Answer selected");
            question.OnAnswerSelect(this);
        }
    }

    public void Shake()
    {
        textBox.Shake();
    }

    public void VisualSelect(bool isTrue)
    {
        if (isTrue)
        {
            altMat.SetColor("_Color", question.questionnaire.style.selectedColor);
        }
        else
        {
            altMat.SetColor("_Color", question.questionnaire.style.unselectedColor);
        }
    }

    public void IndicateAnswer(bool isTrue)
    {
        answered = true;
        if (isTrue)
        {
            shineCoro = StartCoroutine(
                ShineAnswer(question.questionnaire.style.unselectedColor,
                question.questionnaire.style.correctColor));
            altMat.SetColor("_Color", question.questionnaire.style.correctColor);
        }
        else
        {
            altMat.SetColor("_Color", question.questionnaire.style.incorrectColor);
        }
    }

    IEnumerator ShineAnswer(Color baseCol, Color toCol)
    {
        Color c = baseCol;
        while (true)
        {
            c = Color.Lerp(toCol, baseCol, Mathf.Sin(Time.time * 5f));
            altMat.SetColor("_Color", c);
            yield return new WaitForEndOfFrame();
        }

    }

    public void Hide(bool instant = false)
    {
        if (shineCoro != null)
        {
            StopCoroutine(shineCoro);
            altMat.SetColor("_Color", question.questionnaire.style.unselectedColor);
        }

        if (instant)
        {
            textBox.ToggleBox(false, BoxToggleType.Disable);
        }
        else
        {
            textBox.ToggleBox(false);
        }

    }

    public void Reset()
    {
        if (shineCoro != null)
        {
            StopCoroutine(shineCoro);
            altMat.SetColor("_Color", question.questionnaire.style.unselectedColor);
        }
        answered = false;
    }
}
