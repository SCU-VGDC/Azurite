using UnityEngine;
using UnityEngine.UI;

public class InventoryTestAddItem : MonoBehaviour
{
	public Button button = null;
	public Inventory inventory = null;
	public Item item = null;
	public int amount = 1;

    public void Start()
    {
        this.button.onClick.AddListener(this.AddItem);
    }

	public void AddItem()
	{
		this.inventory.AddItem(this.item, this.amount);
	}
}