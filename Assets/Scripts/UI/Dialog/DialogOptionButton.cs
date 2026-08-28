using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class DialogOptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button = null;
    private TextMeshProUGUI text = null;
    private AnimatedArrowIcon arrow = null;
    private Tween tween;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        arrow = GetComponentInChildren<AnimatedArrowIcon>();
    }

    public DialogOptionButton Init(Dialog dialog, DialogStep entry)
    {
        button.onClick.AddListener(() => dialog.CurrentStep = entry);
        text.text = entry.name;
        return this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Underline;
        arrow.Show();

        var layout = GetComponent<HorizontalLayoutGroup>();
        tween?.Kill();
        tween = DOTween.To(() => layout.padding.left, x => layout.padding.left = x, 75, 0.3f).SetEase(Ease.OutCubic);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Normal;
        arrow.Hide();

        var layout = GetComponent<HorizontalLayoutGroup>();
        tween?.Kill();
        tween = DOTween.To(() => layout.padding.left, x => layout.padding.left = x, 0, 0.3f).SetEase(Ease.InCubic);
    }
}