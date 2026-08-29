using UnityEngine;

[CreateAssetMenu(menuName = "Azurite Objects/Item")]
public class Item : ScriptableObject
{
    public enum Category
    {
        TOOL,
        FLOWER,
        MONSTER,
        DOORKEY
    }

    [field: SerializeField] public string DisplayName { get; private set; } = "New Item";

    [field: SerializeField] public string Description { get; private set; } = "I am an item!";

    [field: SerializeField] public Sprite Icon { get; private set; } = null;

    [field: SerializeField] public Sprite Preview { get; private set; } = null;

    [field: SerializeField] public int MaxStackSize { get; private set; } = 99;

    [field: SerializeField] public Category[] Categories { get; private set; } = null;
}