using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuController : MenuBase
{
	[Tooltip("The inventory to display.")]
	[SerializeField] protected Inventory inventory = null;

	[Tooltip("The item stack slot prefab.")]
	[SerializeField] protected ItemStackEntryController itemStackPrefab = null;

	[Tooltip("The toggle group containing the item stacks.")]
	[SerializeField] protected ToggleGroup itemList = null;

	[Tooltip("The item name text box.")]
	[SerializeField] protected TextMeshProUGUI itemName = null;

	public InventoryMenuController Init(Inventory associatedInventory)
	{
		this.inventory = associatedInventory;

		this.inventory.itemAddedEvent.AddListener(this.AddItemEntry);
		this.inventory.itemRemovedEvent.AddListener(this.RemoveItemEntry);
		this.inventory.itemChangedEvent.AddListener(this.UpdateItemEntry);

		Item[] items = this.inventory.GetItems();

		for(int i = 0; i < items.Length; ++i)
		{
			this.AddItemEntry(items[i]);
		}

		return this;
	}

	public override void Update()
	{
		base.Update();

		// Handle inventory traversal with WASD or arrow keys.
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.MoveSelection(Vector2Int.down);
		}

		if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.MoveSelection(Vector2Int.up);
		}

		if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.MoveSelection(Vector2Int.left);
		}

		if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.MoveSelection(Vector2Int.right);
		}

		// Open the inspect menu when space is pressed.
		if(this.childMenu == null && Input.GetKeyDown(KeyCode.Space))
		{
			Item selected = this.GetSelectedItem();

			if(selected == null)
			{
				return;
			}

			this.childMenu = Instantiate(selected.GetInspectMenuPrefab(), this.transform.parent).Init(selected);
			this.childMenu.SetParent(this);
			this.childMenu.gameObject.SetActive(false);
			this.childMenu.onClose.AddListener(() => {
				Destroy(this.childMenu.gameObject);
				this.childMenu = null;
			});
			
			this.onHide.AddListener(this.OpenInspectMenu);
			this.Hide();
		}
	}

	private void OpenInspectMenu()
	{
		this.gameObject.SetActive(false);
		this.childMenu.gameObject.SetActive(true);
		this.childMenu.Open();
		this.onHide.RemoveListener(this.OpenInspectMenu);
	}

	/// <summary>
	/// Get the inventory associated with this menu.
	/// </summary>
	/// <returns>The inventory associated with this menu.</returns>
	public Inventory GetInventory()
	{
		return this.inventory;
	}

	/// <summary>
	/// Add an item stack to to the inventory menu. This does not actually 
	/// add an item to the underlying inventory and is used only for 
	/// updating the menu.
	/// </summary>
	/// <param name="item">The item to add.</param>
	protected virtual void AddItemEntry(Item item)
	{
		ItemStackEntryController stack = Instantiate(this.itemStackPrefab, this.itemList.transform).Init(this.inventory, item);

		if(stack.TryGetComponent(out Toggle toggle))
		{
			toggle.group = this.itemList;
			toggle.onValueChanged.AddListener(this.UpdateItemName);

			if(this.itemList.transform.childCount == 1) // Update the text box when the first item is added.
			{
				this.UpdateItemName(false);
			}
		}
	}

	/// <summary>
	/// Remove an item stack from the inventory menu. This does not actually 
	/// remove an item from the underlying inventory and is only used for 
	/// updating the menu.
	/// </summary>
	/// <param name="item">The item to remove.</param>
	protected virtual void RemoveItemEntry(Item item)
	{
		ItemStackEntryController stack = this.GetItemStack(item);

		if(stack != null)
		{
			Destroy(stack.gameObject);
		}
	}

	/// <summary>
	/// Update an item stack in the inventory menu. This refreshes the item 
	/// stack's stack count label.
	/// </summary>
	/// <param name="item">The item to update.</param>
	protected virtual void UpdateItemEntry(Item item)
	{
		ItemStackEntryController stack = this.GetItemStack(item);

		if(stack != null)
		{
			stack.Refresh();
		}
	}

	/// <summary>
	/// Update the item name in the currently selected item text box.
	/// </summary>
	/// <param name="_">Unused.</param>
	protected virtual void UpdateItemName(bool _)
	{
		if(this.itemName == null)
		{
			return;
		}

		Item selected = this.GetSelectedItem();

		if(selected != null)
		{
			this.itemName.SetText(selected.GetDisplayName());
			this.itemName.enabled = true;

			// Unity says we shouldn't use this function but it works :/
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) this.itemName.transform);
		}
		else
		{	
			this.itemName.enabled = false;
		}
	}

	/// <summary>
	/// Gets the currently selected item stack menu component.
	/// </summary>
	/// <returns>The currently selected item stack menu component or null if none exists.</returns>
	public ItemStackEntryController GetSelectedStack()
	{
		Toggle selected = this.itemList.GetFirstActiveToggle();
		return selected != null ? selected.gameObject.GetComponent<ItemStackEntryController>() : null;
	}

	/// <summary>
	/// Gets the currently seletced item.
	/// </summary>
	/// <returns>The currently selected item or null if none exists.</returns>
	public Item GetSelectedItem()
	{
		ItemStackEntryController selectedStack = this.GetSelectedStack();
		return selectedStack != null ? selectedStack.GetItem() : null;
	}

	/// <summary>
	/// Gets the item stack menu component associated with the specified item.
	/// </summary>
	/// <param name="item">The item to search for.</param>
	/// <returns>The item stack menu component associated with the specified item or null if none exists.</returns>
	public ItemStackEntryController GetItemStack(Item item)
	{
		// Iterate through the item list's children and find the item.
		foreach(Transform child in this.itemList.transform)
		{
    		ItemStackEntryController stack = child.gameObject.GetComponent<ItemStackEntryController>();

			if(stack != null && item == stack.GetItem())
			{
				return stack;
			}
		}

		return null;
	}

	/// <summary>
	/// Move the current selection by a specified offset on the inventory 
	/// grid. The slection will loop around if the selection is outside the 
	/// bounds of the inventory grid.
	/// </summary>
	/// <param name="offset">The selection offset.</param>
	public void MoveSelection(Vector2Int offset)
	{
		// Return if the inventory is empty.
		if(this.itemList.transform.childCount == 0)
		{
			return;
		}

		Vector2Int grid = this.GetGridSize();
		Vector2Int selectedPos = this.GetSelectedPosition();

		// Change the bounds if the current selection is on the incomplete row.
		int lastRowWidth =((this.itemList.transform.childCount - 1) % grid.x) + 1;
		int width = selectedPos.y == grid.y - 1 ? lastRowWidth : grid.x;
		int height = selectedPos.x >= lastRowWidth ? grid.y - 1 : grid.y;

		// Move the selection.
		selectedPos += offset;
		selectedPos.x = selectedPos.x < 0 ? width -(Math.Abs(selectedPos.x + 1) % width) - 1 : selectedPos.x % width;
		selectedPos.y = selectedPos.y < 0 ? height -(Math.Abs(selectedPos.y + 1) % height) - 1 : selectedPos.y % height;

		// Translate the coordinates to an index.
		int index =(selectedPos.y * grid.x + selectedPos.x) % this.itemList.transform.childCount;

		// Toggle the new selected stack.
		if(this.itemList.transform.GetChild(index).TryGetComponent(out Toggle stack))
		{
			stack.isOn = true;
		}
	}

	/// <summary>
	/// Gets the grid coordinates of the currently selected item stack.
	/// </summary>
	/// <returns>The grid coordinates of the currently selected item stack.</returns>
	public Vector2Int GetSelectedPosition()
	{
		Vector2Int grid = this.GetGridSize();

		// Width will only be zero if no items exist, so return.
		if(grid.x == 0)
		{
			grid.Set(-1, -1);
			return grid;
		}

		ItemStackEntryController selected = this.GetSelectedStack();
		
		// Return if no stack is selected.
		if(selected == null)
		{
			grid.Set(-1, -1);
			return grid;
		}

		// Translate the selected stack's index to grid coordinates.
		grid.Set(selected.transform.GetSiblingIndex() % grid.x, selected.transform.GetSiblingIndex() / grid.x);
		return grid;
	}

	/// <summary>
	/// Gets the current width and height of the inventory grid.
	/// </summary>
	/// <returns>The current width and height of the inventory grid.</returns>
	public Vector2Int GetGridSize()
	{
		// Return if the inventory is empty or if the grid layout group doesn't exist.
		if(this.itemList.transform.childCount == 0 || !this.itemList.gameObject.TryGetComponent(out GridLayoutGroup grid))
		{
			return Vector2Int.zero;
		}

		// Switch between grid layout constraints
		switch(grid.constraint)
		{
		// If the column count is fixed, only the row count needs to be found.
		case GridLayoutGroup.Constraint.FixedColumnCount:
			int rowCount = this.itemList.transform.childCount / grid.constraintCount + Mathf.Min(1, this.itemList.transform.childCount % grid.constraintCount);
			return new Vector2Int(grid.constraintCount, rowCount);

		// If the row count is fixed, only the column count needs to be found.
		case GridLayoutGroup.Constraint.FixedRowCount:
			int columnCount = this.itemList.transform.childCount / grid.constraintCount + Mathf.Min(1, this.itemList.transform.childCount % grid.constraintCount);
			return new Vector2Int(columnCount, grid.constraintCount);
		
		// If nothing is fixed, oof.
		case GridLayoutGroup.Constraint.Flexible:
			int gridWidth = 0;
			float prevX = float.NegativeInfinity;
			
			// Find the width by iterating through the item stack's until a wrap around is detected.
			for(int i = 0; i < this.itemList.transform.childCount; ++i)
			{
				float x =((RectTransform) grid.transform.GetChild(i)).anchoredPosition.x;

				if(x <= prevX)
				{
					break;
				}

				prevX = x;
				++gridWidth;
			}

			// Calculate the height using the width.
			int gridHeight = this.itemList.transform.childCount / gridWidth + Mathf.Min(1, this.itemList.transform.childCount % gridWidth);
			return new Vector2Int(gridWidth, gridHeight);
		default:
			// Achievement Get: How did we get here?
			return Vector2Int.zero;
		}
	}
}