using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemPickup : InteractionTrigger
{
    public bool destroyOnPickup = true;
    public bool usePersistentData = true;
    public Item item;
    public int amount = 1;
    public string dataKey;

    private void Awake()
    {
        if (destroyOnPickup && usePersistentData && PersistentDataManager.Instance.TryGet(dataKey, out bool pickedUp) && pickedUp)
        {
            Destroy(gameObject);
            return;
        }

        GetComponent<SpriteRenderer>().sprite = item.Icon;
    }

    public override void Trigger(Player interactingPlayer)
    {
        base.Trigger(interactingPlayer);

        GameManager.Instance.Player.Inventory.AddItem(item, amount);

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
