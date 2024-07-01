using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Question : TextBoxContainer
{
    [TextArea]
    [SerializeField] string questionText;

    [SerializeField]
    List<Reponse> reponses;
    [SerializeField] int correctReponseIndex;
    Reponse selectedAnswer;
    [System.NonSerialized]
    public Questionnaire questionnaire;

    [System.NonSerialized]
    public UnityEvent<Question, bool> onQuestionComplete = new UnityEvent<Question, bool>();

    public UnityEvent onCorrectAnswer = new UnityEvent();
    public UnityEvent onWrongAnswer = new UnityEvent();

    public void AskQuestion()
    {
        textBox.ToggleBox(true);
        textBox.WriteText(questionText);
        textBox.onTextTyped.AddListener(DisplayAnswers);
    }

    void DisplayAnswers()
    {
        foreach (var reponse in reponses)
        {
            reponse.Display(this);
        }
    }

    public void OnAnswerSelect(Reponse rep)
    {
        if (selectedAnswer)
        {
            if (rep == selectedAnswer)
            {
                //Debug.Log("Answer is getting validated");
                AnswerQuestion();
            }
            else
            {

                //Debug.Log("Changing selection");
                selectedAnswer = rep;
                rep.Shake();
                UpdateSelectedAnswers();
            }
        }
        else
        {
            //Debug.Log("First time selecting answer");
            selectedAnswer = rep;
            rep.Shake();
            UpdateSelectedAnswers();
        }
    }

    public void ResetAllReponses(){
        selectedAnswer = null;
        foreach(var rep in reponses){
            rep.Reset();
        }
    }

    void UpdateSelectedAnswers()
    {
        foreach (var rep in reponses)
        {
            rep.VisualSelect(rep == selectedAnswer);
        }
    }

    public void AnswerQuestion()
    {
        DisplayCorrectAnswer();
        int ind = 0;
        for (int i = 0; i < reponses.Count; i++)
        {
            if (selectedAnswer == reponses[i]) ind = i;
        }
        bool correct = ind == correctReponseIndex;
        onQuestionComplete?.Invoke(this, correct);
        if (correct)
        {
            onCorrectAnswer?.Invoke();
        }
        else
        {
            onWrongAnswer?.Invoke();
        }
    }

    public void DisplayCorrectAnswer()
    {
        for (int i = 0; i < reponses.Count; i++)
        {
            reponses[i].IndicateAnswer(i == correctReponseIndex);
        }
    }

    public void Hide(bool instant = false)
    {
        foreach (var rep in reponses)
        {
            rep.Hide(instant);
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

    public void GetAllReps()
    {
        reponses = GetComponentsInChildren<Reponse>().ToList();
    }

    public void NestedUpdateVFX(GameObject vfx)
    {
        foreach (var rep in reponses)
        {
            rep.UpdateSmokeVFX(vfx);
        }

        UpdateSmokeVFX(vfx);
    }
}
