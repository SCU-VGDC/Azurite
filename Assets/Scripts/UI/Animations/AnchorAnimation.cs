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
		RectTransform rectTransform = this.GetComponent<RectTransform>();
		Sequence sequence = DOTween.Sequence().SetAutoKill(false).Pause();

		if(this.reverse)
		{
			sequence.Join(DOTween.To((float x) => {rectTransform.anchorMin = new Vector2(x, rectTransform.anchorMin.y);}, this.minAnchorEnd.x, rectTransform.anchorMin.x, this.duration));
			sequence.Join(DOTween.To((float y) => {rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, y);}, this.minAnchorEnd.y, rectTransform.anchorMin.y, this.duration));
			sequence.Join(DOTween.To((float x) => {rectTransform.anchorMax = new Vector2(x, rectTransform.anchorMax.y);}, this.maxAnchorEnd.x, rectTransform.anchorMax.x, this.duration));
			sequence.Join(DOTween.To((float y) => {rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, y);}, this.maxAnchorEnd.y, rectTransform.anchorMax.y, this.duration));

			rectTransform.anchorMin = this.minAnchorEnd;
			rectTransform.anchorMax = this.maxAnchorEnd;
		}
		else
		{
			sequence.Join(DOTween.To((float x) => {rectTransform.anchorMin = new Vector2(x, rectTransform.anchorMin.y);}, rectTransform.anchorMin.x, this.minAnchorEnd.x, this.duration));
			sequence.Join(DOTween.To((float y) => {rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, y);}, rectTransform.anchorMin.y, this.minAnchorEnd.y, this.duration));
			sequence.Join(DOTween.To((float x) => {rectTransform.anchorMax = new Vector2(x, rectTransform.anchorMax.y);}, rectTransform.anchorMax.x, this.maxAnchorEnd.x, this.duration));
			sequence.Join(DOTween.To((float y) => {rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, y);}, rectTransform.anchorMax.y, this.maxAnchorEnd.y, this.duration));
		}
		
		return sequence;
	}
}