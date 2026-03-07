using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStackEntryController : MonoBehaviour
{
	[Tooltip("The inventory the item is associated with.")]
	[SerializeField] protected Inventory inventory = null;

	[Tooltip("The item to display.")]
	[SerializeField] protected Item item = null;

	[Tooltip("The Image object to fill with the item icon.")]
	[SerializeField] protected Image icon = null;

	[Tooltip("The Text Mesh Pro object to fill with the stack count.")]
	[SerializeField] protected TextMeshProUGUI count = null;

	public ItemStackEntryController Init(Inventory associatedInventory, Item inventoryItem)
	{
		this.inventory = associatedInventory;
		this.item = inventoryItem;
		this.icon.sprite = this.item.GetIcon();

		this.Refresh();
		return this;
	}

	public void Refresh()
	{
		this.count.SetText(this.inventory.GetCount(this.item).ToString());
	}

   public Inventory GetInventory()
	{
		return this.inventory;
	}

	public Item GetItem()
	{
		return this.item;
	}
}