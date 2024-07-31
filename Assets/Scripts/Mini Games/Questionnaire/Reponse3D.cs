using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reponse3D : Reponse
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] int altMaterialIndex = 1;

    Material altMat;

    protected override void OnStart()
    {
        textBox.onClickEvent.AddListener(SelectAnswer);
        altMat = meshRenderer.materials[altMaterialIndex];
    }

    public override void Display(Question q)
    {
        base.Display(q);
        altMat.SetColor("_Color", question.questionnaire.style.unselectedColor);
    }


    public override void VisualSelect(bool isTrue)
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

    public override void IndicateAnswer(bool isTrue)
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

    protected override IEnumerator ShineAnswer(Color baseCol, Color toCol)
    {
        Color c = baseCol;
        while (true)
        {
            c = Color.Lerp(toCol, baseCol, Mathf.Sin(Time.time * 5f));
            altMat.SetColor("_Color", c);
            yield return new WaitForEndOfFrame();
        }
    }

    public override void Hide(bool instant = false)
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

    public override void ResetRep()
    {
        if (shineCoro != null)
        {
            StopCoroutine(shineCoro);
            altMat.SetColor("_Color", question.questionnaire.style.unselectedColor);
        }
        answered = false;
    }
}
