using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour
{
    public static PersistentDataManager Instance;

    [SerializeField] private Dictionary<string, object> persistentDict = new Dictionary<string, object>();

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
    }

    public void Set(string key, object value)
    {
        persistentDict[key] = value;
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
        return (T)persistentDict[key];
    }
}
