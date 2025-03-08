using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIEntry : MonoBehaviour
{
	public Inventory inventory = null;
	public Item item = null;
	public TextMeshProUGUI itemName = null;
	public Image itemIcon = null;

    public void Update()
    {
		if(this.inventory == null || !this.inventory.HasItem(this.item))
		{
			Destroy(this);
		}

        this.itemName.SetText(inventory.GetCount(this.item) + "x " + this.item.getDisplayName());
		this.itemIcon.sprite = this.item.GetIcon();
    }
}