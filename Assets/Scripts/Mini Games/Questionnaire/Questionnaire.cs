using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Questionnaire : Minigame
{
    [SerializeField] bool hideAllQuestionAtStart;
    [SerializeField]
    List<Question> questions;
    public QuestionnaireStyle style;
    [SerializeField] TextBox nextButton;
    Queue<Question> questionsQueue = new Queue<Question>();
    bool _questionCompleted;
    int _numberOfCorrectAnswers = 0;
    Question _lastQuestion;

    [Tooltip("(EN %)pourcentage de bonne réponse a avoir pour déclencher la bonne réplique")]
    [SerializeField] float moyenne;
    [SerializeField] UnityEvent onBelowAverage = new UnityEvent();
    [SerializeField] UnityEvent onAboveAverage = new UnityEvent();
    [Header("Configuration")]
    [SerializeField] GameObject smokeVFX;
    [SerializeField] TextBoxContainerTemplate template;
    public bool configurating {get; private set;}
    

    void Start()
    {
        if (questions == null || questions.Count == 0) questions = GetComponentsInChildren<Question>().ToList();

        if (hideAllQuestionAtStart)
        {
            foreach (var question in questions)
            {
                question.Hide(true);
                question.questionnaire = this;
            }
        }

        ResetQuestionnaire();
    }

    public override void StartMiniGame()
    {
        ResetQuestionnaire();
        AskNextQuestion();
    }

    void EndQuestions()
    {
        foreach (var question in questions)
        {
            question.Hide();
        }

        float perc = _numberOfCorrectAnswers / questions.Count;
        if (perc*100f > moyenne){
            onAboveAverage?.Invoke();
        }else{
            onBelowAverage?.Invoke();
        }

        EndMiniGame();
    }

    void ShowSkipButton(bool toggle)
    {
        nextButton.ToggleBox(toggle);
    }

    void OnQuestionAnswered(Question q, bool correct)
    {
        q.onQuestionComplete.RemoveListener(OnQuestionAnswered);
        _lastQuestion = q;
        if (correct) _numberOfCorrectAnswers++;
        _questionCompleted = true;
        ShowSkipButton(true);
    }

    public void GoToNextQuest()
    {
        if (!_questionCompleted) return;
        if (questionsQueue.Count > 0)
        {
            _lastQuestion.Hide();
            Invoke(nameof(AskNextQuestion), 2f);
            //Debug.Log("Question answered and asking next question");
            //AskNextQuestion();
        }
        else
        {
            //Debug.Log("Finishing questionnaire");
            EndQuestions();
        }
        _questionCompleted = false;
        ShowSkipButton(false);
    }

    void AskNextQuestion()
    {
        //Debug.Log("Asking next question");
        Question question = questionsQueue.Dequeue();
        question.onQuestionComplete.AddListener(OnQuestionAnswered);
        question.AskQuestion();
    }

    public void ResetQuestionnaire()
    {
        questionsQueue.Clear();
        _numberOfCorrectAnswers = 0;
        foreach (var question in questions)
        {
            questionsQueue.Enqueue(question);
            question.ResetAllReponses();
        }
    }
    #region EDITOR FUNCTIONS

    public void GetAllQuestions(){
        if (configurating) return;
        questions = GetComponentsInChildren<Question>().ToList();
        foreach(Question question in questions){
            question.GetAllReps();
        }
    }

    public void ClearQuestions(){
        if (configurating) return;
        questions.Clear();
    }

    public void UpdateVFXs(){
        foreach(Question question in questions){
            question.NestedUpdateVFX(smokeVFX);
        }
    }

    public void UpdateQPosFromTempalte(){
        if (!configurating) return;
        foreach (Question question in questions)
        {
            question.gameObject.SetActive(true);
            template.UpdateContainer(question);
            question.gameObject.SetActive(false);
        }
    }

    public void EnterConfiguration(){
        configurating=true;
        foreach(Question question in questions){
            question.gameObject.SetActive(false);
        }

        template.gameObject.SetActive(true);
    }

    public void ExitConfiguration(){
        configurating=false;
        foreach(Question question in questions){
            question.gameObject.SetActive(true);
        }

        template.gameObject.SetActive(false);
    }

    #endregion
}
