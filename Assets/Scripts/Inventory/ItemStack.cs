using UnityEngine;

public class ItemStack : MonoBehaviour
{
	/// <summary>The item.</summary>
	[Tooltip("The item.")]
	[SerializeField] private Item item = null;

	/// <summary>The quantity of the item.</summary>
	[Tooltip("The quantity of the item.")]
	[SerializeField] private int count = 1;

	/// <summary>
	/// Add the item stack to the specified inventory.
	/// </summary>
	/// <param name="inventory">The inventory to add to.</param>
	public void AddTo()
	{
		GameManager.inst.player.Inventory.AddItem(this.item, this.count);
	}

	/// <summary>
	/// Remove an item from the specified inventory.
	/// </summary>
	/// <param name="inventory">The inventory to remove from.</param>
	public void RemoveFrom(Inventory inventory)
	{
		inventory.RemoveItem(this.item, this.count);
	}

	/// <summary>
	/// Get the item.
	/// </summary>
	/// <returns>The item.</returns>
	public Item GetItem()
	{
		return this.item;
	}

	/// <summary>
	/// Get the quantity of the item.
	/// </summary>
	/// <returns>The quantity of the item.</returns>
	public int GetCount()
	{
		return this.count;
	}
}