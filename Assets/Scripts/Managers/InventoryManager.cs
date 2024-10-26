using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager 
{
    int MaxInventorySize = 6;
    
    private ItemData[] inventory;   

    public InventoryManager()
    {
        inventory = new ItemData[MaxInventorySize];
        for (int i = 0; i < MaxInventorySize; i++)
        {
            inventory[i] = null;
        }
    }


    // Searches the ITEM_LIST for ItemID, and adds it to slot if slot is empty
    public void addItem(int ItemID, int slot)
    {

        if (inventory[slot] == null)
        {
            inventory[slot] = PersistentDataScript.instance.ITEM_LIST[ItemID];
            //Debug.Log(PersistentDataScript.instance.ITEM_LIST[ItemID].getName());
            return;
        }

    }
    public ItemData getItem(int SlotID) {


        return inventory[SlotID];
    }
    



    public void removeItem(int slot)
    {
        if (inventory[slot] == null)
        {
            return;
        }
        inventory[slot] = null;

        
    }

}
