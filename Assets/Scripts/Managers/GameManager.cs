using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    [System.NonSerialized] public GameObject player;

    // game states
    public bool paused;

    void Start()
    {
        paused = false;
    }


    void Awake()
    {
        // Basic singleton pattern. Make sure there is only ever 1 GameManager in the scene and updates inst accordingly.

        // If no GameManager assigned...
        if (inst == null)
        {
            // This is our GameManager
            inst = this;
        }
        // If there is a GameManager assigned, and it's not this
        else if (inst != this)
        {
            // Then this shouldn't exist, destroy it.
            Destroy(this);
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }
}
