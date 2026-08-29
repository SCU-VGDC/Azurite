using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public UnityEvent<Item> onItemAdded = new();
    public UnityEvent<Item> onItemRemoved = new();
    public UnityEvent<Item, int> onItemCountChanged = new();

    public Item[] Items => items.SelectMany(kv => Enumerable.Repeat(kv.Key, kv.Value)).ToArray();

    private readonly Dictionary<Item, int> items = new();

    public bool HasItem(Item item)
    {
        return items.ContainsKey(item);
    }

    public int GetCount(Item item)
    {
        return items.GetValueOrDefault(item, 0);
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

        amount = Math.Max(0, Math.Min(item.MaxStackSize - GetCount(item), amount));

        if (items.TryAdd(item, amount))
            onItemAdded.Invoke(item);
        else
            items[item] += amount;

        if (amount > 0)
            onItemCountChanged.Invoke(item, GetCount(item));

        return amount;
    }

    /// <summary>
    /// Remove a quantity of an item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <param name="amount">The quantity to remove.</param>
    /// <returns>The amount of items successfully removed from the inventory.</returns>
    public int RemoveItem(Item item, int amount)
    {
        if (amount <= 0 || !HasItem(item))
        {
            return 0;
        }

        amount = Math.Min(amount, GetCount(item));
        items[item] -= amount;
        onItemCountChanged.Invoke(item, GetCount(item));

        if (items[item] <= 0)
        {
            items.Remove(item);
            onItemRemoved.Invoke(item);
        }

        return amount;
    }
}