using UnityEngine;

[CreateAssetMenu(menuName = "Azurite Objects/ReadableItem")]
public class ReadableItem : Item
{
    [TextArea] public string noteText;

    public override bool Usable => true;

    public override void Use()
    {
        UIManager.Instance.CreateNotePopup(noteText);
    }
}
