using UnityEngine;

public class IcePuzzlePlayerController : MonoBehaviour
{
	public int MaxScanDistance = 25;
	public Collider2D GoalCollider;
	public bool PuzzleComplete = false;

	void Update()
	{
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

    private void Move(Vector2Int direction)
    {
		if(PuzzleComplete)
		{
			return;
		}

		Vector2Int position = new Vector2Int((int) transform.position.x, (int) transform.position.y);
		RaycastHit2D[] raycasts = null;

		for(int i = 0; i < MaxScanDistance && (raycasts = Physics2D.LinecastAll(position, position + direction)).Length == 0; ++i)
		{
			position += direction;
        }

		for(int i = 0; i < raycasts.Length; ++i)
		{
			if(raycasts[i].collider == GoalCollider)
			{
				PuzzleComplete = true;
				position += direction;
				break;
			}
		}

		transform.position = (Vector3Int) position;
    }
}