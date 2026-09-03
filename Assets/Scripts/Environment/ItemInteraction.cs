using UnityEngine;

public class ItemInteraction : InteractionTrigger
{
    public override bool CanInteract => RequiredItem == null || GameManager.Instance.Player.Inventory.HasItem(RequiredItem);
    [field: SerializeField] public Item RequiredItem { get; private set; }
}
