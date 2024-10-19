using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempInput : MonoBehaviour
{
    [SerializeField] GameObject inv;
    [SerializeField] ItemData tempItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)){
            inv.GetComponent<InventoryScreen>().ToggleInventory();
        } 
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            PersistentDataScript.instance.PlayerInventory.addItem(0,0);

            Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);
        
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PersistentDataScript.instance.PlayerInventory.addItem(0, 1);

            Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PersistentDataScript.instance.PlayerInventory.addItem(0, 2);

            Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PersistentDataScript.instance.PlayerInventory.addItem(0, 3);

            Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PersistentDataScript.instance.PlayerInventory.addItem(0, 4);

            Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);

        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PersistentDataScript.instance.PlayerInventory.addItem(0, 5);

            Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);

        }

    }
}
