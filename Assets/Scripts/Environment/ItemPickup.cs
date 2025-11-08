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

        var item = GetComponent<ItemStack>();
        GetComponent<InteractionTrigger>().onInteract.AddListener(() =>
        {
            item.AddTo();

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
