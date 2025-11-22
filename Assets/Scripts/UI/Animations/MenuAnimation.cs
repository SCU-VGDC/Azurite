using UnityEngine;
using UnityEngine.Events;

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

	[Tooltip("The movement interpolation function to use.")]
	[SerializeField] protected MovementType movementType = MovementType.LINEAR;

	[Tooltip("The animation scale. The animation will progress X times as quickly.")]
	[SerializeField] protected float animationScale = 8f;

	protected float progress = 0f;
	protected float directionScale = 1f;

	public virtual void Update()
	{
		this.progress += Time.deltaTime * this.animationScale * this.directionScale;

		if(this.directionScale == 1f && this.progress >= 1f)
		{
			this.progress = 1f;
			return;
		}
		
		if(this.directionScale == -1f && this.progress <= 0f)
		{
			this.progress = 0f;
			return;
		}
	}

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

	public void Play()
	{
		this.directionScale = 1f;
	}

	public void Rewind()
	{
		this.directionScale = -1f;
	}

	public void Stop()
	{
		this.directionScale = 0f;
	}

	public float GetProgress()
	{
		return this.progress;
	}

	public float GetDirection()
	{
		return this.directionScale;
	}
}