using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LaserTunnel : LaserModifier
{
	[Tooltip("Whether or not the tunnel is locked.")]
	[SerializeField] private bool isLocked = false;

	[Tooltip("Whether or not the tunnel is horizontal. Defaults to vertical.")]
	[SerializeField] private bool isHorizontal = false;

	[Tooltip("The horizontal sprite.")]
	[SerializeField] private Sprite horizontalSprite = null;

	[Tooltip("The vertical sprite.")]
	[SerializeField] private Sprite verticalSprite = null;

	[Tooltip("The horizontal locked sprite.")]
	[SerializeField] private Sprite horizontalLockedSprite = null;

	[Tooltip("The vertical locked sprite.")]
	[SerializeField] private Sprite verticalLockedSprite = null;

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

		this.isHorizontal = !this.isHorizontal;
		this.spriteRenderer.sprite = this.isHorizontal ? this.isLocked ? this.horizontalLockedSprite : this.horizontalSprite : this.isLocked ? this.verticalLockedSprite : this.verticalSprite;

		this.clickEvent.Invoke();
	}

	void OnValidate()
	{
		this.GetComponent<SpriteRenderer>().sprite = this.isHorizontal ? this.isLocked ? this.horizontalLockedSprite : this.horizontalSprite : this.isLocked ? this.verticalLockedSprite : this.verticalSprite;
	}

	public override Direction? GetOutput(Direction side)
	{
		switch(side)
		{
		case Direction.NORTH:
			return this.isHorizontal ? null : Direction.SOUTH;
		case Direction.EAST:
			return this.isHorizontal ? Direction.WEST : null;
		case Direction.SOUTH:
			return this.isHorizontal ? null : Direction.NORTH;
		case Direction.WEST:
			return this.isHorizontal ? Direction.EAST : null;
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