using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ScribeQuizPaper : MonoBehaviour
{
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private VerticalLayoutGroup verticalLayout;
    [SerializeField] private ScribeInteraction scribe;
    public int CorrectAnswerIndex { get; private set; }

    public void SetQuestion(ScribeInteraction.QuestionInfo questionInfo)
    {
        CorrectAnswerIndex = questionInfo.correctIndex;
        foreach (Transform child in verticalLayout.transform)
            if (child.gameObject != questionText.gameObject)
                Destroy(child.gameObject);

        for (int i = 0; i < questionInfo.answers.Count; i++)
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
        if (answerNum == CorrectAnswerIndex)
            scribe.OnCorrect();
        else
            scribe.OnIncorrect();
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
