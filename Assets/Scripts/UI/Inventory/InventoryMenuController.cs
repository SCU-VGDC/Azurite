using System;
using TMPro;
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

    private InspectMenuBase childMenu;

    public InventoryMenuController Init(Inventory associatedInventory)
    {
        associatedInventory.itemAddedEvent.AddListener(AddItemEntry);
        associatedInventory.itemRemovedEvent.AddListener(RemoveItemEntry);
        associatedInventory.itemChangedEvent.AddListener(UpdateItemEntry);

        Item[] items = associatedInventory.GetItems();

        for (int i = 0; i < items.Length; ++i)
        {
            AddItemEntry(associatedInventory, items[i]);
        }

        MoveSelection(-GetSelectedPosition());
        return this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelection(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(Vector2Int.up);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelection(Vector2Int.left);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelection(Vector2Int.right);
        }

        // Open the inspect menu when space is pressed.
        if (childMenu == null && Input.GetKeyDown(KeyCode.Space))
        {
            Item selected = GetSelectedItem();

            if (selected == null)
            {
                return;
            }

            childMenu = Instantiate(selected.GetInspectMenuPrefab(), transform.parent).Init(selected);
            childMenu.transform.SetParent(transform);
            childMenu.gameObject.SetActive(false);
            childMenu.onClose.AddListener(() =>
            {
                childMenu = null;
                Open();
            });

            onClose.AddListener(OpenInspectMenu);
            Close();
        }
    }

    private void OpenInspectMenu()
    {
        gameObject.SetActive(false);
        childMenu.gameObject.SetActive(true);
        childMenu.Open();
        onClose.RemoveListener(OpenInspectMenu);
    }

    protected virtual void AddItemEntry(Inventory inventory, Item item)
    {
        ItemStackEntryController stack = Instantiate(itemStackPrefab, itemList.transform).Init(inventory, item);

        if (stack.TryGetComponent(out Toggle toggle))
        {
            toggle.group = itemList;
            toggle.onValueChanged.AddListener(UpdateItemName);

            if (itemList.transform.childCount == 1)
            {
                UpdateItemName(false);
            }
        }
    }

    protected virtual void RemoveItemEntry(Inventory inventory, Item item)
    {
        ItemStackEntryController stack = GetItemStack(item);

        if (stack != null)
        {
            Destroy(stack.gameObject);
        }
    }

    protected virtual void UpdateItemEntry(Inventory inventory, Item item, int amount)
    {
        ItemStackEntryController stack = GetItemStack(item);

        if (stack != null)
        {
            stack.Refresh();
        }
    }

    protected virtual void UpdateItemName(bool _)
    {
        if (itemName == null)
        {
            return;
        }

        Item selected = GetSelectedItem();

        if (selected != null)
        {
            itemName.SetText(selected.GetDisplayName());
            itemName.enabled = true;

            // Unity says we shouldn't use this function but it works :/
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)itemName.transform);
        }
        else
        {
            itemName.enabled = false;
        }
    }

    public ItemStackEntryController GetSelectedStack()
    {
        Toggle selected = itemList.GetFirstActiveToggle();
        return selected != null ? selected.gameObject.GetComponent<ItemStackEntryController>() : null;
    }

    public Item GetSelectedItem()
    {
        ItemStackEntryController selectedStack = GetSelectedStack();
        return selectedStack != null ? selectedStack.GetItem() : null;
    }

    public ItemStackEntryController GetItemStack(Item item)
    {
        foreach (Transform child in itemList.transform)
        {
            ItemStackEntryController stack = child.gameObject.GetComponent<ItemStackEntryController>();

            if (stack != null && item == stack.GetItem())
            {
                return stack;
            }
        }

        return null;
    }

    public void MoveSelection(Vector2Int offset)
    {
        // Return if the inventory is empty.
        if (itemList.transform.childCount == 0)
        {
            return;
        }

        Vector2Int grid = GetGridSize();
        Vector2Int selectedPos = GetSelectedPosition();

        // Change the bounds if the current selection is on the incomplete row.
        int lastRowWidth = ((itemList.transform.childCount - 1) % grid.x) + 1;
        int width = selectedPos.y == grid.y - 1 ? lastRowWidth : grid.x;
        int height = selectedPos.x >= lastRowWidth ? grid.y - 1 : grid.y;

        // Move the selection.
        selectedPos += offset;
        selectedPos.x = selectedPos.x < 0 ? width - (Math.Abs(selectedPos.x + 1) % width) - 1 : selectedPos.x % width;
        selectedPos.y = selectedPos.y < 0 ? height - (Math.Abs(selectedPos.y + 1) % height) - 1 : selectedPos.y % height;

        // Translate the coordinates to an index.
        int index = (selectedPos.y * grid.x + selectedPos.x) % itemList.transform.childCount;

        // Toggle the new selected stack.
        if (itemList.transform.GetChild(index).TryGetComponent(out Toggle stack))
        {
            stack.isOn = true;
        }
    }

    public Vector2Int GetSelectedPosition()
    {
        Vector2Int grid = GetGridSize();

        // Width will only be zero if no items exist, so return.
        if (grid.x == 0)
        {
            grid.Set(-1, -1);
            return grid;
        }

        ItemStackEntryController selected = GetSelectedStack();

        // Return if no stack is selected.
        if (selected == null)
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
        if (itemList.transform.childCount == 0 || !itemList.gameObject.TryGetComponent(out GridLayoutGroup grid))
        {
            return Vector2Int.zero;
        }

        // Switch between grid layout constraints
        switch (grid.constraint)
        {
            // If the column count is fixed, only the row count needs to be found.
            case GridLayoutGroup.Constraint.FixedColumnCount:
                int rowCount = itemList.transform.childCount / grid.constraintCount + Mathf.Min(1, itemList.transform.childCount % grid.constraintCount);
                return new Vector2Int(grid.constraintCount, rowCount);

            // If the row count is fixed, only the column count needs to be found.
            case GridLayoutGroup.Constraint.FixedRowCount:
                int columnCount = itemList.transform.childCount / grid.constraintCount + Mathf.Min(1, itemList.transform.childCount % grid.constraintCount);
                return new Vector2Int(columnCount, grid.constraintCount);

            // If the grid is felxible, oof.
            case GridLayoutGroup.Constraint.Flexible:
                int gridWidth = 0;
                float prevX = float.NegativeInfinity;

                // Find the width by iterating through the item stack's until a wrap around is detected.
                for (int i = 0; i < itemList.transform.childCount; ++i)
                {
                    float x = ((RectTransform)grid.transform.GetChild(i)).anchoredPosition.x;

                    if (x <= prevX)
                    {
                        break;
                    }

                    prevX = x;
                    ++gridWidth;
                }

                // Calculate the height using the width.
                int gridHeight = itemList.transform.childCount / gridWidth + Mathf.Min(1, itemList.transform.childCount % gridWidth);
                return new Vector2Int(gridWidth, gridHeight);
            default:
                // Achievement Get: How did we get here?
                return Vector2Int.zero;
        }
    }
}