using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager inst;

    void Awake()
    {
        // singleton pattern
        if (inst == null)
        {
            inst = this;

            DontDestroyOnLoad(this);
        }
        else if (inst != this)
        {
            Destroy(this);
        }
    }
}
