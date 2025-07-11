using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataScript : MonoBehaviour
{
    public static PersistentDataScript Instance;
    //Destination coords passed in when teleporter is activated. These will be used to determine player's starting location within each scene.
    private float[] destinationCoords = new float[2];
    private Vector2 DestCoords;
    private int worldState; //Sets the world state. All rooms should call this to determine their state.
    //0 - Default World State
    private int ActionCounter = 0;
    private int ActionThreshold = 10;
    static private int[] ActionThresholdIncrease = new int[] { 10, 10, 10, 10, 10 }; //Adds to threshold limit. 
    private int WorldStateMax = ActionThresholdIncrease.Length;
    [SerializeField] private SubmarineRoute Submarine;
    [SerializeField] private string SubmarineInRoom;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null) //Prevents duplicate instances of persistent data.
        {
            Destroy(this.gameObject);
            return;

        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
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
    public void ChangeSubmarineState(string name)
    {
        if (Submarine != null)
        {
            string potentialName = Submarine.TryMoveNext(out var successSub, name);
            if (successSub == true)
            {
                SubmarineInRoom = potentialName;
            }
        }
    }
    public string GetSubmarineName() 
    {
        return SubmarineInRoom;
    }
}
