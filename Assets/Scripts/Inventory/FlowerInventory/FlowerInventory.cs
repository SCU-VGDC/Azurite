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

    /*private Transform _leftSlotParent;
    private Transform _rightSlotParent;
    private ItemBox _slotPrefab;*/

    public Item Slot1 { get; private set; }
    public Item Slot2 { get; private set; }

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

        if (GameManager.Instance != null && GameManager.Instance.Player != null && GameManager.Instance.Player.Inventory != null)
        {
            GameManager.Instance.Player.Inventory.AddItem(result, 1);
        }
        return result;
    }

    public bool AddFlower(Item item)
    {
        if (Slot1 != null && Slot2 != null) return false;

        if (item.Categories == null || Array.IndexOf(item.Categories, Item.Category.FLOWER) < 0)
        {
            Debug.Log($"Cannot add {item.DisplayName} to the combiner.");
            return false;
        }

        Inventory playerInv = null;
        if (GameManager.Instance != null && GameManager.Instance.Player != null)
        {
            playerInv = GameManager.Instance.Player.Inventory;
        }
        if (playerInv == null || !playerInv.HasItem(item)) return false;

        if (Slot1 == null) Slot1 = item;
        else Slot2 = item;

        playerInv.RemoveItem(item, 1);
        contentChangedEvent?.Invoke();
        return true;
    }

    public ItemSlot RemoveFlower(int slot)
    {
        Item item = null;
        if (slot == 0 && Slot1 != null)
        {
            item = Slot1;
            Slot1 = null;
        }
        else if (slot == 1 && Slot2 != null)
        {
            item = Slot2;
            Slot2 = null;
        }

        if (item != null)
        {
            contentChangedEvent?.Invoke();
            return GameManager.Instance.Player.Inventory.AddItem(item, 1);
        }
        return null;
    }

    public void ReturnItems()
    {
        if (Slot1 != null)
        {
            if (GameManager.Instance != null && GameManager.Instance.Player != null && GameManager.Instance.Player.Inventory != null)
            {
                GameManager.Instance.Player.Inventory.AddItem(Slot1, 1);
            }
            Slot1 = null;
        }
        if (Slot2 != null)
        {
            if (GameManager.Instance != null && GameManager.Instance.Player != null && GameManager.Instance.Player.Inventory != null)
            {
                GameManager.Instance.Player.Inventory.AddItem(Slot2, 1);
            }
            Slot2 = null;
        }
        contentChangedEvent?.Invoke();
    }
}
