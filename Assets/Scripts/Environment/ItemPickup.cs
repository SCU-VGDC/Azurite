using UnityEngine;

public class ItemPickup : InteractionTrigger
{
    public bool destroyOnPickup = true;
    public bool usePersistentData = true;
    public Item item;
    public string dataKey;

    private void Awake()
    {
        if (destroyOnPickup && usePersistentData && PersistentDataManager.Instance.TryGet(dataKey, out bool pickedUp) && pickedUp)
        {
            Destroy(gameObject);
            return;
        }
    }

    public override void Trigger(Player interactingPlayer)
    {
        base.Trigger(interactingPlayer);

        if (usePersistentData)
        {
            PersistentDataManager.Instance.Set(dataKey, true);
        }

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }
}
