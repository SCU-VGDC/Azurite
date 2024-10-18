using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateManager : MonoBehaviour
{
    public static RoomStateManager instance;

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
        worldState = 0;

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
