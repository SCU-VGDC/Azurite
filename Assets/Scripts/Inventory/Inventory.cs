using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
	[SerializeField] private Dictionary<Item, int> items = new Dictionary<Item, int>();
	public UnityEvent<Item> itemAddedEvent = new UnityEvent<Item>();
	public UnityEvent<Item> itemRemovedEvent = new UnityEvent<Item>();
	public UnityEvent<Item> itemChangedEvent = new UnityEvent<Item>();

	/// <summary>
	/// Checks whether or not the specified item is in the inventory.
	/// </summary>
	/// <param name="item">The item to check for.</param>
	/// <returns>True if the item exists in the inventory, false otherwise.</returns>
    public bool HasItem(Item item)
	{
		return this.items.ContainsKey(item);
	}

	public struct ItemStack
	{
		Item item;
		int count;
	}

	/// <summary>
	/// Gets the amount of this item stored in the inventory.
	/// </summary>
	/// <param name="item">The item to check.</param>
	/// <returns>The amount of this item in the inventory.</returns>
	public int GetCount(Item item)
	{
		return this.items.GetValueOrDefault(item, 0);
	}

	/// <summary>
	/// Add a quantity of an item to the inventory.
	/// </summary>
	/// <param name="item">The item to add.</param>
	/// <param name="amount">The amount to add.</param>
	/// <returns>The amount of items not added to the inventory.</returns>
	public int AddItem(Item item, int amount)
	{
		if(amount <= 0)
		{
			return 0;
		}

		amount = Math.Min(item.GetMaxStackSize(), amount);

		if(!this.HasItem(item))
		{
			this.items.Add(item, amount);
			this.itemAddedEvent.Invoke(item);
			return 0;
		}

		if(this.items[item] + amount <= item.GetMaxStackSize())
		{
			this.items[item] += amount;
			this.itemChangedEvent.Invoke(item);
			return 0;
		}

		int remainder = amount + this.items[item] - item.GetMaxStackSize();
		this.items[item] = item.GetMaxStackSize();
		this.itemChangedEvent.Invoke(item);
		return remainder;
	}

	/// <summary>
	/// Remove a quantity of an item from the inventory.
	/// </summary>
	/// <param name="item">The item to remove.</param>
	/// <param name="amount">The quantity to remove.</param>
	/// <returns>The amount of items successfully removed from the inventory.</returns>
	public int RemoveItem(Item item, int amount)
	{
		if(amount <= 0 || !this.HasItem(item))
		{
			return 0;
		}

		if(this.items[item] - amount > 0)
		{
			this.items[item] -= amount;
			this.itemChangedEvent.Invoke(item);
			return amount;
		}

		int removed = this.items[item];
		this.items.Remove(item);
		this.itemRemovedEvent.Invoke(item);
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

		foreach(Item i in this.items.Keys)
		{
			itemArray[++index] = i;
		}

		return itemArray;
	}
}