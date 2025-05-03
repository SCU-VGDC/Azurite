using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStackUIController : MonoBehaviour
{
	/// <summary>The inventory the item is associated with.</summary>
	[Tooltip("The inventory the item is associated with.")]
	[SerializeField] private Inventory inventory = null;

	/// <summary>The item to display.</summary>
	[Tooltip("The item to display.")]
	[SerializeField] private Item item = null;

	/// <summary>The Image object to fill with the item icon.</summary>
	[Tooltip("The Image object to fill with the item icon.")]
	[SerializeField] private Image icon = null;

	/// <summary>The Text Mesh Pro object to fill with the stack count.</summary>
	[Tooltip("The Text Mesh Pro object to fill with the stack count.")]
	[SerializeField] private TextMeshProUGUI count = null;

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
		this.icon.sprite = this.item.GetIcon();

		this.Refresh();
    }

	/// <summary>
	/// Get item stack information from the inventory and update the item 
	/// stack's display. In reality this just refreshes the item count.
	/// </summary>
    public void Refresh()
    {
        this.count.SetText(this.inventory.GetCount(this.item).ToString());
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