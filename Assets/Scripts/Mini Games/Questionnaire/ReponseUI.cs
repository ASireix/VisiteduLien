using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReponseUI : Reponse
{
    [SerializeField] Image reponseBackground;
    public override void Display(Question q)
    {
        base.Display(q);
        reponseBackground.color = question.questionnaire.style.unselectedColor;
    }

    public override void Hide(bool instant = false)
    {
        if (shineCoro != null)
        {
            StopCoroutine(shineCoro);
            reponseBackground.color = question.questionnaire.style.unselectedColor;
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

    public override void IndicateAnswer(bool isTrue)
    {
        answered = true;
        if (isTrue)
        {
            shineCoro = StartCoroutine(
                ShineAnswer(question.questionnaire.style.unselectedColor,
                question.questionnaire.style.correctColor));
            reponseBackground.color = question.questionnaire.style.correctColor;
        }
        else
        {
            reponseBackground.color = question.questionnaire.style.incorrectColor;
        }
    }

    public override void ResetRep()
    {
        if (shineCoro != null)
        {
            StopCoroutine(shineCoro);
            reponseBackground.color = question.questionnaire.style.unselectedColor;
        }
        answered = false;
    }

    public override void VisualSelect(bool isTrue)
    {
        if (isTrue)
        {
            reponseBackground.color = question.questionnaire.style.selectedColor;
        }
        else
        {
            reponseBackground.color = question.questionnaire.style.unselectedColor;
        }
    }

    protected override IEnumerator ShineAnswer(Color baseCol, Color toCol)
    {
        Color c = baseCol;
        while (true)
        {
            c = Color.Lerp(toCol, baseCol, Mathf.Sin(Time.time * 5f));
            reponseBackground.color = c;
            yield return new WaitForEndOfFrame();
        }
    }
}
