using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class InventoryScreen : MonoBehaviour
{
    [SerializeField] GameObject[] inventorySlots;

    public void UpdateSlots()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].GetComponent<InventorySlot>().DrawSlot(i); //Persistant Data > Inventory
        }
    }
}

