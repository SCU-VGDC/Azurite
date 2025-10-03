using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour
{
    public static PersistentDataManager Instance;

    [SerializeField] private Dictionary<string, object> persistentDict;

    // basic singleton pattern
    private void Awake()
    {
        if (Instance != null) //Prevents duplicate instances of persistent data
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void Add(string key, object value)
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
}
