using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataScript : MonoBehaviour
{
    public static PersistentDataScript instance;
    //Destination coords passed in when teleporter is activated. These will be used to determine player's starting location within each scene.
    private float[] destinationCoords = new float[2];
    public int worldState; //Sets the world state. All rooms should call this to determine their state.
    //0 - Default World State
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
    public void SetDestinationCoordinates(float x, float y)
    {
        destinationCoords[0] = x;
        destinationCoords[1] = y;
        return;
    }
}
