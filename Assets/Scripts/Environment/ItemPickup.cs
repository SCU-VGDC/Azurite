using UnityEngine;

[RequireComponent(typeof(ItemStack))]
[RequireComponent(typeof(InteractionTrigger))]
public class ItemPickup : MonoBehaviour
{
    public bool destroyOnPickup = true;
    public bool usePersistentData = true;
    public string dataKey;

    private void Start()
    {
        if (usePersistentData && PersistentDataManager.Instance.TryGet(dataKey, out bool pickedUp) && pickedUp)
        {
            Destroy(gameObject);
            return;
        }

        ItemStack item = GetComponent<ItemStack>();
		
        GetComponent<InteractionTrigger>().playerInteractEvent.AddListener((player) =>
        {
			player.GetInventory().AddItem(item);

            if (usePersistentData)
            {
                PersistentDataManager.Instance.Set(dataKey, true);
            }

            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        });
    }
}
