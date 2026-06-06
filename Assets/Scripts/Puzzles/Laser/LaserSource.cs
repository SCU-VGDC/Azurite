using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LaserSource : MonoBehaviour
{
	[Tooltip("Whether or not the laser is locked.")]
	[SerializeField] private bool isLocked = false;

	[Tooltip("The maximum amount of tiles the laser can check.")]
	[SerializeField] private int maxScanDistance = 50;

	[Tooltip("The laser's rotation.")]
	[SerializeField] private Direction rotation = Direction.NORTH;

	[Tooltip("The laser beam prefab.")]
	[SerializeField] private LaserBeam laserPrefab = null;

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
	private LinkedList<LaserModifier> modifications = new LinkedList<LaserModifier>();

	void Awake()
	{
		this.spriteRenderer = GetComponent<SpriteRenderer>();
		this.transform.position = new Vector3(Mathf.Round(this.transform.position.x), Mathf.Round(this.transform.position.y), Mathf.Round(this.transform.position.z));

		foreach(LaserModifier modifier in this.transform.parent.GetComponentsInChildren<LaserModifier>())
		{
			modifier.clickEvent.AddListener(this.Reset);
		}
	}

	void Start()
	{
		this.UpdateLaser();
	}

	void OnMouseDown()
	{
		if(this.isLocked)
		{
			return;
		}

		this.Rotate();
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

	private void Reset()
	{
		for(int i = 0; i < this.transform.childCount; ++i)
		{
			Destroy(this.transform.GetChild(i).gameObject);
		}

		foreach(LaserModifier modifier in this.modifications)
		{
			modifier.Reset();
		}

		this.modifications.Clear();
		this.UpdateLaser();
	}

	protected static Direction GetOpposite(Direction direction)
	{
		switch(direction)
		{
		case Direction.NORTH:
			return Direction.SOUTH;
		case Direction.EAST:
			return Direction.WEST;
		case Direction.SOUTH:
			return Direction.NORTH;
		case Direction.WEST:
			return Direction.EAST;
		}

		return Direction.NORTH;
	}

	protected static Vector2Int GetVector(Direction direction)
	{
		switch(direction)
		{
		case Direction.NORTH:
			return Vector2Int.up;
		case Direction.EAST:
			return Vector2Int.right;
		case Direction.SOUTH:
			return Vector2Int.down;
		case Direction.WEST:
			return Vector2Int.left;
		}

		return Vector2Int.zero;
	}

	protected void UpdateLaser()
	{
		Direction direction = this.rotation;
		Vector2Int directionVec = GetVector(this.rotation);
		Vector2Int position = new Vector2Int((int) this.transform.position.x, (int) this.transform.position.y);
		RaycastHit2D[] raycast = Physics2D.LinecastAll(position, position + directionVec);
		HashSet<GameObject> ignore = new HashSet<GameObject>();

		ignore.Add(this.gameObject);
		ignore.Add(Instantiate(this.laserPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity, this.transform).SetConnection(direction, true).gameObject);

		for(int i = 0; i < this.maxScanDistance; ++i, raycast = Physics2D.LinecastAll(position, position + directionVec))
		{
			position += directionVec;
			LaserBeam beam = null;
			LaserModifier modifier = null;
			bool suicide = false;

			for(int j = 0; j < raycast.Length; ++j)
			{
				if(ignore.Contains(raycast[j].collider.gameObject))
				{
					continue;
				}
				else if(raycast[j].collider.TryGetComponent<LaserBeam>(out LaserBeam laser))
				{
					beam = laser;
				}
				else if(raycast[j].collider.TryGetComponent<LaserModifier>(out LaserModifier laserModifier))
				{
					modifier = laserModifier;
				}
				else if(raycast[j].collider.gameObject == this.gameObject)
				{
					suicide = true;
				}
				else
				{
					return;
				}
			}
			
			if(suicide)
			{
				beam.SetConnection(GetOpposite(direction), true);
				return;
			}

			ignore.Clear();

			Direction newDirection = direction;

			if(modifier != null)
			{
				this.modifications.AddFirst(modifier);
				ignore.Add(modifier.gameObject);
				modifier.Hit(GetOpposite(direction));

				Direction? output = modifier.GetOutput(GetOpposite(direction));

				if(!output.HasValue)
				{
					return;
				}

				newDirection = output.Value;
			}

			if(beam == null)
			{
				beam = Instantiate(this.laserPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity, this.transform);
			}

			ignore.Add(beam.gameObject);
			beam.SetConnection(GetOpposite(direction), true);
			beam.SetConnection(newDirection, true);

			direction = newDirection;
			directionVec = GetVector(direction);
		}
	}

	public void Rotate()
	{
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

		this.Reset();
	}
}