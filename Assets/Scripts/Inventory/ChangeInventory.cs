using UnityEngine;

public class ChangeInventory : MonoBehaviour
{
	[SerializeField] private Inventory inventory = null;
	[SerializeField] private Item item = null;
	[SerializeField] private int amount = 1;

	/// <summary>
	/// Add an item to the inventory.
	/// </summary>
    public void AddItem()
	{
		this.inventory.AddItem(this.item, this.amount);
	}

	/// <summary>
	/// Remove an item from the inventory.
	/// </summary>
	public void RemoveItem()
	{
		this.inventory.RemoveItem(this.item, this.amount);
	}
}