using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LaserGoal : LaserModifier
{
	[Tooltip("Whether or not the puzzle has been completed.")]
	[SerializeField] private bool puzzleComplete = false;

	[Tooltip("The incomplete sprite")]
	[SerializeField] private Sprite incompleteSprite = null;

	[Tooltip("The complete sprite.")]
	[SerializeField] private Sprite completeSprite = null;

	private SpriteRenderer spriteRenderer = null;

	void Awake()
	{
		this.spriteRenderer = this.GetComponent<SpriteRenderer>();
		this.transform.position = new Vector3(Mathf.Round(this.transform.position.x), Mathf.Round(this.transform.position.y), Mathf.Round(this.transform.position.z));
	}

	void OnValidate()
	{
		this.GetComponent<SpriteRenderer>().sprite = this.puzzleComplete ? this.completeSprite : this.incompleteSprite;
	}

	public bool IsComplete()
	{
		return this.puzzleComplete;
	}

	public override Direction? GetOutput(Direction side)
	{
		switch(side)
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

		return null;
	}

	public override void Hit(Direction side)
	{
		this.puzzleComplete = true;
		this.spriteRenderer.sprite = this.completeSprite;
	}

	public override void Reset()
	{
		this.puzzleComplete = false;
		this.spriteRenderer.sprite = this.incompleteSprite;
	}
}