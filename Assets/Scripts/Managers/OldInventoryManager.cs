using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager 
{
    public const int MaxInventorySize = 6;
    private readonly OldItemData[] inventory;   

    public InventoryManager()
    {
        inventory = new OldItemData[MaxInventorySize];
        System.Array.Fill(inventory, null);
    }

    // Searches the ITEM_LIST for ItemID, and adds it to slot if slot is empty
    public void AddItem(int ItemID, int slot)
    {
        if (inventory[slot] == null)
        {
            inventory[slot] = PersistentDataScript.Instance.ITEM_LIST[ItemID];
            //Debug.Log(PersistentDataScript.Instance.ITEM_LIST[ItemID].getName());
            return;
        }
    }

    public void TryAddItem(int ItemID, out bool success)
    {
        success = false;
        int inventorySlot = 0;
        while(success == false)
        {
            if(inventorySlot == MaxInventorySize)
            {
                success = false;
                return;
            }
            if (inventory[inventorySlot] == null)
            {
                inventory[inventorySlot] = PersistentDataScript.Instance.ITEM_LIST[ItemID];
                //Debug.Log(PersistentDataScript.Instance.ITEM_LIST[ItemID].getName());
                Debug.Log(inventorySlot);
                success = true;
                return;
            }
            else
            {
                inventorySlot++;
                Debug.Log(inventorySlot);
            }
        }
    }

    public OldItemData GetItem(int SlotID) {
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

    /* Finds item given the item's ItemID, assumes items are added into first open slot in inventory. 
     * returns slotID (the index in the inventory array) if found
     * returns -1 if not found
     */
    public int FindItem(int ItemID) {
        int i = 0;
        while (inventory[i] != null) {
            if (inventory[i].GetItemID() == ItemID) return i;
            i++;
        }
        return -1;
    }

    /* Checks that item exists in inventory given the item's ItemID. Returns true or false.*/
    public bool CheckForItem(int ItemID) {
        if (FindItem(ItemID) >= 0) return true;
        return false;
    }

}
