using UnityEngine;

public abstract class ItemSubmissionInteraction : InteractionTrigger
{
    public abstract bool CheckSubmittedItem(Item item);
    public virtual void OnSubmissionPassed(Item item) { }

    public override void Trigger(Player interactingPlayer)
    {
        base.Trigger(interactingPlayer);

        var menu = UIManager.Instance.CreateItemSubmission();
        menu.CheckSubmittedItem = CheckSubmittedItem;
        menu.OnSubmissionPassed += OnSubmissionPassed;
    }
}
