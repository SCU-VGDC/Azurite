using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // Start is called before the first frame update

    public void DrawSlot(int i)
    {
        ItemData item = PersistentDataScript.instance.PlayerInventory.getItem(i);
        if (item != null)
        {
            this.gameObject.GetComponent<Image>().sprite = item.getSprite();
        }
        //Debug.Log("Drawing");

    }
}
