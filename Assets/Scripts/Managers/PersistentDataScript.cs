using UnityEngine;

public class PersistentDataScript : MonoBehaviour
{
    public static PersistentDataScript instance;
    private int worldState; //Sets the world state. All rooms should call this to determine their state.
    //0 - Default World State
    private int ActionCounter = 0;
    private int ActionThreshold = 10;
    private static readonly int[] ActionThresholdIncrease = new int[] { 10, 10, 10, 10, 10 }; //Adds to threshold limit. 
    private readonly int WorldStateMax = ActionThresholdIncrease.Length;
    public ItemData[] ITEM_LIST;
    [System.NonSerialized] public InventoryManager PlayerInventory;

    // Start is called before the first frame update
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

    public void IncrementThreshold(int x)
    {
        if (worldState != WorldStateMax)
        {
            ActionThreshold += x;
            Debug.Log(ActionThreshold);
        }
    }

    private void IncrementRoomState()
    {
        if (ActionCounter >= ActionThreshold && (worldState + 1) <= (WorldStateMax))
        {
            ActionThreshold += ActionThresholdIncrease[worldState];
            worldState++;
            Debug.Log(worldState);
            IncrementRoomState();
        }
        return;
    }

    public void IncrementAction(int x)
    {
        if (worldState + 1 <= WorldStateMax)
        {
            ActionCounter += x;
            Debug.Log(ActionCounter);
            IncrementRoomState();
            return;
        }
    }
}
