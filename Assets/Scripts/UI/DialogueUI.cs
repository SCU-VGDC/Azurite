using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI subjectText;
    private Sequence currentFadeSequence;

    public string CurrentChoice { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void DisplayText(string text, string subjectName)
    {
        currentFadeSequence?.Kill();
        currentFadeSequence = DOTween.Sequence()
            .Append(mainText.DOFade(0f, 0.1f).SetEase(Ease.InQuad))
            .Join(subjectText.DOFade(0f, 0.1f).SetEase(Ease.InQuad))
            .Join(canvasGroup.DOFade(0f, 0.1f).SetEase(Ease.InQuad))
            .AppendCallback(() =>
                {
                    mainText.text = text;
                    subjectText.text = subjectName;
                }
            )
            .Append(mainText.DOFade(1f, 0.2f).SetEase(Ease.OutQuad))
            .Join(subjectText.DOFade(1f, 0.2f).SetEase(Ease.OutQuad))
            .Join(canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.OutQuad));
    }

    private void FadeGroup(float alpha)
    {
        canvasGroup.DOFade(alpha, 0.2f).SetEase(Ease.OutQuad);
    }

    public void FadeIn()
    {
        FadeGroup(1f);
    }

    public void FadeOut()
    {
        FadeGroup(0f);
    }

    public IEnumerator WaitForPlayerChoice()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
    }
}
