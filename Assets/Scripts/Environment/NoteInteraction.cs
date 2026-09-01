using UnityEngine;

public class NoteInteraction : InteractionTrigger
{
    [TextArea] public string noteText;
    public bool NoteShown { get; private set; } = false;

    public override void Trigger(Player interactingPlayer)
    {
        base.Trigger(interactingPlayer);
        NoteShown = true;
        UIManager.Instance.CreateNotePopup(noteText);
    }
}
