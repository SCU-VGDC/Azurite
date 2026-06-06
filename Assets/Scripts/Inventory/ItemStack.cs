using System;
using UnityEngine;

public class ItemStack : MonoBehaviour
{
	[Tooltip("The item.")]
	[SerializeField] private Item item = null;

	[Tooltip("The quantity of the item.")]
	[SerializeField] private int count = 1;

    public void AddTo(Inventory inventory)
	{
		inventory.AddItem(this.item, this.count);
	}

	public void AddTo(Player player)
	{
		this.AddTo(player.GetInventory());
	}

    public void RemoveFrom(Inventory inventory)
	{
		inventory.RemoveItem(this.item, this.count);
	}

	 public void RemoveFrom(Player player)
	{
		this.RemoveFrom(player.GetInventory());
	}

	public Item GetItem()
	{
		return this.item;
	}

	public int GetCount()
	{
		return this.count;
	}
}