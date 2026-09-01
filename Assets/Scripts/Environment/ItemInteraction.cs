using UnityEngine;

public class ItemInteraction : InteractionTrigger
{
    public override bool CanInteract => RequiredItem == null || GameManager.Instance.Player.Inventory.HasItem(RequiredItem);
    public override string PopupText => CanInteract ? defaultPopupText : MissingItemText;
    protected virtual Item RequiredItem { get; }
    public string MissingItemText = "Missing Required Item";
}
