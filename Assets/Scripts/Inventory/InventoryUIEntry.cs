using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIEntry : MonoBehaviour
{
	[SerializeField] private Inventory inventory = null;
	[SerializeField] private Item item = null;
	[SerializeField] private TextMeshProUGUI itemName = null;
	[SerializeField] private Image itemIcon = null;

	/// <summary>
	/// Initializes the inventory entry. This can only be called once 
	/// and will immediately return if it has already been run.
	/// </summary>
	/// <param name="associatedInventory">The inventory that this entry is associated with.</param>
	/// <param name="inventoryItem">The item to show from the inventory.</param>
    public void Init(Inventory associatedInventory, Item inventoryItem)
    {
		// Return immediately if this has already been run.
        if(this.inventory != null)
		{
			return;
		}

		this.inventory = associatedInventory;
		this.item = inventoryItem;
		this.itemIcon.sprite = this.item.GetIcon();

		this.Refresh();
    }

	/// <summary>
	/// Refresh the contents of the UI entry.
	/// </summary>
    public void Refresh()
    {
        this.itemName.SetText(inventory.GetCount(this.item) + "x " + this.item.getDisplayName());
    }

    /// <summary>
    /// Gets the inventory that this slot is associated with.
    /// </summary>
    /// <returns>The inventory that this slot is associated with.</returns>
    public Inventory GetInventory()
	{
		return this.inventory;
	}

	/// <summary>
	/// Gets the item stored in this entry.
	/// </summary>
	/// <returns>The item in the entry.</returns>
	public Item GetItem()
	{
		return this.item;
	}
}