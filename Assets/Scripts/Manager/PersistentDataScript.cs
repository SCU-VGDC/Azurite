using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataScript : MonoBehaviour
{
    public static PersistentDataScript instance;
    //Destination coords passed in when teleporter is activated. These will be used to determine player's starting location within each scene.
    private float[] destinationCoords = new float[2];
    private Vector2 DestCoords;
    public int worldState; //Sets the world state. All rooms should call this to determine their state.
    //0 - Default World State
    private int ActionCounter = 0;
    public int ActionThreshold = 10;
    public int[] ActionThresholdIncrease = new int[5];
    [SerializeField] public ItemData[] ITEM_LIST;
    [HideInInspector] public InventoryManager PlayerInventory;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null) //Prevents duplicate instances of persistent data.
        {
            Destroy(this.gameObject);
            return;

        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        PlayerInventory = new InventoryManager();
    }
    //For DEBUG Only
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
    public void SetDestinationCoordinates(Vector2 x)
    {
        DestCoords = x;
        return;
    }
    public void IncrementRoomState()
    {
        ActionThreshold += ActionThresholdIncrease[worldState];
        worldState++;
        return;
    }
    public void IncrementAction(int x)
    {
        ActionCounter += x;
        if (ActionCounter >= ActionThreshold)
        {
            IncrementRoomState();
        }
        return;

    }
}
