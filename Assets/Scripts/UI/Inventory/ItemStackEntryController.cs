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
        inventory = associatedInventory;
        item = inventoryItem;
        icon.sprite = item.Icon;

        Refresh();
        return this;
    }

    public void Refresh()
    {
        count.SetText(inventory.GetCount(item).ToString());
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public Item GetItem()
    {
        return item;
    }
}