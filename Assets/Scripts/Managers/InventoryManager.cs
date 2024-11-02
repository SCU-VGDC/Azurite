public class InventoryManager 
{
    public const int MaxInventorySize = 6;
    private readonly ItemData[] inventory;   

    public InventoryManager()
    {
        inventory = new ItemData[MaxInventorySize];
        System.Array.Fill(inventory, null);
    }

    // Searches the ITEM_LIST for ItemID, and adds it to slot if slot is empty
    public void AddItem(int ItemID, int slot)
    {
        if (inventory[slot] == null)
        {
            inventory[slot] = PersistentDataScript.instance.ITEM_LIST[ItemID];
            //Debug.Log(PersistentDataScript.instance.ITEM_LIST[ItemID].getName());
            return;
        }
    }

    public ItemData GetItem(int SlotID) {
        return inventory[SlotID];
    }
    
    public void RemoveItem(int slot)
    {
        if (inventory[slot] == null)
        {
            return;
        }
        inventory[slot] = null;
    }

}
