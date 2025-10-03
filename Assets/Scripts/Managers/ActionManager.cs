using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    private int actionCounter
    {
        get { return (int)PersistentDataManager.Instance.Get("actionCounter"); }

        set { PersistentDataManager.Instance.Set("actionCounter", value);  }
    }

    private int actionThreshold
    {
        get { return (int)PersistentDataManager.Instance.Get("actionThreshold"); }

        set { PersistentDataManager.Instance.Set("actionThreshold", value); }
    }

    private int[] actionThresholdIncrease
    {
        get { return PersistentDataManager.Instance.Get("actionThresholdIncrease") as int[]; }

        set { PersistentDataManager.Instance.Set("actionThresholdIncrease", value);  }
    }

    private int worldState
    {
        get { return (int)PersistentDataManager.Instance.Get("worldState"); }

        set { PersistentDataManager.Instance.Set("worldState", value); }
    }

    private int WorldStateMax
    {
        get { return ((int[])PersistentDataManager.Instance.Get("actionThresholdIncrease")).Length; }
    }

    [SerializeField] private SubmarineRoute submarine;
    [SerializeField] private string submarineInRoom;

    // basic singleton pattern
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // initialize persistent data
        actionCounter = 0;
        actionThreshold = 10;
        actionThresholdIncrease = new int[] { 10, 10, 10, 10, 10 };
    }

    private void IncrementRoomState()
    {
        if (actionCounter >= actionThreshold && (worldState + 1) <= WorldStateMax)
        {
            actionThreshold += actionThresholdIncrease[worldState];
            worldState++;

            Debug.Log(worldState);
            IncrementRoomState();
        }
    }

    public void IncrementAction(int x)
    {
        if (worldState + 1 <= WorldStateMax)
        {
            actionCounter += x;

            Debug.Log(actionCounter);
            IncrementRoomState();
        }
    }

    public void ChangeSubmarineState(string name)
    {
        if (submarine != null)
        {
            string potentialName = submarine.TryMoveNext(out var successSub, name);

            if (successSub == true)
            {
                submarineInRoom = potentialName;
            }
        }
    }
}
