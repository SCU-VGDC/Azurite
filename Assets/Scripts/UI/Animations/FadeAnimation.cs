using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class FadeAnimation : MenuAnimation
{
	[Tooltip("The ending alpha value.")]
	[SerializeField] float endAlpha = 0f;

	protected override Tween CreateTween()
	{
		if(this.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
		{
			if(this.reverse)
			{
				Tween tween = DOTween.To((value) => {canvasGroup.alpha = value;}, this.endAlpha, canvasGroup.alpha, this.duration).SetAutoKill(false).Pause();
				canvasGroup.alpha = this.endAlpha;
				return tween;
			}

			return DOTween.To((value) => {canvasGroup.alpha = value;}, canvasGroup.alpha, this.endAlpha, this.duration).SetAutoKill(false).Pause();
		}
		else if(this.TryGetComponent<CanvasRenderer>(out CanvasRenderer canvasRenderer))
		{
			if(this.reverse)
			{
				Tween tween = DOTween.To(canvasRenderer.SetAlpha, this.endAlpha, canvasRenderer.GetAlpha(), this.duration).SetAutoKill(false).Pause();
				canvasRenderer.SetAlpha(this.endAlpha);
				return tween;
			}

			return DOTween.To(canvasRenderer.SetAlpha, canvasRenderer.GetAlpha(), this.endAlpha, this.duration).SetAutoKill(false).Pause();
		}
		else
		{
			return DOTween.To(() => {return 0f;}, (value) => {}, 0f, 0f).SetEase(this.interpolation).SetAutoKill(false).Pause();
		}
	}
}