using System;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class Puzzles : ScriptableObject
{
    public int ButtonCount;
    public int width;
    public int height;

    public bool ToggleNearby;


    public bool[] StartOn;
    public bool[] SolutionIsOn;

    public bool[] IsLocked;

    public ButtonScript.ButtonSprites[] ButtonIcons;
}
