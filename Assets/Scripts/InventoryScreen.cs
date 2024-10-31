using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class InventoryScreen : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] GameObject[] inventorySlots;
    int SlotCount;
    private void Start()
    {
        SlotCount= inventorySlots.Length;
    }

    


    public void openInventory()
    {

        this.gameObject.SetActive(true);
        isOpen= true;
        UpdateSlots();

    }
    public void closeInventory()
    {
        this.gameObject.SetActive(false);
        isOpen= false;
    }
    public void ToggleInventory()
    {
        if (isOpen)
        {
            closeInventory();

        } else
        {
            openInventory();
        }
    }
    public void UpdateSlots()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            inventorySlots[i].GetComponent<InventorySlot>().DrawSlot(i); //Persistant Data > Inventory
        }
    }
}

