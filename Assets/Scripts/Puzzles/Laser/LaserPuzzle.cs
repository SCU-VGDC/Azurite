using System.Collections.Generic;

public class LaserPuzzle : PuzzleLogic
{
	private List<LaserGoal> goals = new List<LaserGoal>();

	void Awake()
	{
		this.goals.Clear();

		foreach(LaserGoal goal in this.GetComponentsInChildren<LaserGoal>())
		{
			this.goals.Add(goal);
		}
	}

	public override bool IsComplete()
	{
		foreach(LaserGoal goal in this.goals)
		{
			if(!goal.IsComplete())
			{
				return false;
			}
		}

		return true;
	}
}