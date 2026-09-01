using UnityEngine;

public class ItemDialogStep : DialogStep
{
    public Item requiredItem;
    public int takeAmount = 0;

    public Item giveItem;
    public int giveAmount = 1;

    public override bool TransitionAllowed => requiredItem == null || GameManager.Instance.Player.Inventory.HasItem(requiredItem);

    public override void OnEnterStep()
    {
        base.OnEnterStep();
        if (giveItem != null)
            GameManager.Instance.Player.Inventory.AddItem(giveItem, giveAmount);
        if (requiredItem != null)
            GameManager.Instance.Player.Inventory.RemoveItem(requiredItem, takeAmount);
    }
}
