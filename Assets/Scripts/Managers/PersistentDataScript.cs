using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataScript : MonoBehaviour
{
    public static PersistentDataScript instance;

    //Destination coords passed in when teleporter is activated. These will be used to determine player's starting location within each scene.
    private Vector2 destinationCoords;
    public int worldState; //Sets the world state. All rooms should call this to determine their state.
    //0 - Default World State

    public ItemData[] ITEM_LIST;
    [System.NonSerialized] public InventoryManager PlayerInventory;

    private void Awake()
    {
        if (instance != null) //Prevents duplicate instances of persistent data.
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        PlayerInventory = new InventoryManager();
    }

    //Just in case added effects are wanted later (visual effects, sound, etc.)
    public void SetRoomState(int x)
    {
        worldState = x;
        return;
    }

    //Just in case added effects or variables are needed when checked (UI or anything else).
    public int GetRoomState()
    {
        return worldState;
    }

    //Used to set destination coordinates for teleporation.
    public void SetDestinationCoordinates(Vector2 destination)
    {
        destinationCoords.x = destination.x;
        destinationCoords.y = destination.y;
    }
}
