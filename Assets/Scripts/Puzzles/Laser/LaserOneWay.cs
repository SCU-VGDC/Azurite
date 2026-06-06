using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LaserOneWay : LaserModifier
{
	[Tooltip("Whether or not the gate is locked.")]
	[SerializeField] private bool isLocked = false;

	[Tooltip("The gate's rotation.")]
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

	[Tooltip("The laser beam prefab.")]
	[SerializeField] private LaserBeam laserPrefab = null;

	private SpriteRenderer spriteRenderer = null;
	private LaserBeam fakeBeam = null;

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
		switch(side)
		{
		case Direction.NORTH:
			return this.rotation == Direction.SOUTH ? Direction.SOUTH : null;
		case Direction.EAST:
			return this.rotation == Direction.WEST ? Direction.WEST : null;
		case Direction.SOUTH:
			return this.rotation == Direction.NORTH ? Direction.NORTH : null;
		case Direction.WEST:
			return this.rotation == Direction.EAST ? Direction.EAST : null;
		}

		return null;
	}

	public override void Hit(Direction side)
	{
		if(((this.rotation == Direction.NORTH || this.rotation == Direction.SOUTH) && (side == Direction.EAST || side == Direction.WEST)) || ((this.rotation == Direction.EAST || this.rotation == Direction.WEST) && (side == Direction.NORTH || side == Direction.SOUTH)))
		{
			this.fakeBeam = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity, this.transform).SetConnection(side, true);
		}
	}

	public override void Reset()
	{
		if(this.fakeBeam != null)
		{
			Destroy(this.fakeBeam.gameObject);
			this.fakeBeam = null;
		}
	}
}