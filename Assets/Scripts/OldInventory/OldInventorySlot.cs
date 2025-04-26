using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OldInventorySlot : MonoBehaviour
{
    // Start is called before the first frame update

    public void DrawSlot(int i)
    {
        OldItemData item = PersistentDataScript.instance.PlayerInventory.GetItem(i);
        GetComponent<Image>().sprite = item != null ? item.GetSprite() : null;
    }
}
