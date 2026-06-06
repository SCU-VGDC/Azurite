using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LaserDoubleMirror : LaserModifier
{
	[Tooltip("Whether or not the mirror is locked.")]
	[SerializeField] private bool isLocked = false;

	[Tooltip("Whether or not the mirror is forwards (/). Defaults to backwards (\\).")]
	[SerializeField] private bool isForward = false;

	[Tooltip("The foward (/) sprite.")]
	[SerializeField] private Sprite forwardSprite = null;

	[Tooltip("The backward (\\) sprite.")]
	[SerializeField] private Sprite backwardSprite = null;

	[Tooltip("The foward (/) locked sprite.")]
	[SerializeField] private Sprite forwardLockedSprite = null;

	[Tooltip("The backward (\\) locked sprite.")]
	[SerializeField] private Sprite backwardLockedSprite = null;

	private SpriteRenderer spriteRenderer = null;

	void Awake()
	{
		this.spriteRenderer = this.GetComponent<SpriteRenderer>();
		this.transform.position = new Vector3(Mathf.Round(this.transform.position.x), Mathf.Round(this.transform.position.y), Mathf.Round(this.transform.position.z));
	}

	void OnMouseDown()
	{
		if(this.isLocked)
		{
			return;
		}

		this.isForward = !this.isForward;
		this.spriteRenderer.sprite = this.isForward ? this.isLocked ? this.forwardLockedSprite : this.forwardSprite : this.isLocked ? this.backwardLockedSprite : this.backwardSprite;
		this.clickEvent.Invoke();
	}

	void OnValidate()
	{
		this.GetComponent<SpriteRenderer>().sprite = this.isForward ? this.isLocked ? this.forwardLockedSprite : this.forwardSprite : this.isLocked ? this.backwardLockedSprite : this.backwardSprite;
	}

	public override Direction? GetOutput(Direction side)
	{
		switch(side)
		{
		case Direction.NORTH:
			return this.isForward ? Direction.WEST : Direction.EAST;
		case Direction.EAST:
			return this.isForward ? Direction.SOUTH : Direction.NORTH;
		case Direction.SOUTH:
			return this.isForward ? Direction.EAST : Direction.WEST;
		case Direction.WEST:
			return this.isForward ? Direction.NORTH : Direction.SOUTH;
		}

		return null;
	}

	public override void Hit(Direction side)
	{
	}

	public override void Reset()
	{
	}
}