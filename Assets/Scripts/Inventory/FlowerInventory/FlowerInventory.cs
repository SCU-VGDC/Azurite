using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class FlowerInventory : MonoBehaviour
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

    public Item Slot1 { get; private set; }
    public Item Slot2 { get; private set; }

    public UnityEngine.Events.UnityEvent contentChangedEvent = new UnityEngine.Events.UnityEvent();

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
            Inventory dummy = left.gameObject.AddComponent<Inventory>();
            dummy.AddItem(Slot1, 1);
            left.Init(dummy, Slot1);
            if (left.TryGetComponent(out Toggle t)) { t.group = null; t.interactable = false; }
        }
        if (Slot2 != null)
        {
            var right = Instantiate(_slotPrefab, _rightSlotParent);
            Inventory dummy = right.gameObject.AddComponent<Inventory>();
            dummy.AddItem(Slot2, 1);
            right.Init(dummy, Slot2);
            if (right.TryGetComponent(out Toggle t)) { t.group = null; t.interactable = false; }
        }
    }

    [Tooltip("The item given to the player when a combination fails to match a recipe.")]
    [SerializeField] private Item failedCombinationItem;

    public Item Combine()
    {
        if (_craftMap == null || Slot1 == null || Slot2 == null) return null;

        if (!_craftMap.TryGetValue((Slot1, Slot2), out Item result) || result == null)
        {
            result = failedCombinationItem;
        }

        if (result == null) return null;

        Slot1 = null;
        Slot2 = null;
        contentChangedEvent?.Invoke();

        GameManager.inst?.player?.Inventory?.AddItem(result, 1);
        return result;
    }

    public void AddFlower(Item item)
    {
        if (Slot1 != null && Slot2 != null) return;

        if (item.GetCategories() == null || Array.IndexOf(item.GetCategories(), Item.Category.FLOWER) < 0)
        {
            Debug.Log($"Cannot add {item.GetDisplayName()} to the combiner.");
            return;
        }

        Inventory player = GameManager.inst?.player?.Inventory;
        if (player == null || !player.HasItem(item)) return;

        if (Slot1 == null) Slot1 = item;
        else Slot2 = item;

        player.RemoveItem(item, 1);
        contentChangedEvent?.Invoke();
    }

    public void RemoveFlower(Item item)
    {
        if (Slot1 == item) Slot1 = null;
        else if (Slot2 == item) Slot2 = null;
        else return;

        GameManager.inst?.player?.Inventory?.AddItem(item, 1);
        contentChangedEvent?.Invoke();
    }

    public void ReturnItems()
    {
        if (Slot1 != null)
        {
            GameManager.inst?.player?.Inventory?.AddItem(Slot1, 1);
            Slot1 = null;
        }
        if (Slot2 != null)
        {
            GameManager.inst?.player?.Inventory?.AddItem(Slot2, 1);
            Slot2 = null;
        }
        contentChangedEvent?.Invoke();
    }
}
