using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class FlowerInventory : Inventory
{
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

    [SerializeField] private RecipeEntry[] recipes;
    private Dictionary<(Item, Item), Item> _craftMap;

    private Transform _leftSlotParent;
    private Transform _rightSlotParent;
    private ItemStackEntryController _slotPrefab;
    private bool _eventsSubscribed;

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
    public void BindCombinerSlots(Transform leftPanel, Transform rightPanel, ItemStackEntryController slotPrefab)
    {
        _leftSlotParent = leftPanel;
        _rightSlotParent = rightPanel;
        _slotPrefab = slotPrefab;
        if (!_eventsSubscribed)
        {
            itemAddedEvent.AddListener((inv, item) => RefreshCombinerSlots());
            itemRemovedEvent.AddListener((inv, item) => RefreshCombinerSlots());
            itemChangedEvent.AddListener((inv, item, amount) => RefreshCombinerSlots());
            _eventsSubscribed = true;
        }
        RefreshCombinerSlots();
    }

    private void RefreshCombinerSlots()
    {
        if (_leftSlotParent == null || _rightSlotParent == null || _slotPrefab == null) return;

        for (int i = _leftSlotParent.childCount - 1; i >= 0; i--)
            Destroy(_leftSlotParent.GetChild(i).gameObject);
        for (int i = _rightSlotParent.childCount - 1; i >= 0; i--)
            Destroy(_rightSlotParent.GetChild(i).gameObject);

        Item[] items = GetItems();
        if (items.Length > 0)
        {
            var left = Instantiate(_slotPrefab, _leftSlotParent).Init(this, items[0]);
            if (left.TryGetComponent(out Toggle t)) { t.group = null; t.interactable = false; }
        }
        if (items.Length > 1)
        {
            var right = Instantiate(_slotPrefab, _rightSlotParent).Init(this, items[1]);
            if (right.TryGetComponent(out Toggle t)) { t.group = null; t.interactable = false; }
        }
    }

    [Tooltip("The item given to the player when a combination fails to match a recipe.")]
    [SerializeField] private Item failedCombinationItem;

    public Item Combine()
    {
        Item[] items = GetItems();
        if (_craftMap == null || items.Length != 2) return null;

        if (!_craftMap.TryGetValue((items[0], items[1]), out Item result) || result == null)
        {
            result = failedCombinationItem;
        }

        if (result == null) return null;

        RemoveItem(items[0], 1);
        RemoveItem(items[1], 1);

        GameManager.inst?.player?.Inventory?.AddItem(result, 1);
        return result;
    }

    public void AddFlower(Item item)
    {
        if (this.GetItems().Length >= 2) return;

        if (item.GetCategories() == null || Array.IndexOf(item.GetCategories(), Item.Category.FLOWER) < 0)
        {
            Debug.Log($"Cannot add {item.GetDisplayName()} to the combiner.");
            return;
        }

        Inventory player = GameManager.inst?.player?.Inventory;
        if (player == null || !player.HasItem(item)) return;

        AddItem(item, 1);
        player.RemoveItem(item, 1);
    }

    public void RemoveFlower(Item item)
    {
        RemoveItem(item, 1);
        GameManager.inst?.player?.Inventory?.AddItem(item, 1);
    }
}
