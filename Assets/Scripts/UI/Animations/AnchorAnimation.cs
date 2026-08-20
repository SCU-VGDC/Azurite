using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AnchorAnimation : MenuAnimation
{
    [Tooltip("The min anchor's final position.")]
    [SerializeField] private Vector2 minAnchorEnd = Vector2.zero;

    [Tooltip("The max anchor's final position.")]
    [SerializeField] private Vector2 maxAnchorEnd = Vector2.zero;

    protected override Tween CreateTween()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Sequence sequence = DOTween.Sequence().SetAutoKill(false).Pause();

        if (reverse)
        {
            _ = sequence.Join(DOTween.To((float x) => { rectTransform.anchorMin = new Vector2(x, rectTransform.anchorMin.y); }, minAnchorEnd.x, rectTransform.anchorMin.x, duration));
            _ = sequence.Join(DOTween.To((float y) => { rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, y); }, minAnchorEnd.y, rectTransform.anchorMin.y, duration));
            _ = sequence.Join(DOTween.To((float x) => { rectTransform.anchorMax = new Vector2(x, rectTransform.anchorMax.y); }, maxAnchorEnd.x, rectTransform.anchorMax.x, duration));
            _ = sequence.Join(DOTween.To((float y) => { rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, y); }, maxAnchorEnd.y, rectTransform.anchorMax.y, duration));

            rectTransform.anchorMin = minAnchorEnd;
            rectTransform.anchorMax = maxAnchorEnd;
        }
        else
        {
            _ = sequence.Join(DOTween.To((float x) => { rectTransform.anchorMin = new Vector2(x, rectTransform.anchorMin.y); }, rectTransform.anchorMin.x, minAnchorEnd.x, duration));
            _ = sequence.Join(DOTween.To((float y) => { rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, y); }, rectTransform.anchorMin.y, minAnchorEnd.y, duration));
            _ = sequence.Join(DOTween.To((float x) => { rectTransform.anchorMax = new Vector2(x, rectTransform.anchorMax.y); }, rectTransform.anchorMax.x, maxAnchorEnd.x, duration));
            _ = sequence.Join(DOTween.To((float y) => { rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, y); }, rectTransform.anchorMax.y, maxAnchorEnd.y, duration));
        }

        return sequence;
    }
}