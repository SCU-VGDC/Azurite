using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class InventoryScreen : MonoBehaviour
{
    [SerializeField] InventorySlot[] inventorySlots;

    public void UpdateSlots()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].DrawSlot(i); //Persistant Data > Inventory
        }
    }
}
