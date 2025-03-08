using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	public Inventory inventory = null;
	public InventoryUIEntry itemEntryPrefab = null;
	public Button discardOne = null;
	public Button discardAll = null;
	public ToggleGroup itemList = null;
	public Image previewImage = null;
	public TextMeshProUGUI previewTitle = null;
	public TextMeshProUGUI previewDescription = null;

    public void Start()
    {
        this.inventory.changedEvent.AddListener(this.RegenerateSlots);
		this.discardOne.onClick.AddListener(this.DiscardOne);
		this.discardAll.onClick.AddListener(this.DiscardAll);
    }

	public void RegenerateSlots()
	{
		Item[] items = this.inventory.GetItems();
		Item selectedItem = this.GetSelectedItem();

		foreach(Transform child in itemList.transform)
		{
    		Destroy(child.gameObject);
		}

		for(int i = 0; i < items.Length; ++i)
		{
			InventoryUIEntry entry = Instantiate(this.itemEntryPrefab);
			entry.inventory = this.inventory;
			entry.item = items[i];
			entry.transform.SetParent(itemList.transform);

			Toggle toggle = entry.GetComponent<Toggle>();

			if(toggle != null)
			{
				toggle.group = this.itemList;

				if((selectedItem == null && i == 0) || items[i] == selectedItem)
				{
					toggle.isOn = true;
				}

				toggle.onValueChanged.AddListener(this.UpdatePreview);
			}
		}
	}

	public void UpdatePreview(bool value)
	{
		if(!value)
		{
			return;
		}

		Item selectedItem = this.GetSelectedItem();

		if(selectedItem == null)
		{
			return;
		}

		this.previewImage.sprite = selectedItem.GetPreview();
		this.previewTitle.SetText(selectedItem.getDisplayName());
		this.previewDescription.SetText(selectedItem.getDescription());
	}

	public void DiscardOne()
	{
		Item selectedItem = this.GetSelectedItem();

		if(selectedItem == null || !selectedItem.IsTrashable())
		{
			return;
		}

		this.inventory.RemoveItem(selectedItem, 1);
	}

	public void DiscardAll()
	{
		Item selectedItem = this.GetSelectedItem();

		if(selectedItem == null || !selectedItem.IsTrashable())
		{
			return;
		}

		this.inventory.RemoveItem(selectedItem, selectedItem.GetMaxStackSize());
	}

	private Item GetSelectedItem()
	{
		Toggle selected = this.itemList.GetFirstActiveToggle();
		
		if(selected == null)
		{
			return null;
		}

		InventoryUIEntry selectedEntry = selected.gameObject.GetComponent<InventoryUIEntry>();
			
		if(selectedEntry == null)
		{
			return null;
		}

		return selectedEntry.item;
	}
}