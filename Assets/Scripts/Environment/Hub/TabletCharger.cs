using UnityEngine;

public class TabletCharger : ItemInteraction
{
    private const string stateKey = "tablet_charger";

    public override string PopupText => CanInteract ? charging ? "Charge" : "Place Tablet" : MissingItemText;
    protected override Item RequiredItem => charging ? null : unchargedTablet;

    [SerializeField] private Item unchargedTablet;
    [SerializeField] private Item chargedTablet;
    public int chargeActions = 5;

    private bool charging = false;

    private void Start()
    {
        if (!PersistentDataManager.Instance.TryGet<int>(stateKey, out var state))
        {
            PersistentDataManager.Instance.Set(stateKey, 0);
            return;
        }

        switch (state)
        {
            case 1:
                charging = true;
                actionCount = chargeActions;
                break;
            case 2:
                enabled = false;
                break;
        }
    }

    public override void Trigger(Player interactingPlayer)
    {
        base.Trigger(interactingPlayer);

        if (charging)
        {
            PersistentDataManager.Instance.Set(stateKey, 2);
            interactingPlayer.Inventory.AddItem(chargedTablet);

            enabled = false;
        }
        else if (interactingPlayer.Inventory.HasItem(unchargedTablet))
        {
            PersistentDataManager.Instance.Set(stateKey, 1);
            interactingPlayer.Inventory.RemoveItem(unchargedTablet);

            charging = true;
            actionCount = chargeActions;
        }
    }
}
