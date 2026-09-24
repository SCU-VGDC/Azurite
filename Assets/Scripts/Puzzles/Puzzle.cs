using System;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public abstract Bounds Bounds { get; }
    public event Action OnClosed;
    public event Action OnSolved;
    public bool Solved { get; protected set; }

    public virtual void Solve()
    {
        Solved = true;
        OnSolved?.Invoke();
    }

    public virtual void Close()
    {
        OnClosed?.Invoke();
    }
}
