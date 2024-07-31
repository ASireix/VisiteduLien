using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Reponse : TextBoxContainer
{
    [TextArea]
    [SerializeField] protected string reponseText;
    protected Question question;

    protected bool answered;
    protected Coroutine shineCoro;

    protected void OnValidate()
    {
        if (!string.IsNullOrEmpty(reponseText))
        {
            gameObject.name = reponseText.Truncate(7);
            textBox.WriteText(reponseText, true);
        }
    }

    protected override void OnStart()
    {
        textBox.onClickEvent.AddListener(SelectAnswer);
    }

    public virtual void Display(Question q)
    {
        textBox.ToggleBox(true);
        textBox.WriteText(reponseText);
        question = q;
    }

    protected void SelectAnswer()
    {
        if (!answered)
        {
            question.OnAnswerSelect(this);
        }
    }

    public void Shake()
    {
        textBox.Shake();
    }

    public abstract void VisualSelect(bool isTrue);

    public abstract void IndicateAnswer(bool isTrue);

    protected abstract IEnumerator ShineAnswer(Color baseCol, Color toCol);

    public abstract void Hide(bool instant = false);

    public abstract void ResetRep();
}

public static class StringExt
{
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
}
