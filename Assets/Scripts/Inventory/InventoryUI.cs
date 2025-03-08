using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	[SerializeField] private Inventory inventory = null;
	[SerializeField] private InventoryUIEntry itemEntryPrefab = null;
	[SerializeField] private Button discardOne = null;
	[SerializeField] private Button discardAll = null;
	[SerializeField] private ToggleGroup itemList = null;
	[SerializeField] private Image previewImage = null;
	[SerializeField] private TextMeshProUGUI previewTitle = null;
	[SerializeField] private TextMeshProUGUI previewDescription = null;

    public void Start()
    {
		// Add the inventory event listeners.
        this.inventory.itemAddedEvent.AddListener(this.AddEntry);
		this.inventory.itemRemovedEvent.AddListener(this.RemoveEntry);
		this.inventory.itemChangedEvent.AddListener(this.UpdateEntry);

		// Add the button listeners.
		this.discardOne.onClick.AddListener(this.DiscardOne);
		this.discardAll.onClick.AddListener(this.DiscardAll);
    }

	/// <summary>
	/// Add an item to the inventory UI.
	/// </summary>
	/// <param name="item">The item to add.</param>
	public void AddEntry(Item item)
	{
		// Initialize the UI entry and add it to the item list.
		InventoryUIEntry entry = Instantiate(this.itemEntryPrefab);
		entry.Init(this.inventory, item);
		entry.transform.SetParent(itemList.transform);

		Toggle toggle = entry.GetComponent<Toggle>();

		if(toggle == null)
		{
			return;
		}

		// Link the entry with the item list's toggle group.
		toggle.group = this.itemList;
		toggle.onValueChanged.AddListener(this.UpdatePreview);

		// If no item is selected, select the new entry.
		if(this.GetSelectedItem() == null)
		{
			toggle.isOn = true;
			this.UpdatePreview(true);
		}
	}

	/// <summary>
	/// Remove an item from the inventory UI.
	/// </summary>
	/// <param name="item">The item to remove.</param>
    public void RemoveEntry(Item item)
	{
		InventoryUIEntry entry = this.GetEntry(item);
		
		if(entry != null)
		{
			Destroy(entry.gameObject);
			this.UpdatePreview(true);
		}
	}

	/// <summary>
	/// Update the contents of an item entry in the UI.
	/// </summary>
	/// <param name="item">The item to update.</param>
	public void UpdateEntry(Item item)
	{
		InventoryUIEntry entry = this.GetEntry(item);
		
		if(entry != null)
		{
			entry.Refresh();
		}
	}

	/// <summary>
	/// Update the contents of the item preview and description 
	///  with that of the currently selected item. If no item is 
	///  selected, clear the preview and description.
	/// </summary>
	public void UpdatePreview(bool _)
	{
		Item selectedItem = this.GetSelectedItem();

		// If the inventory is empty, clear the preview.
		if(selectedItem == null)
		{
			this.previewImage.color = Color.clear;
			this.previewTitle.SetText("");
			this.previewDescription.SetText("");
			return;
		}

		// Fill the preview with the selected item.
		this.previewImage.sprite = selectedItem.GetPreview();
		this.previewImage.color = Color.white;
		this.previewTitle.SetText(selectedItem.getDisplayName());
		this.previewDescription.SetText(selectedItem.getDescription());
	}

	/// <summary>
	/// Shrink the currently selected item stack by one.
	/// </summary>
	public void DiscardOne()
	{
		Item selectedItem = this.GetSelectedItem();

		if(selectedItem == null || !selectedItem.IsTrashable())
		{
			return;
		}

		this.inventory.RemoveItem(selectedItem, 1);
	}

	/// <summary>
	/// Remove the currently selected item stack from the inventory.
	/// </summary>
	public void DiscardAll()
	{
		Item selectedItem = this.GetSelectedItem();

		if(selectedItem == null || !selectedItem.IsTrashable())
		{
			return;
		}

		this.inventory.RemoveItem(selectedItem, selectedItem.GetMaxStackSize());
	}

	/// <summary>
	/// Get the currently selected item from the inventory UI.
	/// </summary>
	/// <returns>The currently selected item or null if none is selected.</returns>
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

		return selectedEntry.GetItem();
	}

	/// <summary>
	/// Get the UI entry associated with the specified item.
	/// </summary>
	/// <param name="item">The item to associated with the UI entry.</param>
	/// <returns>The associated UI entry or null if none was found.</returns>
	private InventoryUIEntry GetEntry(Item item)
	{
		foreach(Transform child in this.itemList.transform)
		{
    		InventoryUIEntry entry = child.gameObject.GetComponent<InventoryUIEntry>();

			if(entry != null && item == entry.GetItem())
			{
				return entry;
			}
		}

		return null;
	}
}