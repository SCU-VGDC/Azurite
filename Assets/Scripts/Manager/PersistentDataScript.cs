using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataScript : MonoBehaviour
{
    public static PersistentDataScript instance;

    public float[] destinationCoords = new float[2];

    public int worldState; //Sets the world state. All rooms should call this to determine their state.
    //0 - Default World State
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;

        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }
    //Just in case added effects are wanted later.
    public void setRoomState(int x)
    {
        worldState = x;
        return;
    }
    //Just in case added effects or variables are needed when checked.
    public int getRoomState()
    {
        return worldState;
    }
    public void setDestinationCoordinates(float x, float y)
    {
        destinationCoords[0] = x;
        destinationCoords[1] = y;
        return;
    }
}
