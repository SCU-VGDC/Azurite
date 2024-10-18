using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateManager : MonoBehaviour
{
    public static RoomStateManager instance;

    public int worldState = 0; //Sets the world state. All rooms should call this to determine their state.
    //0 - Default World State
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);

        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
