using UnityEngine;

[HideInInspector]
public class MenuAnimation : MonoBehaviour
{
	public enum MovementType
	{
		LINEAR,
		SMOOTHSTEP,
		SMOOTHERSTEP,
		ARC
	}

	/// <summary>The movement interpolation function to use.</summary>
	[Tooltip("The movement interpolation function to use.")]
	[SerializeField] protected MovementType movementType = MovementType.LINEAR;

	/// <summary>The animation scale. The animation will progress x times as quickly.</summary>
	[Tooltip("The animation scale. The animation will progress x times as quickly.")]
	[SerializeField] protected float animationScale = 8f;

	/// <summary>The animations progress from 0 to 1.</summary>
	protected float progress = 0f;

	/// <summary>The starting position of the animation. 0 means play, 1 means rewind.</summary>
	protected float startPos = 0f;

	/// <summary>The ending position of the animation. 1 means play, 0 means rewind.</summary>
	protected float endPos = 1f;

	public virtual void Update()
	{
		// Change the progress by delta time and clamp from 0 to 1.
		if((this.startPos < this.endPos && (this.progress += Time.deltaTime * this.animationScale) > this.endPos) || (this.startPos > this.endPos && (this.progress -= Time.deltaTime * this.animationScale) < this.endPos))
		{
			this.progress = this.endPos;
		}
	}

	/// <summary>
	/// Use an interpolate the animations progress into a position from 0 to 1.
	/// </summary>
	/// <returns>The interpolated value.</returns>
	protected float CalculatePosition()
	{
		switch(this.movementType)
		{
		case MovementType.SMOOTHSTEP:
			return this.progress * this.progress * (3f - 2f * this.progress);
		case MovementType.SMOOTHERSTEP:
			return this.progress * this.progress * this.progress * (this.progress * (6f * this.progress - 15f) + 10f);
		case MovementType.ARC:
			return Mathf.Sqrt(this.progress * (2f - this.progress));
		default:
			return this.progress;
		}
	}

	/// <summary>
	/// Play the animation.
	/// </summary>
	public void Play()
	{
		this.startPos = 0f;
		this.endPos = 1f;
	}

	/// <summary>
	/// Play the animation in reverse.
	/// </summary>
	public void Rewind()
	{
		this.startPos = 1f;
		this.endPos = 0f;
	}

	/// <summary>
	/// Check if the animation has finished playing.
	/// </summary>
	/// <returns>True only if the animation has finished playing. This will be false if the animation is currently playing or if it is rewinding/rewound.</returns>
	public bool HasPlayed()
	{
		return this.startPos == 0f && this.progress == 1f;
	}

	/// <summary>
	/// Check if the animation has finished rewinding.
	/// </summary>
	/// <returns>True only if the animation has finished rewinding. This will be false if the animation is currently rewinding or if it is playing/played.</returns>
	public bool HasRewound()
	{
		return this.startPos == 1f && this.progress == 0f;
	}
	
	/// <summary>
	/// Check if the animation is playing.
	/// </summary>
	/// <returns>True only if the animation is playing. This will be false if the animation has finished playing or if it is rewinding/rewound.</returns>
	public bool IsPlaying()
	{
		return this.startPos == 0f && this.progress != 1f;
	}

	/// <summary>
	/// Check if the animation is rewiding.
	/// </summary>
	/// <returns>True only if the animation is playing. This will be false if the animation has finished rewinding or if it is playing/played.</returns>
	public bool IsRewinding()
	{
		return this.startPos == 1f && this.progress != 0f;
	}
}