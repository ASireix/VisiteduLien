using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestionLibre : TextBoxContainer
{
    //Make sure the textfield area is set to integer or float if you want the juste prix to work
    [TextArea]
    [SerializeField] string questionText;
    [SerializeField] string answer;
    [SerializeField] int differenceTolerance;
    [SerializeField] bool enableJustePrix;

    [Tooltip("Leave empty if not juste prix")]
    [SerializeField] JustePrix justePrix;

    ReponseLibre answerField;

    public UnityEvent onStart = new UnityEvent();

    [Header("Section Essais")]

    public UnityEvent onWrongAnswer = new UnityEvent();
    public UnityEvent onCorrectAnswer = new UnityEvent();

    [Tooltip("Leave at 0 for infinite try")]
    [SerializeField] int maxNumberOfTry = 0;

    int currentTry = 0;

    protected override void OnStart()
    {
        base.OnStart();
        answerField = GetComponentInChildren<ReponseLibre>();
        ResetQuestion();
    }

    void ResetQuestion()
    {
        textBox.ToggleBox(false, BoxToggleType.Disable);
        answerField.HideReponse();
    }

    public void StartQuestionLibre()
    {
        onStart?.Invoke();
        answerField.ShowReponse();
        textBox.ToggleBox(true);
        textBox.WriteText(questionText);
    }

    public void FinishQuestionLibre()
    {
        answerField.HideReponse();
        textBox.ToggleBox(false);
    }

    public void CompareAnswers(){
        currentTry++;
        if (enableJustePrix)
        {
            CompareNumberAnswer(float.Parse(answerField.GetAnswer()));
        }
        else
        {
            CompareStringAnswers(answerField.GetAnswer());
        }
    }

    void CompareStringAnswers(string ans)
    {
        int dist = DamerauLevenshtein.
        DamerauLevenshteinDistance(ans.Trim().ToLower(), answer.Trim().ToLower());
        if (dist <= differenceTolerance)
        {
            Debug.Log("GOOD, diff is " + dist);
            currentTry = 0;
            answerField.SetAnswer(answer);
            onCorrectAnswer?.Invoke();
            FinishQuestionLibre();
        }
        else
        {
            if (maxNumberOfTry <= currentTry)
            {

            }
            Debug.Log("BAD, diff is " + dist);
            onWrongAnswer?.Invoke();
        }
    }

    void CompareNumberAnswer(float ans)
    {
        float dist = ans - float.Parse(answer);

        if (dist < 0f)
        {
            justePrix.UpperIndication();
        }else if (dist == 0f)
        {
            Debug.Log("BIEN OUEJ");
            onCorrectAnswer?.Invoke();
            FinishQuestionLibre();
        }
        else
        {
            justePrix.LowerIndication();
        }
    }

    public void EndQuestion()
    {
        currentTry = 0;
    }
}