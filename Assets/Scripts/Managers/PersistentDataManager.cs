using System;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour
{
    public static PersistentDataManager Instance;
    
    [SerializeField] private Dictionary<string, object> persistentDict = new();
    private readonly Dictionary<string, Delegate> eventStore = new();

    // basic singleton pattern
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;
    }

    public void Start()
    {
        // Initialize any persistent data
        Instance.Set("worldState", 0);

        // mechanical room
        Instance.Set("mechanicalRoomPuzzlesCompleted", new List<List<bool>>() {
            new List<bool> { false, false, false, false },
            new List<bool> { false, false, false, false },
            new List<bool> { false, false, false, false },
            new List<bool> { false, false, false, false },
        });
    }

    public void Set<T>(string key, T value)
    {
        T oldValue = default;
        if (persistentDict.TryGetValue(key, out var oldValueObj))
        {
            if (oldValueObj is not T)
            {
                Debug.LogError($"'{key}' is not a {typeof(T)}!");
                return;
            }
            oldValue = (T)oldValueObj;
        }

        persistentDict[key] = value;

        if (eventStore.TryGetValue(key, out var func))
        {
            func.DynamicInvoke(value, oldValue);
        }
    }

    public bool Remove(string key)
    {
        return persistentDict.Remove(key);
    }

    public object Get(string key)
    {
        return persistentDict[key];
    }

    public T Get<T>(string key)
    {
        return (T)Get(key);
    }

    public bool TryGet(string key, out object value)
    {
        return persistentDict.TryGetValue(key, out value);
    }

    public bool TryGet<T>(string key, out T value)
    {
        if (persistentDict.TryGetValue(key, out object obj) && obj is T castObj)
        {
            value = castObj;
            return true;
        }
        value = default;
        return false;
    }

    public void ListenForKeyChanged<T>(string key, Action<T,T> action)
    {
        if (eventStore.TryGetValue(key, out var func))
            eventStore[key] = Delegate.Combine(func, action);
        else
            eventStore[key] = action;
    }

    public void StopListeningForKeyChanged(string key, Delegate action)
    {
        if (eventStore.TryGetValue(key, out var func))
        {
            var newDelegate = Delegate.Remove(func, action);
            if (newDelegate == null)
                eventStore.Remove(key);
            else
                eventStore[key] = newDelegate;
        }
    }
}
