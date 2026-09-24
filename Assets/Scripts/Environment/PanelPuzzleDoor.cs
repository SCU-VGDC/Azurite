using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PanelPuzzleDoor : MonoBehaviour
{
    [SerializeField] private PuzzleInteraction[] puzzles;

    private void Start()
    {
        foreach (var puzzle in puzzles)
            puzzle.OnSolved += CheckPuzzleStates;
    }

    private void OnDestroy()
    {
        foreach (var puzzle in puzzles)
            puzzle.OnSolved -= CheckPuzzleStates;
    }

    private void CheckPuzzleStates()
    {
        if (puzzles.All(p => p.Solved))
            gameObject.SetActive(false);
    }
}
