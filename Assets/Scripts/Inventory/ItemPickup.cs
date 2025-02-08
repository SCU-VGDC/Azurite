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
        interaction.OnInteract += Pickup;
    }
    void Pickup()
    {
        if (PersistentDataScript.instance.PlayerInventory.TryAddItem(ItemID, out int _) && DestroyOnPickup) //If item is marked for destruction and the item is picked up, destroy the item picked up.
        {
            Destroy(gameObject);
        }
    }
}
