using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueSequence))]
[RequireComponent(typeof(InteractionTrigger))]
public class ScribeInteraction : MonoBehaviour
{
    public struct QuestionInfo
    {
        public string question;
        public List<string> answers;
        public int correctIndex;
    }

    private DialogueSequence dialogue;
    [SerializeField] private DialogueHolder failSequence;
    [SerializeField] private DialogueHolder successSequence;
    [SerializeField] private ScribeQuizPaper quizPaper;
    private int currentQuestion = -1;
    private Func<QuestionInfo>[] questionGenerators;

    void Awake()
    {
        dialogue = GetComponent<DialogueSequence>();
        questionGenerators = new Func<QuestionInfo>[] {
            GetBedQuestion
        };
    }

    public void OnInteract()
    {
        dialogue.DialogueStart();
    }

    public void ShowQuestionsUI(string playerChoice)
    {
        if (playerChoice != "Yes") return;
        quizPaper.Show();
        currentQuestion = -1;
        DoNextQuestion();
    }

    private void DoNextQuestion()
    {
        currentQuestion++;
        Debug.Log($"Question {currentQuestion}");
        quizPaper.SetQuestion(questionGenerators[currentQuestion]());
    }

    private void OnQuizSuccess()
    {
        dialogue.SetDialogueSteps(successSequence.ReturnList());
        dialogue.DialogueStart();
    }

    public void OnIncorrect()
    {
        currentQuestion = -1;
        dialogue.SetDialogueSteps(failSequence.ReturnList());
        dialogue.DialogueStart();
        quizPaper.Hide();
    }

    public void OnCorrect()
    {
        if (currentQuestion < questionGenerators.Length - 1)
            DoNextQuestion();
        else
            OnQuizSuccess();
    }

    private QuestionInfo GetBedQuestion()
    {
        return new QuestionInfo
        {
            question = "I want to remember how cramped these living quarters are. How many bunk beds are in this room?",
            answers = new(){
                "1",
                "2",
                "4",
                "5"
            },
            correctIndex = 3
        };
    }

    private QuestionInfo GetFlowersQuestion()
    {
        return new QuestionInfo
        {

        };
    }
}
