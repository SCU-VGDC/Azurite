using UnityEngine;

[RequireComponent(typeof(CanvasRenderer))]
public class FadeAnimation : MenuAnimation
{
	/// <summary>The starting alpha value to use when playing the animation.</summary>
	[Tooltip("The starting alpha value to use when playing the animation.")]
	[SerializeField] float startAlpha = 0f;

	/// <summary>The final alpha value to use when playing the animation.</summary>
	private float endAlpha = 0f;

	/// <summary>The canvas renderer to adjust the alpha value of.</summary>
	private CanvasRenderer uiRenderer = null;

	public void Start()
	{
		this.uiRenderer = this.GetComponent<CanvasRenderer>();
		this.endAlpha = this.uiRenderer.GetAlpha();
	}

	public override void Update()
	{
		if(this.progress == this.endPos)
		{
			return;
		}
		
		base.Update();

		// Set the alpha to the interpolated progress.
		float position = this.CalculatePosition();
		this.uiRenderer.SetAlpha(position * this.endAlpha + (1f - position) * this.startAlpha);
	}
}