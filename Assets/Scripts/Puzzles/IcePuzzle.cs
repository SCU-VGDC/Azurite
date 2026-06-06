using System;
using DG.Tweening;
using UnityEngine;

public class IcePuzzle : PuzzleLogic
{
	[Tooltip("The maximum amount of tiles the player can travel.")]
	[SerializeField] private int maxScanDistance = 25;
	
	[Tooltip("The goal hitbox.")]
	[SerializeField] private Collider2D goalCollider = null;
	
	[Tooltip("Whether or not the puzzle has been completed.")]
	[SerializeField] private bool puzzleComplete = false;
	
	[Tooltip("The duration of the slide animation in seconds.")]
	[SerializeField] private float slideDuration = 0.125f;

	private Tween slideAnimation = null;

	void Start()
	{
		this.transform.position = new Vector3(Mathf.Round(this.transform.position.x), Mathf.Round(this.transform.position.y), Mathf.Round(this.transform.position.z));
		this.goalCollider.transform.position = new Vector3(Mathf.Round(this.goalCollider.transform.position.x), Mathf.Round(this.goalCollider.transform.position.y), Mathf.Round(this.goalCollider.transform.position.z));
	}

	void Update()
	{
		if(this.IsComplete())
		{
			return;
		}

		if(Input.GetButtonDown("Horizontal"))
		{
			Move(new Vector2Int((int) Math.Round(Input.GetAxisRaw("Horizontal")), 0));
		}
		else if(Input.GetButtonDown("Vertical"))
		{
			Move(new Vector2Int(0, (int) Math.Round(Input.GetAxisRaw("Vertical"))));
		}
	}

	private void Move(Vector2Int direction)
    {
		this.slideAnimation?.Complete();

		Vector2Int position = new Vector2Int((int) this.transform.position.x, (int) this.transform.position.y);
		RaycastHit2D raycast = Physics2D.Linecast(position, position + direction);

		for(int i = 0; i < this.maxScanDistance && !raycast; ++i, raycast = Physics2D.Linecast(position, position + direction))
		{
			position += direction;
		}

		if(raycast.collider == this.goalCollider)
		{
			this.puzzleComplete = true;
			position += direction;
		}

		this.slideAnimation = this.transform.DOMove(new Vector3(position.x, position.y, this.transform.position.z), this.slideDuration);
    }

	public override bool IsComplete()
	{
		return this.puzzleComplete;
	}
}