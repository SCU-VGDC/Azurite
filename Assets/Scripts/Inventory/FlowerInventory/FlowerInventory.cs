using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class FlowerInventory : MonoBehaviour
{
    public const int numSlots = 2;

    [SerializeField] private Item Beanstalk;
    [SerializeField] private Item MapleSapling;
    [SerializeField] private Item MonkeyCup;
    [SerializeField] private Item OrangeSapling;
    [SerializeField] private Item PrettyFlower;


    [Serializable]
    public struct RecipeEntry
    {
        public Item a;
        public Item b;
        public Item result;
    }

    public int FirstOpenSlot
    {
        get
        {
            for (int i = 0; i < numSlots; i++)
                if (items[i] == null)
                    return i;
            return -1;
        }
    }

    public int FirstFilledSlot
    {
        get
        {
            for (int i = 0; i < numSlots; i++)
                if (items[i] != null)
                    return i;
            return -1;
        }
    }

    [SerializeField] private RecipeEntry[] recipes;
    private Dictionary<(Item, Item), Item> _craftMap;

    /*private Transform _leftSlotParent;
    private Transform _rightSlotParent;
    private ItemBox _slotPrefab;*/

    private readonly Item[] items = new Item[numSlots];

    public UnityEngine.Events.UnityEvent contentChangedEvent = new();

    public void Awake()
    {
        _craftMap = new Dictionary<(Item, Item), Item>();
        foreach (var e in recipes)
        {
            if (e.a == null || e.b == null || e.result == null) continue;
            _craftMap[(e.a, e.b)] = e.result;
            _craftMap[(e.b, e.a)] = e.result;
        }
    }

    /// <summary>Bind the two combiner slot panels (left = slot 0, right = slot 1). </summary>
    /*public void BindCombinerSlots(Transform leftPanel, Transform rightPanel, ItemBox slotPrefab)
    {
        _leftSlotParent = leftPanel;
        _rightSlotParent = rightPanel;
        _slotPrefab = slotPrefab;

        contentChangedEvent.RemoveListener(RefreshCombinerSlots);
        contentChangedEvent.AddListener(RefreshCombinerSlots);

        RefreshCombinerSlots();
    }

    private void RefreshCombinerSlots()
    {
        if (_leftSlotParent == null || _rightSlotParent == null || _slotPrefab == null) return;

        for (int i = _leftSlotParent.childCount - 1; i >= 0; i--)
            Destroy(_leftSlotParent.GetChild(i).gameObject);
        for (int i = _rightSlotParent.childCount - 1; i >= 0; i--)
            Destroy(_rightSlotParent.GetChild(i).gameObject);

        if (Slot1 != null)
        {
            var left = Instantiate(_slotPrefab, _leftSlotParent);
            left.Item = Slot1;
            if (left.TryGetComponent(out Toggle t)) { t.group = null; t.interactable = false; }
        }
        if (Slot2 != null)
        {
            var right = Instantiate(_slotPrefab, _rightSlotParent);
            right.Item = Slot2;
            if (right.TryGetComponent(out Toggle t)) { t.group = null; t.interactable = false; }
        }
    }*/

    [Tooltip("The item given to the player when a combination fails to match a recipe.")]
    [SerializeField] private Item failedCombinationItem;

    public ItemSlot Combine()
    {
        if (_craftMap == null || FirstOpenSlot != -1)
            return null;

        if (!_craftMap.TryGetValue((items[0], items[1]), out Item result) || result == null)
        {
            result = failedCombinationItem;
        }

        if (result == null)
            return null;

        for (int i = 0; i < numSlots; i++)
            items[i] = null;

        contentChangedEvent?.Invoke();

        return GameManager.Instance.Player.Inventory.AddItem(result, 1);
    }

    public int AddFlower(Item item)
    {
        int slot = FirstOpenSlot;
        if (slot == -1)
            return -1;

        if (item.Categories == null || Array.IndexOf(item.Categories, Item.Category.FLOWER) < 0)
            return -1;

        var playerInv = GameManager.Instance.Player.Inventory;
        if (playerInv == null || !playerInv.HasItem(item))
            return -1;

        items[slot] = item;
        playerInv.RemoveItem(item, 1);
        contentChangedEvent?.Invoke();

        return slot;
    }

    public ItemSlot RemoveFlower(int slot)
    {
        Item item = items[slot];

        if (item != null)
        {
            items[slot] = null;
            contentChangedEvent?.Invoke();
            return GameManager.Instance.Player.Inventory.AddItem(item, 1);
        }

        return null;
    }

    public void ReturnItems()
    {
        for (int i = 0; i < numSlots; i++)
        {
            if (items[i] != null)
            {
                GameManager.Instance.Player.Inventory.AddItem(items[i], 1);
                items[i] = null;
            }
        }

        contentChangedEvent?.Invoke();
    }
}
