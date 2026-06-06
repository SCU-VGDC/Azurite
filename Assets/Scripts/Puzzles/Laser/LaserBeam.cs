using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LaserBeam : MonoBehaviour
{
	[Tooltip("The sprite representing no connections.")]
	[SerializeField] private Sprite spriteN0S0E0W0 = null;

	[Tooltip("The sprite representing a west connection.")]
	[SerializeField] private Sprite spriteN0S0E0W1 = null;

	[Tooltip("The sprite representing an east connection.")]
	[SerializeField] private Sprite spriteN0S0E1W0 = null;

	[Tooltip("The sprite representing east and west connection.")]
	[SerializeField] private Sprite spriteN0S0E1W1 = null;

	[Tooltip("The sprite representing a south connection.")]
	[SerializeField] private Sprite spriteN0S1E0W0 = null;

	[Tooltip("The sprite representing a south and west connection.")]
	[SerializeField] private Sprite spriteN0S1E0W1 = null;

	[Tooltip("The sprite representing a south and east connection.")]
	[SerializeField] private Sprite spriteN0S1E1W0 = null;

	[Tooltip("The sprite representing a south, east, and west connection.")]
	[SerializeField] private Sprite spriteN0S1E1W1 = null;

	[Tooltip("The sprite representing a north connection.")]
	[SerializeField] private Sprite spriteN1S0E0W0 = null;

	[Tooltip("The sprite representing a north and west connection.")]
	[SerializeField] private Sprite spriteN1S0E0W1 = null;

	[Tooltip("The sprite representing a north and east connection.")]
	[SerializeField] private Sprite spriteN1S0E1W0 = null;

	[Tooltip("The sprite representing a north, east, and west connection.")]
	[SerializeField] private Sprite spriteN1S0E1W1 = null;

	[Tooltip("The sprite representing a north and south connection.")]
	[SerializeField] private Sprite spriteN1S1E0W0 = null;

	[Tooltip("The sprite representing a north, south, and west connection.")]
	[SerializeField] private Sprite spriteN1S1E0W1 = null;

	[Tooltip("The sprite representing north, south, and east connection.")]
	[SerializeField] private Sprite spriteN1S1E1W0 = null;

	[Tooltip("The sprite representing north, south, east, and west connection.")]
	[SerializeField] private Sprite spriteN1S1E1W1 = null;

	private SpriteRenderer spriteRenderer = null;
	private Sprite[] sprites = new Sprite[16];
	private bool n = false;
	private bool e = false;
	private bool s = false;
	private bool w = false;

	void Awake()
	{
		this.spriteRenderer = GetComponent<SpriteRenderer>();

		this.sprites[0] = this.spriteN0S0E0W0;
		this.sprites[1] = this.spriteN0S0E0W1;
		this.sprites[2] = this.spriteN0S0E1W0;
		this.sprites[3] = this.spriteN0S0E1W1;
		this.sprites[4] = this.spriteN0S1E0W0;
		this.sprites[5] = this.spriteN0S1E0W1;
		this.sprites[6] = this.spriteN0S1E1W0;
		this.sprites[7] = this.spriteN0S1E1W1;
		this.sprites[8] = this.spriteN1S0E0W0;
		this.sprites[9] = this.spriteN1S0E0W1;
		this.sprites[10] = this.spriteN1S0E1W0;
		this.sprites[11] = this.spriteN1S0E1W1;
		this.sprites[12] = this.spriteN1S1E0W0;
		this.sprites[13] = this.spriteN1S1E0W1;
		this.sprites[14] = this.spriteN1S1E1W0;
		this.sprites[15] = this.spriteN1S1E1W1;

		this.SetSprite(this.n, this.e, this.s, this.w);
	}

	public void SetSprite(bool north, bool east, bool south, bool west)
	{
		int index = 0;

		if(north)
		{
			index += 8;
		}

		if(south)
		{
			index += 4;
		}

		if(east)
		{
			index += 2;
		}

		if(west)
		{
			++index;
		}

		this.spriteRenderer.sprite = this.sprites[index];
	}

	public LaserBeam SetConnection(Direction side, bool connected)
	{
		switch(side)
		{
		case Direction.NORTH:
			this.n = connected;
			break;
		case Direction.EAST:
			this.e = connected;
			break;
		case Direction.SOUTH:
			this.s = connected;
			break;
		case Direction.WEST:
			this.w = connected;
			break;
		}

		this.SetSprite(this.n, this.e, this.s, this.w);
		return this;
	}
}