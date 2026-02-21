using System;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuController : MenuBase
{
	[Tooltip("The item stack slot prefab.")]
	[SerializeField] protected ItemStackEntryController itemStackPrefab = null;

	[Tooltip("The toggle group containing the item stacks.")]
	[SerializeField] protected ToggleGroup itemList = null;

	[Tooltip("The item name text box.")]
	[SerializeField] protected TextMeshProUGUI itemName = null;

	public InventoryMenuController Init(Inventory associatedInventory)
	{
		associatedInventory.itemAddedEvent.AddListener(this.AddItemEntry);
		associatedInventory.itemRemovedEvent.AddListener(this.RemoveItemEntry);
		associatedInventory.itemChangedEvent.AddListener(this.UpdateItemEntry);

		Item[] items = associatedInventory.GetItems();

		for(int i = 0; i < items.Length; ++i)
		{
			this.AddItemEntry(associatedInventory, items[i]);
		}

		this.MoveSelection(-this.GetSelectedPosition());
		return this;
	}

	public override void Update()
	{
		base.Update();

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
				this.childMenu = null;
				this.Open();
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

	protected virtual void AddItemEntry(Inventory inventory, Item item)
	{
		ItemStackEntryController stack = Instantiate(this.itemStackPrefab, this.itemList.transform).Init(inventory, item);

		if(stack.TryGetComponent(out Toggle toggle))
		{
			toggle.group = this.itemList;
			toggle.onValueChanged.AddListener(this.UpdateItemName);

			if(this.itemList.transform.childCount == 1)
			{
				this.UpdateItemName(false);
			}
		}
	}

	protected virtual void RemoveItemEntry(Inventory inventory, Item item)
	{
		ItemStackEntryController stack = this.GetItemStack(item);

		if(stack != null)
		{
			Destroy(stack.gameObject);
		}
	}

	protected virtual void UpdateItemEntry(Inventory inventory, Item item, int amount)
	{
		ItemStackEntryController stack = this.GetItemStack(item);

		if(stack != null)
		{
			stack.Refresh();
		}
	}

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

	public ItemStackEntryController GetSelectedStack()
	{
		Toggle selected = this.itemList.GetFirstActiveToggle();
		return selected != null ? selected.gameObject.GetComponent<ItemStackEntryController>() : null;
	}

	public Item GetSelectedItem()
	{
		ItemStackEntryController selectedStack = this.GetSelectedStack();
		return selectedStack != null ? selectedStack.GetItem() : null;
	}

	public ItemStackEntryController GetItemStack(Item item)
	{
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
		
		// If the grid is felxible, oof.
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