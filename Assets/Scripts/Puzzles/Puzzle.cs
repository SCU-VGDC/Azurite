using UnityEngine;

public class Puzzle : MonoBehaviour
{
	protected PuzzleLogic mainPuzzle = null;

	void Awake()
	{
		this.mainPuzzle = this.GetComponentInChildren<PuzzleLogic>();

		if(this.mainPuzzle == null)
		{
			this.mainPuzzle = this.GetComponent<PuzzleLogic>();
		}
	}

	public bool IsComplete()
	{
		return this.mainPuzzle.IsComplete();
	}
}