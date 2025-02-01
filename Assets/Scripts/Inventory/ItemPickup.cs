using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        bool success;
        PersistentDataScript.instance.PlayerInventory.TryAddItem(ItemID, out success);
        if (success && DestroyOnPickup)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
