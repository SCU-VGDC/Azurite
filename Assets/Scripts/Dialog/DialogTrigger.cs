using UnityEngine;

[RequireComponent(typeof(Dialog))]
public class DialogTrigger : InteractionTrigger
{
    public override void Trigger(Player interactingPlayer)
    {
        base.Trigger(interactingPlayer);
        UIManager.Instance.CreateDialog(GetComponent<Dialog>());
    }
}