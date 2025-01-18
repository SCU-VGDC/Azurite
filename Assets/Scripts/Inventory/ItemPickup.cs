using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private InteractionTrigger interaction;
    public int ItemID;
    // Start is called before the first frame update
    void Start()
    {
        interaction.OnInteract += Pickup;

    }
    void Pickup()
    {
        bool success;
        PersistentDataScript.instance.PlayerInventory.TryAddItem(ItemID, out success);
        if (success)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
