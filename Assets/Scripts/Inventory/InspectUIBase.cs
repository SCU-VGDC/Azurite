using UnityEngine;

public abstract class InspectUIBase : MonoBehaviour 
{
	/// <summary>
	/// Initialize the inspect UI with the proper item.
	/// </summary>
	/// <param name="item">The item to display.</param>
	/// <param name="previousUI">The previously open UI.</param>
	public abstract void Init(Item item, MonoBehaviour previousUI);
}