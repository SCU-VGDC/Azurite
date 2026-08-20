using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class DialogOptionButton : MonoBehaviour
{
    private Button button = null;
    private TextMeshProUGUI text = null;
    private AnimatedArrowIcon arrow = null;
    private Tween tween;

    private void Start()
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

    public void OnHover(bool active)
    {
        text.fontStyle = active ? FontStyles.Underline : FontStyles.Normal;
        if (active) arrow.Show(); else arrow.Hide();

        var layout = GetComponent<HorizontalLayoutGroup>();
        tween?.Kill();
        tween = DOTween.To(() => layout.padding.left, x => layout.padding.left = x, active ? 75 : 0, 0.3f);
    }
}