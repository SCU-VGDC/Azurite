using DG.Tweening;
using UnityEngine;

[HideInInspector]
public abstract class MenuAnimation : MonoBehaviour
{
	[Tooltip("The movement interpolation function to use.")]
	[SerializeField] protected Ease interpolation = Ease.Linear;

	[Tooltip("The duration of the animation in seconds.")]
	[SerializeField] protected float duration = 0.125f;

	[Tooltip("Whether or not the animation should play in reverse.")]
	[SerializeField] protected bool reverse = false;

	protected Tween tween = null;

	protected abstract Tween CreateTween();

	public Tween GetTween()
	{
		if(this.tween == null)
		{
			this.tween = this.CreateTween();
		}

		return this.tween;
	}

	public void OnDestroy()
	{
		if(this.tween != null)
		{
			this.tween.Kill();
		}
	}
}