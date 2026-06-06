using UnityEngine;
using UnityEngine.Events;

public abstract class LaserModifier : MonoBehaviour
{
	public UnityEvent clickEvent = new UnityEvent();

	public abstract Direction? GetOutput(Direction side);

	public abstract void Hit(Direction side);

	public abstract void Reset();
}