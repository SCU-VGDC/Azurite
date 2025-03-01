using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
	[SerializeField] private Dictionary<Item, int> items = new Dictionary<Item, int>();
	public UnityEvent changedEvent = new UnityEvent();

    public bool HasItem(Item item)
	{
		return this.items.ContainsKey(item);
	}

	public int GetCount(Item item)
	{
		return this.items.GetValueOrDefault(item, 0);
	}

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
			changedEvent.Invoke();
			return 0;
		}

		if(this.items[item] + amount <= item.GetMaxStackSize())
		{
			this.items[item] += amount;
			changedEvent.Invoke();
			return 0;
		}

		int remainder = amount + this.items[item] - item.GetMaxStackSize();
		this.items[item] = item.GetMaxStackSize();
		changedEvent.Invoke();
		return remainder;
	}

	public int RemoveItem(Item item, int amount)
	{
		if(amount <= 0 || !this.HasItem(item))
		{
			return 0;
		}

		if(this.items[item] - amount > 0)
		{
			this.items[item] -= amount;
			changedEvent.Invoke();
			return amount;
		}

		int removed = this.items[item];
		this.items.Remove(item);
		changedEvent.Invoke();
		return removed;
	}

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