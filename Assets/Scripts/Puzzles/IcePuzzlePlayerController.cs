using UnityEngine;

public class IcePuzzlePlayerController : MonoBehaviour
{
	/// <summary>The maximum amount of tiles the player can travel.</summary>
	[Tooltip("The maximum amount of tiles the player can travel.")]
	public int maxScanDistance = 25;
	/// <summary>The goal hitbox.</summary>
	[Tooltip("The goal hitbox.")]
	public Collider2D goalCollider = null;
	/// <summary>Whether or not the puzzle has been completed.</summary>
	[Tooltip("Whether or not the puzzle has been completed.")]
	public bool puzzleComplete = false;
	public bool runOnceFlag = false;
	/// <summary>The duration of the slide animation in seconds.</summary>
	[Tooltip("The duration of the slide animation in seconds.")]
	public float slideDuration = 0.5f;

	/// <summary>The start position of the slide animation.</summary>
	private Vector3 startPos = Vector3.zero;
	/// <summary>The end position of the slide animation.</summary>
	private Vector3 finalPos = Vector3.zero;
	/// <summary>The animation progress from 0 to 1.</summary>
	private float animationTime = 1;

	void Update()
	{
		// Play the slide animation.
		if(this.animationTime < 1)
		{
			this.animationTime += Time.deltaTime / this.slideDuration;
			
			if(this.animationTime > 1)
			{
				this.animationTime = 1;
			}

			// The player's position is determined by the equation 6x^5 - 15x^4 + 10x^3
			float interpolatedTime = this.animationTime * this.animationTime * this.animationTime * (this.animationTime * (6f * this.animationTime - 15f) + 10f);

			this.transform.position = new Vector3(
				(this.finalPos.x - this.startPos.x) * interpolatedTime + this.startPos.x,
				(this.finalPos.y - this.startPos.y) * interpolatedTime + this.startPos.y,
				0
			);
		}

		// Stop further processing if the puzzle has been completed
		if(this.puzzleComplete)
		{
			if (!runOnceFlag)
			{
            	StartCoroutine(GameManager.inst.Sleep(1.0f, GameManager.inst.EndCurrentPuzzle));

				runOnceFlag = true;
			}

			return;
		}
		
		// Player movement is controlled by WASD or arrow keys.
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			Move(Vector2Int.up);
		}
		else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			Move(Vector2Int.down);
		}
		else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Move(Vector2Int.left);
		}
		else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			Move(Vector2Int.right);
		}
	}

	/// <summary>
	/// Move the player in a straight line.
	/// </summary>
	/// <param name="direction">The unit vector representing the player's driection.</param>
    private void Move(Vector2Int direction)
    {
		// If the slide animation hasn't completed, teleport the player.
		if(this.animationTime < 1)
		{
			this.animationTime = 1;
			this.transform.position = this.finalPos;
		}

		Vector2Int position = new Vector2Int((int) this.transform.position.x, (int) this.transform.position.y);
		RaycastHit2D[] raycasts = null;

		// Continually move the player by one tile until a wall has been hit or the player has moved the maximum amount of tiles.
		for(int i = 0; i < this.maxScanDistance && (raycasts = Physics2D.LinecastAll(position, position + direction)).Length == 0; ++i)
		{
			position += direction;
        }

		// Check if the wall hit is the goal.
		// If so, move the player one tile more and complete the puzzle.
		for(int i = 0; i < raycasts.Length; ++i)
		{
			if(raycasts[i].collider == this.goalCollider)
			{
				this.puzzleComplete = true;
				position += direction;
				break;
			}
		}

		// Start the slide animation.
		this.animationTime = 0;
		this.startPos = this.transform.position;
		this.finalPos = (Vector3Int) position;
    }
}