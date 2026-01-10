using UnityEngine;
using UnityEngine.UI;

public class InteractionMenuController : InventoryMenuController
{
	/// <summary>
	/// Add an item stack to to the inventory menu. This does not actually 
	/// add an item to the underlying inventory and is used only for 
	/// updating the menu.
	/// </summary>
	/// <param name="item">The item to add.</param>
	protected override void AddItemEntry(Inventory inventory, Item item)
	{
		base.AddItemEntry(inventory, item);
		this.UpdateGridSize();
	}

	/// <summary>
	/// Remove an item stack from the inventory menu. This does not actually 
	/// remove an item from the underlying inventory and is only used for 
	/// updating the menu.
	/// </summary>
	/// <param name="item">The item to remove.</param>
	protected override void RemoveItemEntry(Inventory inventory, Item item)
	{
		base.RemoveItemEntry(inventory, item);
		this.UpdateGridSize();
	}

	/// <summary>
	/// Update the column count of the grid layout group so the grid stays 
	/// a horizontal rectangle/square.
	/// </summary>
	private void UpdateGridSize()
	{
		if(!this.itemList.TryGetComponent(out GridLayoutGroup grid))
		{
			return;
		}

		grid.constraintCount = Mathf.FloorToInt(Mathf.Sqrt(this.itemList.transform.childCount - 1)) + 1;
	}
}