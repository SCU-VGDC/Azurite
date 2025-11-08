using System.Collections.Generic;
using UnityEngine;

public class MechanicalRoomDoor : MonoBehaviour
{
    [SerializeField] private List<GameObject> doors;

    void Start()
    {
        // add a watch to see if this door should be open 
        PersistentDataManager.Instance.ListenForKeyChanged<List<bool>>("mechanicalRoomDoorsOpen", OnDoorKeyChanged);
    }

    void OnDoorKeyChanged(List<bool> newDoorStates, List<bool> oldDoorStates)
    {
        for (int i = 0; i < doors.Count; i++)
        {
            if (newDoorStates[i] && doors[i] != null)
            {
                GameObject tempDoor = doors[i].gameObject;
                doors[i] = null;

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
}
