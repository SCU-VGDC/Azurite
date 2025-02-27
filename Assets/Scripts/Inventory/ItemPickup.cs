using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
public class ItemPickup : MonoBehaviour
{
    private InteractionTrigger interaction;
    public int ItemID;
    public bool DestroyOnPickup;
    // Start is called before the first frame update
    void Start()
    {
        interaction = GetComponent<InteractionTrigger>();
        if (interaction != null)
        {
            interaction.OnInteract += Pickup;
        }
        else
        {
            Debug.LogWarning("InteractionTrigger component not found on " + gameObject.name);
        }

    }
    void Pickup()
    {
        PersistentDataScript.instance.PlayerInventory.TryAddItem(ItemID, out bool success); //Tries to add item ID to player inventory
        if (success && DestroyOnPickup) //If item is marked for destruction and the item is picked up, destroy the item picked up.
        {
            Destroy(gameObject);
        }
    }
}
