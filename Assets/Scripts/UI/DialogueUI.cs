using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Dialogue.Data;

public class DialogueUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public CanvasGroup choiceListCanvasGroup;
    public GameObject choiceTextButtonPrefab;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI subjectText;
    private Sequence currentFadeSequence;
    private bool choiceAvailable = false;

    public string CurrentChoice { get; private set; } = string.Empty;
    
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0;
    }


    public void DisplayText(string text, string subjectName, List<DialogueChoice> choices = null)
    {
        CurrentChoice = string.Empty;
        choiceAvailable = choices != null && choices.Count > 0;

        currentFadeSequence?.Kill();
        currentFadeSequence = DOTween.Sequence()
            .Append(mainText.DOFade(0f, 0.1f).SetEase(Ease.InQuad))
            .Join(subjectText.DOFade(0f, 0.1f).SetEase(Ease.InQuad))
            .Join(canvasGroup.DOFade(0f, 0.1f).SetEase(Ease.InQuad))
            .Join(choiceListCanvasGroup.DOFade(0f, 0.1f).SetEase(Ease.InQuad))
            .AppendCallback(() =>
                {
                    mainText.text = text;
                    subjectText.text = subjectName;
                    ClearChoiceButtons();
                    if (choiceAvailable)
                    {
                        foreach (var choice in choices)
                        {
                            GameObject newButton = Instantiate(choiceTextButtonPrefab);
                            newButton.GetComponent<TextMeshProUGUI>().text = "► " + choice.choiceText;
                            newButton.transform.SetParent(choiceListCanvasGroup.transform, false);
                            //newButton.GetComponent<Button>().onClick.AddListener(() => CurrentChoice = choice);
                            newButton.GetComponent<Button>().onClick.AddListener(() =>{CurrentChoice = choice.choiceText; choice.choiceCallback.Invoke(choice.choiceText); foreach (var btn in choiceListCanvasGroup.GetComponentsInChildren<Button>())
                                {
                                    btn.onClick.RemoveAllListeners();
                                }
                            });
                        }
                    }
                }
            )
            .Append(mainText.DOFade(1f, 0.2f).SetEase(Ease.OutQuad))
            .Join(subjectText.DOFade(1f, 0.2f).SetEase(Ease.OutQuad))
            .Join(canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.OutQuad));

        if (choiceAvailable)
        {
            currentFadeSequence.Join(choiceListCanvasGroup.DOFade(1f, 0.2f).SetEase(Ease.OutQuad));
        }
    }

    private void ClearChoiceButtons()
    {
        foreach (Transform child in choiceListCanvasGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void FadeGroup(float alpha)
    {
        currentFadeSequence?.Kill();
        currentFadeSequence = DOTween.Sequence()
            .Append(canvasGroup.DOFade(alpha, 0.2f).SetEase(Ease.OutQuad));
    }

    public void FadeIn()
    {
        FadeGroup(1f);
    }

    public void FadeOut()
    {
        FadeGroup(0f);
        currentFadeSequence.AppendCallback(ClearChoiceButtons);
    }

    public IEnumerator WaitForPlayerChoice()
    {
        if (choiceAvailable)
            yield return new WaitUntil(() => CurrentChoice != string.Empty);
        else
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    }
}
