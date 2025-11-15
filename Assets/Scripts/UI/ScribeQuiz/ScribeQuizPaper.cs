using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ScribeQuizPaper : MonoBehaviour
{
    public class QuizQuestion
    {
        public string question;
        public string[] answers;
        public int correctIndex;
    }

    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private VerticalLayoutGroup verticalLayout;
    [SerializeField] private ScribeInteraction scribe;
    [SerializeField] private QuizQuestion[] questions;

    private int currentQuestion;

    private void SetupQuestionDisplay()
    {
        foreach (Transform child in verticalLayout.transform)
            if (child.gameObject != questionText.gameObject)
                Destroy(child.gameObject);

        var questionInfo = questions[currentQuestion];
        for (int i = 0; i < questionInfo.answers.Length; i++)
        {
            TextMeshProUGUI text = Instantiate(answerPrefab, verticalLayout.transform).GetComponent<TextMeshProUGUI>();
            text.text = (char)(65 + i) + ") " + questionInfo.answers[i];
            text.GetComponent<Button>().onClick.AddListener(() => OnAnswer(i));
        }
    }

    private void OnAnswer(int answerNum)
    {
        foreach (Transform child in verticalLayout.transform)
            if (child.TryGetComponent(out Button button))
                button.onClick.RemoveAllListeners();
        if (answerNum == questions[currentQuestion].correctIndex)
        {
            OnCorrect();
        }
        else
        {
            OnIncorrect();
        }
    }

    private void OnCorrect()
    {
        currentQuestion++;
        if (currentQuestion == questions.Length)
        {

        }
        else
        {
            
        }
    }

    private void OnIncorrect()
    {
        currentQuestion = 0;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
    }

    public void Hide()
    {
        DOTween.Sequence()
            .Append(GetComponent<CanvasGroup>().DOFade(0f, 0.3f))
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
