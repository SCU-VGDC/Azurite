using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class DialogOptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform textContainer;
    [SerializeField] private TextMeshProUGUI text;
    private Button button = null;
    private AnimatedArrowIcon arrow = null;
    private float paddingLeftStart;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        arrow = GetComponentInChildren<AnimatedArrowIcon>();
    }

    private void OnDestroy()
    {
        textContainer.DOKill();
    }

    public DialogOptionButton Init(Dialog dialog, DialogStep entry)
    {
        button.onClick.AddListener(() => dialog.CurrentStep = entry);
        text.text = entry.name;
        paddingLeftStart = textContainer.anchoredPosition.x;
        textContainer.anchoredPosition = Vector2.zero;
        return this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Underline;
        arrow.Show();
        textContainer.DOAnchorPos(new Vector2(paddingLeftStart, 0), 0.15f).SetEase(Ease.OutCubic);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Normal;
        arrow.Hide();
        textContainer.DOAnchorPos(Vector2.zero, 0.15f).SetEase(Ease.InCubic);
    }
}