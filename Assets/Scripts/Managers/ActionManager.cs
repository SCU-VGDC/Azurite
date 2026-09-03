using System;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    private int ActionCounter
    {
        get => PersistentDataManager.Instance.Get<int>("actionCounter");
        set => PersistentDataManager.Instance.Set("actionCounter", value);
    }

    private int ActionThreshold
    {
        get => PersistentDataManager.Instance.Get<int>("actionThreshold");
        set => PersistentDataManager.Instance.Set("actionThreshold", value);
    }

    private int[] ActionThresholdIncrease
    {
        get => PersistentDataManager.Instance.Get<int[]>("actionThresholdIncrease");
        set => PersistentDataManager.Instance.Set("actionThresholdIncrease", value);
    }

    private int WorldState
    {
        get => PersistentDataManager.Instance.Get<int>("worldState");
        set => PersistentDataManager.Instance.Set("worldState", value);
    }

    private int WorldStateMax => PersistentDataManager.Instance.Get<int[]>("actionThresholdIncrease").Length;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void RuntimeInit()
    {
        Instance = null;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // initialize persistent data
        ActionCounter = 0;
        ActionThreshold = 10;
        ActionThresholdIncrease = new int[] { 10, 10, 10, 10, 10 };
    }

    private void IncrementRoomState()
    {
        if (ActionCounter >= ActionThreshold && (WorldState + 1) <= WorldStateMax)
        {
            ActionThreshold += ActionThresholdIncrease[WorldState];
            WorldState++;

            Debug.Log(WorldState);
            IncrementRoomState();
        }
    }

    public void IncrementAction(int x)
    {
        if (WorldState + 1 <= WorldStateMax)
        {
            ActionCounter += x;
            IncrementRoomState();
        }
    }

    public void ChangeSubmarineState(string name)
    {
        PersistentDataManager.Instance.Set("submarineInRoom", name);
    }
}
