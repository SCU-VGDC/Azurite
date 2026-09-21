using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class FlowerMenu : Menu
{
    [SerializeField] protected ItemBox itemBoxPrefab = null;

    [SerializeField] protected TextMeshProUGUI itemName = null;
    [SerializeField] private RectTransform[] slots;

    [Tooltip("The flower combiner inventory to transfer items to.")]
    [SerializeField]
    protected FlowerInventory flowerInventory = null;

    [Tooltip("The Combine Button (Should not be altered outside of prefab).")]
    [SerializeField]
    protected Button combineButton = null;

    private InventoryMenu invMenu;
    private bool itemCrafted = false;
    private readonly ItemBox[] uiBoxes = new ItemBox[FlowerInventory.numSlots];

    protected override void OnDestroy()
    {
        if (flowerInventory != null)
            flowerInventory.ReturnItems();

        if (invMenu != null)
        {
            invMenu.OnItemClicked -= OnInventoryItemClicked;
            foreach (var box in invMenu.Boxes)
                box.flashing = false;
        }

        base.OnDestroy();
    }

    protected override Tween AnimateOnOpen()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.blocksRaycasts = true;
        return cg.DOFade(1, 0.3f);
    }

    protected override Tween AnimateOnClose()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        return cg.DOFade(0, 0.3f);
    }

    public FlowerMenu Init(FlowerInventory combiner)
    {
        flowerInventory = combiner;

        if (combineButton != null)
        {
            combineButton.onClick.RemoveAllListeners();
            combineButton.onClick.AddListener(OnCombineButtonClicked);
        }

        invMenu = GetComponentInParent<InventoryMenu>();
        if (invMenu == null)
            Debug.LogError("A FlowerMenu should be a child of an InventoryMenu");
        else
            invMenu.OnItemClicked += OnInventoryItemClicked;

        foreach (var box in invMenu.Boxes)
            box.flashing = box.Item.Categories.Contains(Item.Category.FLOWER);

        return this;
    }

    private void OnInventoryItemClicked(ItemBox itemBox)
    {
        if (itemCrafted)
            return;

        if (!itemBox.Item.Categories.Contains(Item.Category.FLOWER))
            return;

        var slotIndex = flowerInventory.AddFlower(itemBox.Item);
        if (slotIndex != -1)
        {
            var targetSlot = slots[slotIndex];
            var newBox = Instantiate(itemBoxPrefab, targetSlot.transform);
            newBox.Item = itemBox.Item;
            newBox.AnimateItemTransfer(itemBox.GetComponent<RectTransform>(), targetSlot);
            newBox.OnClick += () => OnFlowerSlotClicked(slotIndex, newBox);

            uiBoxes[slotIndex] = newBox;
        }
    }

    private void OnFlowerSlotClicked(int slot, ItemBox itemBox)
    {
        var playerInvSlot = flowerInventory.RemoveFlower(slot);
        if (playerInvSlot == null)
            return;

        var target = invMenu.GetUIForSlot(playerInvSlot);
        target.Visible = false;
        target.flashing = true;
        var newBox = Instantiate(itemBoxPrefab, invMenu.transform);
        newBox.Item = playerInvSlot.item;
        newBox.AnimateItemTransfer(itemBox.GetComponent<RectTransform>(), target.transform).onComplete += () =>
        {
            Destroy(newBox.gameObject);
            target.Visible = true;
        };
        Destroy(itemBox.gameObject);
        
    }

    public void OnCombineButtonClicked()
    {
        if (flowerInventory == null || itemCrafted)
            return;

        var slot = flowerInventory.Combine();
        if (slot == null)
            return;

        itemCrafted = true;
        var craftedItemBox = invMenu.GetUIForSlot(slot);
        craftedItemBox.Visible = false;

        for (int i = 0; i < uiBoxes.Length; i++)
        {
            var box = uiBoxes[i];
            var go = box.gameObject;
            if (box != null)
            {
                box.AnimateItemTransfer(combineButton.transform).onComplete += () => Destroy(go);
                uiBoxes[i] = null;
            }
        }

        var transferBox = Instantiate(itemBoxPrefab, invMenu.transform);
        transferBox.CenterPivot();
        transferBox.Item = slot.item;
        var boxRt = transferBox.GetComponent<RectTransform>();
        boxRt.position = combineButton.transform.position;
        boxRt.DOAnchorPos(boxRt.anchoredPosition + Vector2.up * 150, 0.8f).SetEase(Ease.OutBack).SetDelay(0.55f).onComplete += () =>
        {
            transferBox.AnimateItemTransfer(craftedItemBox.transform).onComplete += () =>
            {
                if (IsOpen)
                    Close();
                Destroy(transferBox.gameObject);
                craftedItemBox.Visible = true;
            };
        };
    }

    /*

    /// <summary>
    /// Add an item stack to the menu. This does not actually
    /// add an item to the underlying inventory and is used only for
    /// updating the menu.
    /// </summary>
    protected virtual void AddItemEntry(Item item)
    {
        ItemStackEntryController stack = Instantiate(itemStackPrefab, itemList.transform)
            .Init(GameManager.Instance.Player.Inventory, item);

        itemStacks[item] = stack;

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

    /// <summary>
    /// Remove an item stack from the menu. This does not actually
    /// remove an item from the underlying inventory and is only used for
    /// updating the menu.
    /// </summary>
    protected virtual void RemoveItemEntry(Item item)
    {
        if (itemStacks.TryGetValue(item, out ItemStackEntryController stack) && stack != null)
        {
            Destroy(stack.gameObject);
            itemStacks.Remove(item);
        }
    }

    /// <summary>
    /// Update an item stack in the menu. This refreshes the item
    /// stack's stack count label.
    /// </summary>
    protected virtual void UpdateItemEntry(Item item, int amount)
    {
        ItemStackEntryController stack = GetItemStack(item);

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
        if (itemName == null)
        {
            return;
        }

        Item selected = GetSelectedItem();

        if (selected != null)
        {
            itemName.SetText(selected.DisplayName);
            itemName.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)itemName.transform);
        }
        else
        {
            itemName.enabled = false;
        }
    }

    /// <summary>
    /// Gets the currently selected item stack menu component.
    /// </summary>
    public ItemStackEntryController GetSelectedStack()
    {
        Toggle selected = itemList.GetFirstActiveToggle();
        return selected != null
            ? selected.gameObject.GetComponent<ItemStackEntryController>()
            : null;
    }

    /// <summary>
    /// Gets the currently selected item.
    /// </summary>
    public Item GetSelectedItem()
    {
        ItemStackEntryController selectedStack = GetSelectedStack();
        return selectedStack != null ? selectedStack.GetItem() : null;
    }

    /// <summary>
    /// Gets the item stack menu component associated with the specified item.
    /// </summary>
    public ItemStackEntryController GetItemStack(Item item)
    {
        itemStacks.TryGetValue(item, out ItemStackEntryController stack);
        return stack;
    }

    /// <summary>
    /// Move the current selection by a specified offset on the grid.
    /// The selection will loop around if outside the bounds of the grid.
    /// </summary>
    public void MoveSelection(Vector2Int offset)
    {
        if (itemList.transform.childCount == 0)
        {
            return;
        }

        Vector2Int grid = GetGridSize();
        Vector2Int selectedPos = GetSelectedPosition();

        int lastRowWidth = ((itemList.transform.childCount - 1) % grid.x) + 1;
        int width = selectedPos.y == grid.y - 1 ? lastRowWidth : grid.x;
        int height = selectedPos.x >= lastRowWidth ? grid.y - 1 : grid.y;

        selectedPos += offset;
        selectedPos.x = ((selectedPos.x % width) + width) % width;
        selectedPos.y = ((selectedPos.y % height) + height) % height;

        int index = ((selectedPos.y * grid.x) + selectedPos.x) % itemList.transform.childCount;

        if (itemList.transform.GetChild(index).TryGetComponent(out Toggle stack))
        {
            stack.isOn = true;
        }
    }

    /// <summary>
    /// Gets the grid coordinates of the currently selected item stack.
    /// </summary>
    public Vector2Int GetSelectedPosition()
    {
        Vector2Int grid = GetGridSize();

        if (grid.x == 0)
        {
            grid.Set(-1, -1);
            return grid;
        }

        ItemStackEntryController selected = GetSelectedStack();

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
        if (itemList.transform.childCount == 0 || (gridLayoutGroup == null && !itemList.gameObject.TryGetComponent(out gridLayoutGroup)))
        {
            return Vector2Int.zero;
        }

        switch (gridLayoutGroup.constraint)
        {
            case GridLayoutGroup.Constraint.FixedColumnCount:
                int rowCount = (itemList.transform.childCount + gridLayoutGroup.constraintCount - 1) / gridLayoutGroup.constraintCount;
                return new Vector2Int(gridLayoutGroup.constraintCount, rowCount);

            case GridLayoutGroup.Constraint.FixedRowCount:
                int columnCount = (itemList.transform.childCount + gridLayoutGroup.constraintCount - 1) / gridLayoutGroup.constraintCount;
                return new Vector2Int(columnCount, gridLayoutGroup.constraintCount);

            case GridLayoutGroup.Constraint.Flexible:
                int gridWidth = 0;
                float prevX = float.NegativeInfinity;

                for (int i = 0; i < itemList.transform.childCount; ++i)
                {
                    float x = ((RectTransform)gridLayoutGroup.transform.GetChild(i)).anchoredPosition.x;
                    if (x <= prevX)
                        break;

                    prevX = x;
                    ++gridWidth;
                }

                int gridHeight = (itemList.transform.childCount + gridWidth - 1) / gridWidth;
                return new Vector2Int(gridWidth, gridHeight);

            default:
                return Vector2Int.zero;
        }
    }

    */
}
