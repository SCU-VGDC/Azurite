using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public UnityEvent<ItemSlot> onItemAdded = new();
    public UnityEvent<ItemSlot> onItemRemoved = new();
    public UnityEvent<ItemSlot, int> onItemCountChanged = new();

    public Item[] Items => slots.SelectMany(slot => Enumerable.Repeat(slot.item, slot.count)).ToArray();

    private readonly List<ItemSlot> slots = new();

    public bool HasItem(Item item)
    {
        return slots.Any(slot => slot.item == item);
    }

    public int GetCount(Item item)
    {
        return slots.Sum(slot => slot.item == item ? slot.count : 0);
    }

    public void Clear()
    {
        while (slots.Count > 0)
            RemoveItem(slots[^1], slots[^1].count);
    }

    /// <summary>
    /// Add a quantity of an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="amount">The amount to add.</param>
    public ItemSlot AddItem(Item item, int amount, out int amountAdded)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException("amount", "amount must be greater than 0");

        var slot = slots.FirstOrDefault(slot => slot.item == item);

        if (slot == null)
        {
            amount = Math.Min(item.MaxStackSize, amount);
            slot = new ItemSlot()
            {
                item = item,
                count = amount
            };
            slots.Add(slot);
            onItemAdded.Invoke(slot);
        }
        else
        {
            amount = Math.Max(0, Math.Min(item.MaxStackSize - slot.count, amount));
            slot.count += amount;
        }

        onItemCountChanged.Invoke(slot, GetCount(item));

        amountAdded = amount;
        return slot;
    }

    public ItemSlot AddItem(Item item, int amount)
    {
        return AddItem(item, amount, out _);
    }

    /// <summary>
    /// Remove a quantity of an item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <param name="amount">The quantity to remove.</param>
    public ItemSlot RemoveItem(Item item, int amount = 1)
    {
        if (amount <= 0 || !HasItem(item))
            return null;

        var slot = slots.First(slot => slot.item == item);
        return RemoveItem(slot, amount);
    }

    public ItemSlot RemoveItem(ItemSlot slot, int amount = 1)
    {
        if (amount <= 0)
            return slot;

        amount = Math.Min(amount, slot.count);
        slot.count -= amount;

        if (slot.count <= 0)
        {
            slots.Remove(slot);
            onItemRemoved.Invoke(slot);
            return null;
        }
        else
        {
            onItemCountChanged.Invoke(slot, slot.count);
            return slot;
        }
    }
}