using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlowerMenu : MenuBase
{
    [Tooltip("The item stack slot prefab.")]
    [SerializeField]
    protected ItemStackEntryController itemStackPrefab = null;

    [Tooltip("The toggle group containing the item stacks.")]
    [SerializeField]
    protected ToggleGroup itemList = null;

    [Tooltip("The item name text box.")]
    [SerializeField]
    protected TextMeshProUGUI itemName = null;

    [Tooltip("The flower combiner inventory to transfer items to.")]
    [SerializeField]
    protected FlowerInventory flowerInventory = null;

    [Tooltip("The Combine Button (Should not be altered outside of prefab).")]
    [SerializeField]
    protected Button combineButton = null;

    private Dictionary<Item, ItemStackEntryController> itemStacks = new Dictionary<Item, ItemStackEntryController>();
    private GridLayoutGroup gridLayoutGroup;

    public FlowerMenu Init(Inventory associatedInventory, FlowerInventory combiner = null)
    {
        associatedInventory.itemAddedEvent.AddListener(this.AddItemEntry);
        associatedInventory.itemRemovedEvent.AddListener(this.RemoveItemEntry);
        associatedInventory.itemChangedEvent.AddListener(this.UpdateItemEntry);

        Item[] items = associatedInventory.GetItems();

        for (int i = 0; i < items.Length; ++i)
        {
            this.AddItemEntry(associatedInventory, items[i]);
        }

        if (combiner != null)
        {
            this.flowerInventory = combiner;
            Transform leftPanel = this.transform.Find("Left Item Panel");
            Transform rightPanel = this.transform.Find("Right Item Panel");
            if (leftPanel != null && rightPanel != null && this.itemStackPrefab != null)
                combiner.BindCombinerSlots(leftPanel, rightPanel, this.itemStackPrefab);

            if (this.combineButton != null)
            {
                this.combineButton.onClick.RemoveAllListeners();
                this.combineButton.onClick.AddListener(this.OnCombineButtonClicked);
            }
        }

        return this;
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.MoveSelection(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.MoveSelection(Vector2Int.up);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.MoveSelection(Vector2Int.left);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.MoveSelection(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Item selected = this.GetSelectedItem();
            if (selected != null && this.flowerInventory != null)
                this.flowerInventory.AddFlower(selected);
        }
    }

    public void OnCombineButtonClicked()
    {
        if (this.flowerInventory == null) return;

        Item result = this.flowerInventory.Combine();
        if (result != null)
        {
            Debug.Log($"Crafted {result.GetDisplayName()}");

            FlowerMenuController controller = FindFirstObjectByType<FlowerMenuController>();
            if (controller != null)
            {
                controller.CloseMenu();
            }
            else
            {
                this.Close();
            }
        }
    }

    /// <summary>
    /// Add an item stack to the menu. This does not actually
    /// add an item to the underlying inventory and is used only for
    /// updating the menu.
    /// </summary>
    protected virtual void AddItemEntry(Inventory inventory, Item item)
    {
        ItemStackEntryController stack = Instantiate(this.itemStackPrefab, this.itemList.transform)
            .Init(inventory, item);

        this.itemStacks[item] = stack;

        if (stack.TryGetComponent(out Toggle toggle))
        {
            toggle.group = this.itemList;
            toggle.onValueChanged.AddListener(this.UpdateItemName);

            if (this.itemList.transform.childCount == 1)
            {
                this.UpdateItemName(false);
            }
        }
    }

    /// <summary>
    /// Remove an item stack from the menu. This does not actually
    /// remove an item from the underlying inventory and is only used for
    /// updating the menu.
    /// </summary>
    protected virtual void RemoveItemEntry(Inventory inventory, Item item)
    {
        if (this.itemStacks.TryGetValue(item, out ItemStackEntryController stack) && stack != null)
        {
            Destroy(stack.gameObject);
            this.itemStacks.Remove(item);
        }
    }

    /// <summary>
    /// Update an item stack in the menu. This refreshes the item
    /// stack's stack count label.
    /// </summary>
    protected virtual void UpdateItemEntry(Inventory inventory, Item item, int amount)
    {
        ItemStackEntryController stack = this.GetItemStack(item);

        if (stack != null)
        {
            stack.Refresh();
        }
    }

    /// <summary>
    /// Update the item name in the currently selected item text box.
    /// </summary>
    protected virtual void UpdateItemName(bool _)
    {
        if (this.itemName == null)
        {
            return;
        }

        Item selected = this.GetSelectedItem();

        if (selected != null)
        {
            this.itemName.SetText(selected.GetDisplayName());
            this.itemName.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.itemName.transform);
        }
        else
        {
            this.itemName.enabled = false;
        }
    }

    /// <summary>
    /// Gets the currently selected item stack menu component.
    /// </summary>
    public ItemStackEntryController GetSelectedStack()
    {
        Toggle selected = this.itemList.GetFirstActiveToggle();
        return selected != null
            ? selected.gameObject.GetComponent<ItemStackEntryController>()
            : null;
    }

    /// <summary>
    /// Gets the currently selected item.
    /// </summary>
    public Item GetSelectedItem()
    {
        ItemStackEntryController selectedStack = this.GetSelectedStack();
        return selectedStack != null ? selectedStack.GetItem() : null;
    }

    /// <summary>
    /// Gets the item stack menu component associated with the specified item.
    /// </summary>
    public ItemStackEntryController GetItemStack(Item item)
    {
        this.itemStacks.TryGetValue(item, out ItemStackEntryController stack);
        return stack;
    }

    /// <summary>
    /// Move the current selection by a specified offset on the grid.
    /// The selection will loop around if outside the bounds of the grid.
    /// </summary>
    public void MoveSelection(Vector2Int offset)
    {
        if (this.itemList.transform.childCount == 0)
        {
            return;
        }

        Vector2Int grid = this.GetGridSize();
        Vector2Int selectedPos = this.GetSelectedPosition();

        int lastRowWidth = ((this.itemList.transform.childCount - 1) % grid.x) + 1;
        int width = selectedPos.y == grid.y - 1 ? lastRowWidth : grid.x;
        int height = selectedPos.x >= lastRowWidth ? grid.y - 1 : grid.y;

        selectedPos += offset;
        selectedPos.x = ((selectedPos.x % width) + width) % width;
        selectedPos.y = ((selectedPos.y % height) + height) % height;

        int index = (selectedPos.y * grid.x + selectedPos.x) % this.itemList.transform.childCount;

        if (this.itemList.transform.GetChild(index).TryGetComponent(out Toggle stack))
        {
            stack.isOn = true;
        }
    }

    /// <summary>
    /// Gets the grid coordinates of the currently selected item stack.
    /// </summary>
    public Vector2Int GetSelectedPosition()
    {
        Vector2Int grid = this.GetGridSize();

        if (grid.x == 0)
        {
            grid.Set(-1, -1);
            return grid;
        }

        ItemStackEntryController selected = this.GetSelectedStack();

        if (selected == null)
        {
            grid.Set(-1, -1);
            return grid;
        }

        grid.Set(
            selected.transform.GetSiblingIndex() % grid.x,
            selected.transform.GetSiblingIndex() / grid.x
        );
        return grid;
    }

    /// <summary>
    /// Gets the current width and height of the grid.
    /// </summary>
    public Vector2Int GetGridSize()
    {
        if (this.itemList.transform.childCount == 0 || (this.gridLayoutGroup == null && !this.itemList.gameObject.TryGetComponent(out this.gridLayoutGroup)))
        {
            return Vector2Int.zero;
        }

        switch (this.gridLayoutGroup.constraint)
        {
            case GridLayoutGroup.Constraint.FixedColumnCount:
                int rowCount = (this.itemList.transform.childCount + this.gridLayoutGroup.constraintCount - 1) / this.gridLayoutGroup.constraintCount;
                return new Vector2Int(this.gridLayoutGroup.constraintCount, rowCount);

            case GridLayoutGroup.Constraint.FixedRowCount:
                int columnCount = (this.itemList.transform.childCount + this.gridLayoutGroup.constraintCount - 1) / this.gridLayoutGroup.constraintCount;
                return new Vector2Int(columnCount, this.gridLayoutGroup.constraintCount);

            case GridLayoutGroup.Constraint.Flexible:
                int gridWidth = 0;
                float prevX = float.NegativeInfinity;

                for (int i = 0; i < this.itemList.transform.childCount; ++i)
                {
                    float x = ((RectTransform)this.gridLayoutGroup.transform.GetChild(i)).anchoredPosition.x;
                    if (x <= prevX)
                        break;

                    prevX = x;
                    ++gridWidth;
                }

                int gridHeight = (this.itemList.transform.childCount + gridWidth - 1) / gridWidth;
                return new Vector2Int(gridWidth, gridHeight);

            default:
                return Vector2Int.zero;
        }
    }

    protected virtual void OnDestroy()
    {
        if (this.flowerInventory != null)
        {
            this.flowerInventory.ReturnItems();
        }
    }
}
