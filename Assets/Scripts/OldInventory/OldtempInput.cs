using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldtempInput : MonoBehaviour
{
    [SerializeField] OldItemData tempItem;

    // Very bad, very temp input manager REMOVE LATER
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            PersistentDataScript.instance.PlayerInventory.AddItem(0,0);

            //Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);
        
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PersistentDataScript.instance.PlayerInventory.AddItem(0, 1);

            //Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PersistentDataScript.instance.PlayerInventory.AddItem(0, 2);

            //Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PersistentDataScript.instance.PlayerInventory.AddItem(0, 3);

            //Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PersistentDataScript.instance.PlayerInventory.AddItem(0, 4);

            //Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);

        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PersistentDataScript.instance.PlayerInventory.AddItem(0, 5);

            //Debug.Log(PersistentDataScript.instance.ITEM_LIST[0].name);

        }

    }
}
