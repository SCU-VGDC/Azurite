using UnityEngine;
using UnityEngine.UI;

public class InteractionUIController : InventoryUIController
{
	/// <summary>
	/// Add an item stack to to the inventory UI. This does not actually 
	/// add an item to the underlying inventory and is used only for 
	/// updating the UI.
	/// </summary>
	/// <param name="item">The item to add.</param>
	protected override void AddItem(Item item)
	{
		// Create a new item stack controller
		ItemStackUIController stack = Instantiate(this.itemStackPrefab);
		stack.Init(this.inventory, item);
		stack.transform.SetParent(this.itemList.transform);
		this.UpdateGridSize();

		// Add the stack to the item list toggle group.
		if(stack.TryGetComponent(out Toggle toggle))
		{
			toggle.group = this.itemList;
			toggle.onValueChanged.AddListener(this.UpdateItemName);
		}
	}

	/// <summary>
	/// Remove an item stack from the inventory UI. This does not actually 
	/// remove an item from the underlying inventory and is only used for 
	/// updating the UI.
	/// </summary>
	/// <param name="item">The item to remove.</param>
	protected override void RemoveItem(Item item)
	{
		ItemStackUIController stack = this.GetItemStack(item);

		if(stack != null)
		{
			Destroy(stack.gameObject);
			this.UpdateGridSize();
		}
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