using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Canvas))]
public class ScribeQuizPaper : MonoBehaviour
{
    [Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] answers;
        public int correctIndex;
        [NonSerialized] public int selectedIndex = -1;
    }

    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private VerticalLayoutGroup verticalLayout;
    [SerializeField] private QuizQuestion[] questions;
    [SerializeField] private Button prevButton, nextButton;

    private int currentQuestion;

    public bool IsCorrect
    {
        get
        {
            foreach (var question in questions)
            {
                if (question.selectedIndex != question.correctIndex)
                    return false;
            }
            return true;

        }
    }

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        prevButton.onClick.AddListener(() => TrySetQuestionIndex(currentQuestion - 1));
        nextButton.onClick.AddListener(() => TrySetQuestionIndex(currentQuestion + 1));
        TrySetQuestionIndex(0);
    }

    private bool TrySetQuestionIndex(int newIndex)
    {
        newIndex = Math.Clamp(newIndex, 0, questions.Length - 1);
        if (newIndex == currentQuestion)
            return false;

        currentQuestion = newIndex;
        prevButton.interactable = currentQuestion > 0;
        nextButton.interactable = currentQuestion < questions.Length - 1;
        DisplayCurrentQuestion();
        return true;
    }

    private void DisplayCurrentQuestion()
    {
        questionText.text = questions[currentQuestion].question;

        foreach (Transform child in verticalLayout.transform)
            Destroy(child.gameObject);

        var questionInfo = questions[currentQuestion];
        for (int i = 0; i < questionInfo.answers.Length; i++)
        {
            int qNumCopy = currentQuestion;
            int aNumCopy = i;
            TextMeshProUGUI text = Instantiate(answerPrefab, verticalLayout.transform).GetComponent<TextMeshProUGUI>();
            text.text = (char)(65 + i) + ") " + questionInfo.answers[i];
            text.GetComponent<Button>().onClick.AddListener(() => OnAnswer(qNumCopy, aNumCopy));
        }
    }

    private void OnAnswer(int questionNum, int answerNum)
    {
        questions[questionNum].selectedIndex = answerNum;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        var group = GetComponent<CanvasGroup>();
        group.DOFade(1f, 0.3f);
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    public void Hide()
    {
        var group = GetComponent<CanvasGroup>();
        DOTween.Sequence()
            .Append(group.DOFade(0f, 0.3f))
            .AppendCallback(() =>
            {
                group.interactable = false;
                group.blocksRaycasts = false;
                gameObject.SetActive(false);
            });
    }
}
