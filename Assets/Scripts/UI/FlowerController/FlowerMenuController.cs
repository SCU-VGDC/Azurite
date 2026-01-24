using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlowerMenuController : MenuBase
{
	[Tooltip("The inventory to display flowers from.")]
	[SerializeField] protected Inventory inventory = null;

	[Tooltip("The item stack slot prefab.")]
	[SerializeField] protected ItemStackEntryController itemStackPrefab = null;

	[Tooltip("The toggle group containing the flower stacks.")]
	[SerializeField] protected ToggleGroup itemList = null;

	[Tooltip("The flower name text box.")]
	[SerializeField] protected TextMeshProUGUI itemName = null;

	/// <summary>
	/// Initializes the flower menu with the associated inventory.
	/// </summary>
	/// <param name="associatedInventory">The inventory to display flowers from.</param>
	/// <returns>The initialized FlowerMenuController.</returns>
	public FlowerMenuController Init(Inventory associatedInventory)
	{
		this.inventory = associatedInventory;

		// Subscribe to inventory events, but filter for flowers only
		this.inventory.itemAddedEvent.AddListener(this.OnItemAdded);
		this.inventory.itemRemovedEvent.AddListener(this.OnItemRemoved);
		this.inventory.itemChangedEvent.AddListener(this.OnItemChanged);

		// Get all items and filter for flowers
		Item[] allItems = this.inventory.GetItems();
		List<Item> flowerItems = new List<Item>();

		for(int i = 0; i < allItems.Length; ++i)
		{
			if(this.IsFlower(allItems[i]))
			{
				flowerItems.Add(allItems[i]);
			}
		}

		// Add flower entries to the menu
		for(int i = 0; i < flowerItems.Count; ++i)
		{
			this.AddItemEntry(flowerItems[i]);
		}

		return this;
	}

	private void Start()
	{
		// Initialize on creation, similar to how inventory menu works
		// Find the player and initialize with their inventory
		// Using Start() instead of Awake() to ensure GameManager and Player are initialized first
		if(this.inventory == null)
		{
			// Try GameManager first (more reliable if available)
			if(GameManager.inst != null && GameManager.inst.player != null && GameManager.inst.player.Inventory != null)
			{
				this.Init(GameManager.inst.player.Inventory);
			}
			else
			{
				// Fallback to finding by tag
				GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
				if(playerObject != null)
				{
					Player player = playerObject.GetComponent<Player>();
					if(player != null && player.Inventory != null)
					{
						this.Init(player.Inventory);
					}
				}
			}
		}
	}

	/// <summary>
	/// Public method to open/close the flower menu. Can be called from InteractionTrigger's UnityEvent.
	/// </summary>
	public void ToggleFlowerMenu()
	{
		// Toggle the menu
		if(this.IsOpen())
		{
			this.Close();
		}
		else
		{
			this.Open();
		}
	}

	/// <summary>
	/// Checks if an item is a flower.
	/// </summary>
	/// <param name="item">The item to check.</param>
	/// <returns>True if the item is a flower, false otherwise.</returns>
	private bool IsFlower(Item item)
	{
		if(item == null)
		{
			return false;
		}

		Item.Category[] categories = item.GetCategories();
		
		if(categories == null)
		{
			return false;
		}

		foreach(Item.Category category in categories)
		{
			if(category == Item.Category.FLOWER)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Handles when an item is added to the inventory. Only processes flowers.
	/// </summary>
	/// <param name="item">The item that was added.</param>
	private void OnItemAdded(Item item)
	{
		if(this.IsFlower(item))
		{
			this.AddItemEntry(item);
		}
	}

	/// <summary>
	/// Handles when an item is removed from the inventory. Only processes flowers.
	/// </summary>
	/// <param name="item">The item that was removed.</param>
	private void OnItemRemoved(Item item)
	{
		if(this.IsFlower(item))
		{
			this.RemoveItemEntry(item);
		}
	}

	/// <summary>
	/// Handles when an item count changes in the inventory. Only processes flowers.
	/// </summary>
	/// <param name="item">The item that changed.</param>
	private void OnItemChanged(Item item)
	{
		if(this.IsFlower(item))
		{
			this.UpdateItemEntry(item);
		}
	}

	public override void Update()
	{
		base.Update();

		// Handle flower menu traversal with WASD or arrow keys.
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
	/// Add a flower stack to the flower menu. This does not actually 
	/// add an item to the underlying inventory and is used only for 
	/// updating the menu.
	/// </summary>
	/// <param name="item">The flower item to add.</param>
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
	/// Remove a flower stack from the flower menu. This does not actually 
	/// remove an item from the underlying inventory and is only used for 
	/// updating the menu.
	/// </summary>
	/// <param name="item">The flower item to remove.</param>
	protected virtual void RemoveItemEntry(Item item)
	{
		ItemStackEntryController stack = this.GetItemStack(item);

		if(stack != null)
		{
			Destroy(stack.gameObject);
		}
	}

	/// <summary>
	/// Update a flower stack in the flower menu. This refreshes the flower 
	/// stack's stack count label.
	/// </summary>
	/// <param name="item">The flower item to update.</param>
	protected virtual void UpdateItemEntry(Item item)
	{
		ItemStackEntryController stack = this.GetItemStack(item);

		if(stack != null)
		{
			stack.Refresh();
		}
	}

	/// <summary>
	/// Update the flower name in the currently selected flower text box.
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
	/// Gets the currently selected flower stack menu component.
	/// </summary>
	/// <returns>The currently selected flower stack menu component or null if none exists.</returns>
	public ItemStackEntryController GetSelectedStack()
	{
		Toggle selected = this.itemList.GetFirstActiveToggle();
		return selected != null ? selected.gameObject.GetComponent<ItemStackEntryController>() : null;
	}

	/// <summary>
	/// Gets the currently selected flower.
	/// </summary>
	/// <returns>The currently selected flower or null if none exists.</returns>
	public Item GetSelectedItem()
	{
		ItemStackEntryController selectedStack = this.GetSelectedStack();
		return selectedStack != null ? selectedStack.GetItem() : null;
	}

	/// <summary>
	/// Gets the flower stack menu component associated with the specified flower.
	/// </summary>
	/// <param name="item">The flower to search for.</param>
	/// <returns>The flower stack menu component associated with the specified flower or null if none exists.</returns>
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
	/// Move the current selection by a specified offset on the flower 
	/// grid. The selection will loop around if the selection is outside the 
	/// bounds of the flower grid.
	/// </summary>
	/// <param name="offset">The selection offset.</param>
	public void MoveSelection(Vector2Int offset)
	{
		// Return if the flower menu is empty.
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
	/// Gets the grid coordinates of the currently selected flower stack.
	/// </summary>
	/// <returns>The grid coordinates of the currently selected flower stack.</returns>
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
	/// Gets the current width and height of the flower grid.
	/// </summary>
	/// <returns>The current width and height of the flower grid.</returns>
	public Vector2Int GetGridSize()
	{
		// Return if the flower menu is empty or if the grid layout group doesn't exist.
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