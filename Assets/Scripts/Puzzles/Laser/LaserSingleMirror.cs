using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LaserSingleMirror : LaserModifier
{
	[Tooltip("Whether or not the mirror is locked.")]
	[SerializeField] private bool isLocked = false;

	[Tooltip("The mirror's rotation.")]
	[SerializeField] private Direction rotation = Direction.NORTH;

	[Tooltip("The north facing sprite.")]
	[SerializeField] private Sprite northSprite = null;

	[Tooltip("The east facing sprite.")]
	[SerializeField] private Sprite eastSprite = null;

	[Tooltip("The south facing sprite.")]
	[SerializeField] private Sprite southSprite = null;

	[Tooltip("The west facing sprite.")]
	[SerializeField] private Sprite westSprite = null;

	[Tooltip("The north facing locked sprite.")]
	[SerializeField] private Sprite northLockedSprite = null;

	[Tooltip("The east facing locked sprite.")]
	[SerializeField] private Sprite eastLockedSprite = null;

	[Tooltip("The south facing locked sprite.")]
	[SerializeField] private Sprite southLockedSprite = null;

	[Tooltip("The west facing locked sprite.")]
	[SerializeField] private Sprite westLockedSprite = null;

	private SpriteRenderer spriteRenderer = null;

	void Awake()
	{
		this.spriteRenderer = GetComponent<SpriteRenderer>();
		this.transform.position = new Vector3(Mathf.Round(this.transform.position.x), Mathf.Round(this.transform.position.y), Mathf.Round(this.transform.position.z));
	}

	void OnMouseDown()
	{
		if(this.isLocked)
		{
			return;
		}

		switch(this.rotation)
		{
		case Direction.NORTH:
			this.rotation = Direction.EAST;
			this.spriteRenderer.sprite = this.isLocked ? this.eastLockedSprite : this.eastSprite;
			break;
		case Direction.EAST:
			this.rotation = Direction.SOUTH;
			this.spriteRenderer.sprite = this.isLocked ? this.southLockedSprite : this.southSprite;
			break;
		case Direction.SOUTH:
			this.rotation = Direction.WEST;
			this.spriteRenderer.sprite = this.isLocked ? this.westLockedSprite : this.westSprite;
			break;
		case Direction.WEST:
			this.rotation = Direction.NORTH;
			this.spriteRenderer.sprite = this.isLocked ? this.northLockedSprite : this.northSprite;
			break;
		}

		this.clickEvent.Invoke();
	}

	void OnValidate()
	{
		switch(this.rotation)
		{
		case Direction.NORTH:
			this.GetComponent<SpriteRenderer>().sprite = this.isLocked ? this.northLockedSprite : this.northSprite;
			break;
		case Direction.EAST:
			this.GetComponent<SpriteRenderer>().sprite = this.isLocked ? this.eastLockedSprite : this.eastSprite;
			break;
		case Direction.SOUTH:
			this.GetComponent<SpriteRenderer>().sprite = this.isLocked ? this.southLockedSprite : this.southSprite;
			break;
		case Direction.WEST:
			this.GetComponent<SpriteRenderer>().sprite = this.isLocked ? this.westLockedSprite : this.westSprite;
			break;
		}
	}

	public override Direction? GetOutput(Direction side)
	{
		switch(this.rotation)
		{
		case Direction.NORTH:
			if(side == Direction.NORTH)
			{
				return Direction.EAST;
			}
			else if(side == Direction.EAST)
			{
				return Direction.NORTH;
			}

			return null;
		case Direction.EAST:
			if(side == Direction.EAST)
			{
				return Direction.SOUTH;
			}
			else if(side == Direction.SOUTH)
			{
				return Direction.EAST;
			}

			return null;
		case Direction.SOUTH:
			if(side == Direction.SOUTH)
			{
				return Direction.WEST;
			}
			else if(side == Direction.WEST)
			{
				return Direction.SOUTH;
			}

			return null;
		case Direction.WEST:
			if(side == Direction.WEST)
			{
				return Direction.NORTH;
			}
			else if(side == Direction.NORTH)
			{
				return Direction.WEST;
			}

			return null;
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