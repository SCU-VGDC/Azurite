using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryPopupController : MenuBase
{
	[Tooltip("This event is called whenever an item is selected from the popup.")]
	public UnityEvent<Inventory, Item> itemSelectedEvent = new UnityEvent<Inventory, Item>();
	
	[Tooltip("The item stack slot prefab.")]
	[SerializeField] protected ItemStackEntryController itemStackPrefab = null;

	[Tooltip("The toggle group containing the item stacks.")]
	[SerializeField] protected ToggleGroup itemList = null;

	[Tooltip("The transform that the popup moves relative to.")]
	[SerializeField] protected Transform relativeTransform = null;

	[Tooltip("The offset from the transform.")]
	[SerializeField] protected Vector3 offset = Vector3.zero;

	private Inventory inventory = null;

	public InventoryPopupController Init(Transform relativePosition, Vector3 relativeOffset, Inventory associatedInventory, Item.Category? filterCategory)
	{
		this.transform.localScale = new Vector3(0.0125f, 0.0125f, 0.0125f);
		this.offset = relativeOffset;
		this.relativeTransform = relativePosition;
		this.inventory = associatedInventory;
		Item[] items = this.inventory.GetItems();

		for(int i = 0; i < items.Length; ++i)
		{
			if(filterCategory == null)
			{
				this.AddItemEntry(this.inventory, items[i]);
				continue;
			}

			Item.Category[] categories = items[i].GetCategories();

			for(int j = 0; j < categories.Length; ++j)
			{
				if(filterCategory == categories[j])
				{
					this.AddItemEntry(this.inventory, items[i]);
					break;
				}
			}
		}

		this.itemSelectedEvent.AddListener((inv, item) => {this.Close();});
		return this;
	}

	public override void Update()
	{
		base.Update();
		this.transform.position = this.relativeTransform.position + this.offset;

		// Handle inventory traversal with WASD or arrow keys.
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.MoveSelection(Vector2Int.down);
		}

		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.MoveSelection(Vector2Int.up);
		}

		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.MoveSelection(Vector2Int.left);
		}

		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.MoveSelection(Vector2Int.right);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Item selected = this.GetSelectedItem();

			if(selected == null)
			{
				return;
			}

			this.itemSelectedEvent.Invoke(this.inventory, selected);
			Debug.Log(selected.name);
		}
	}

	protected void AddItemEntry(Inventory inventory, Item item)
	{
		ItemStackEntryController stack = Instantiate(this.itemStackPrefab, this.itemList.transform).Init(inventory, item);

		if(stack.TryGetComponent(out Toggle toggle))
		{
			toggle.group = this.itemList;
		}
		
		this.UpdateGridSize();
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

	public void MoveSelection(Vector2Int offset)
	{
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

	public Vector2Int GetGridSize()
	{
		// Return if the inventory is empty or if the grid layout group doesn't exist.
		if(this.itemList.transform.childCount == 0 || !this.itemList.gameObject.TryGetComponent(out GridLayoutGroup grid))
		{
			return Vector2Int.zero;
		}

		int gridWidth = 0;
		float prevX = float.NegativeInfinity;
		
		// Find the width by iterating through the item stack's until a wrap around is detected.
		for(int i = 0; i < this.itemList.transform.childCount; ++i)
		{
			float x = ((RectTransform) grid.transform.GetChild(i)).anchoredPosition.x;

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
	}

	public Vector2Int GetSelectedPosition()
	{
		Vector2Int grid = this.GetGridSize();

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

	private void UpdateGridSize()
	{
		if(this.itemList.TryGetComponent(out GridLayoutGroup grid))
		{
			grid.constraintCount = Mathf.FloorToInt(Mathf.Sqrt(this.itemList.transform.childCount - 1)) + 1;
		}
	}
}