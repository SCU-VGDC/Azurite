using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [Tooltip("This event is called whenever a new item is added to the inventory.")]
    public UnityEvent<Inventory, Item> itemAddedEvent = new();

    [Tooltip("This event is called whenever an item is completely removed from the inventory.")]
    public UnityEvent<Inventory, Item> itemRemovedEvent = new();

    [Tooltip("This event is called whenever the stacksize of an item is changed.")]
    public UnityEvent<Inventory, Item, int> itemChangedEvent = new();

    [Tooltip("The inventory menu prefab.")]
    [SerializeField] private InventoryMenuController inventoryMenuPrefab = null;

    [Tooltip("The inventory popup prefab.")]
    [SerializeField] private InventoryPopupController inventoryPopupPrefab = null;

    private readonly Dictionary<Item, int> items = new();

    public bool HasItem(Item item)
    {
        return this.items.ContainsKey(item);
    }

    public int GetCount(Item item)
    {
        return this.items.GetValueOrDefault(item, 0);
    }

    public bool SelectItem(Item item)
    {
        return this.items.ContainsKey(item);
    }

    /// <summary>
    /// Add a quantity of an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="amount">The amount to add.</param>
    /// <returns>The amount of items successfully added to the inventory.</returns>
    public int AddItem(Item item, int amount)
    {
        if (amount <= 0)
        {
            return 0;
        }

        amount = Math.Min(item.GetMaxStackSize(), amount);

        if (!this.HasItem(item))
        {
            this.items.Add(item, amount);
            this.itemAddedEvent.Invoke(this, item);
            this.itemChangedEvent.Invoke(this, item, amount);
            return amount;
        }

        if (this.items[item] + amount <= item.GetMaxStackSize())
        {
            this.items[item] += amount;
            this.itemChangedEvent.Invoke(this, item, amount);
            return amount;
        }

        int added = item.GetMaxStackSize() - this.items[item];
        this.items[item] = item.GetMaxStackSize();
        this.itemChangedEvent.Invoke(this, item, added);
        return added;
    }

    /// <summary>
    /// Remove a quantity of an item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <param name="amount">The quantity to remove.</param>
    /// <returns>The amount of items successfully removed from the inventory.</returns>
    public int RemoveItem(Item item, int amount)
    {
        if (amount <= 0 || !this.HasItem(item))
        {
            return 0;
        }

        if (this.items[item] - amount > 0)
        {
            this.items[item] -= amount;
            this.itemChangedEvent.Invoke(this, item, -amount);
            return amount;
        }

        int removed = this.items[item];
        this.items.Remove(item);
        this.itemRemovedEvent.Invoke(this, item);
        this.itemChangedEvent.Invoke(this, item, -removed);
        return removed;
    }

    /// <summary>
    /// Get the items stored in the inventory as an array.
    /// </summary>
    /// <returns>The items in the inventory as an array.</returns>
    public Item[] GetItems()
    {
        Item[] itemArray = new Item[this.items.Count];
        int index = -1;

        foreach (Item i in this.items.Keys)
        {
            itemArray[++index] = i;
        }

        return itemArray;
    }

    public bool IsMenuOpen()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");

        if (canvas == null)
        {
            Debug.Log("Failed to find the main canvas!");
            return false;
        }

        return canvas.transform.GetComponentInChildren<InventoryMenuController>(true) != null;
    }

    public InventoryMenuController GetOpenMenu()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");

        if (canvas == null)
        {
            Debug.Log("Failed to find the main canvas!");
            return null;
        }

        return canvas.transform.GetComponentInChildren<InventoryMenuController>(true);
    }

    public void OpenMenu()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");

        if (canvas == null || canvas.transform.GetComponentInChildren<MenuBase>() != null)
        {
            Debug.Log("A menu is already open!");
            return;
        }

        Instantiate(this.inventoryMenuPrefab, canvas.transform).Init(this).Open();
    }

    public bool IsPopupOpen()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("World Canvas");

        if (canvas == null)
        {
            Debug.Log("Failed to find the world canvas!");
            return false;
        }

        return canvas.transform.GetComponentInChildren<InventoryPopupController>() != null;
    }

    public InventoryPopupController GetOpenPopup()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("World Canvas");

        if (canvas == null)
        {
            Debug.Log("Failed to find the world canvas!");
            return null;
        }

        return canvas.transform.GetComponentInChildren<InventoryPopupController>();
    }

    public void OpenPopup(Transform relativePosition, Vector3 offset, Item.Category? category)
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("World Canvas");

        if (canvas == null)
        {
            Debug.Log("Failed to find the world canvas!");
            return;
        }

        Instantiate(this.inventoryPopupPrefab, canvas.transform).Init(relativePosition, offset, this, category).Open();
    }
}