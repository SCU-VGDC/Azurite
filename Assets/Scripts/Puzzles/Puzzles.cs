using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class Puzzles : ScriptableObject
{
    [SerializeField] public int ButtonCount;
    [SerializeField] public int width;
    [SerializeField] public int height;

    [SerializeField] public bool ToggleNearby;


    [SerializeField] public bool[] StartOn;
    [SerializeField] public bool[] SolutionIsOn;

    [SerializeField] public bool[] IsLocked;

    [SerializeField] public ButtonScript.ButtonSprites[] ButtonIcons;


}
