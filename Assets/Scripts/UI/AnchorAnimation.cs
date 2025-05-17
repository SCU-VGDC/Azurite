using UnityEngine;

public class AnchorAnimation : MenuAnimation
{
	/// <summary>The starting min anchor value to use when playing the animation.</summary>
	[Tooltip("The starting min anchor value to use when playing the animation.")]
	[SerializeField] private Vector2 startMinAnchor = Vector2.zero;

	/// <summary>The starting max anchor value to use when playing the animation.</summary>
	[Tooltip("The starting max anchor value to use when playing the animation.")]
	[SerializeField] private Vector2 startMaxAnchor = Vector2.zero;

	/// <summary>The final min anchor value to use when playing the animation.</summary>
	private Vector2 endMinAnchor = Vector2.zero;

	/// <summary>The final max anchor value to use when playing the animation.</summary>
	private Vector2 endMaxAnchor = Vector2.zero;

	public void Start()
	{
		this.endMinAnchor = ((RectTransform) this.transform).anchorMin;
		this.endMaxAnchor = ((RectTransform) this.transform).anchorMax;
	}

	public override void Update()
	{
		if(this.progress == this.endPos)
		{
			return;
		}
		
		base.Update();

		// Set the min and max anchors to the interpolated position.
		float position = this.CalculatePosition();
		((RectTransform) this.transform).anchorMin = position * this.endMinAnchor + (1f - position) * this.startMinAnchor;
		((RectTransform) this.transform).anchorMax = position * this.endMaxAnchor + (1f - position) * this.startMaxAnchor;
	}
}