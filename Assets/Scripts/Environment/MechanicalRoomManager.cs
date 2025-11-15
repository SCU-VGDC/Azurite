using System.Collections.Generic;
using UnityEngine;

public class MechanicalRoomManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> doors;

    const int NUMBER_PUZZLES_PER_DOOR = 4; 

    /// <summary>
    /// 4 sets of puzzle prefabs, one set for each part of the mechanical room separated by doors.
    /// </summary>
    [SerializeField] private List<List<GameObject>> puzzlePrefabSets;

    void Start()
    {
        // add a watch to see if this door should be open 
        PersistentDataManager.Instance.ListenForKeyChanged<List<List<bool>>>("mechanicalRoomPuzzlesCompleted", OnDoorKeyChanged);
    }

    void OnDoorKeyChanged(List<List<bool>> newPuzzlesCompletedState, List<List<bool>> oldPuzzlesCompletedState)
    {
        for (int doorIndex = 0; doorIndex < doors.Count; doorIndex++)
        {
            int numberCompletedPuzzles = 0;
            for (int puzzleIndex = 0; puzzleIndex < newPuzzlesCompletedState[doorIndex].Count; puzzleIndex++)
            {
                bool puzzleCompleted = newPuzzlesCompletedState[doorIndex][puzzleIndex];

                if (puzzleCompleted)
                {
                    numberCompletedPuzzles++;
                }

            }
            
            // if all the puzzles for this door have been completed
            if (numberCompletedPuzzles >= NUMBER_PUZZLES_PER_DOOR)
            {
                GameObject tempDoor = doors[doorIndex].gameObject;
                doors[doorIndex] = null;

                OpenDoor(tempDoor);
            }
        }
    }

    // TODO: may need to be changed later
    void OpenDoor(GameObject door)
    {
        if (door == null) return;

        Destroy(door);
    }

    /// <summary>
    /// Returns a random puzzle from the specified set and removes it from the set.
    /// </summary>
    /// <param name="setNumber"></param>
    /// <returns></returns>
    public GameObject GetAPuzzle(int setNumber)
    {
        int randomPuzzleIndex = Random.Range(0, puzzlePrefabSets[setNumber].Count);

        GameObject puzzlePrefab = puzzlePrefabSets[setNumber][randomPuzzleIndex];
        puzzlePrefabSets[setNumber].RemoveAt(randomPuzzleIndex);

        return puzzlePrefab;
    }
}
