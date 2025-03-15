using UnityEngine;

[CreateAssetMenu(menuName = "Azurite Objects/Item")]
public class Item : ScriptableObject
{
	[SerializeField] private string displayName = "New Item";
	[SerializeField] private string description = "I am an item!";
	[SerializeField] private Sprite icon = null;
	[SerializeField] private Sprite preview = null;
	[SerializeField] private int maxStackSize = 99;
	[SerializeField] private bool trashable = true;

	/// <summary>
	/// Gets the item's display name.
	/// </summary>
	/// <returns>The item's display name.</returns>
	public string getDisplayName()
	{
		return this.displayName;
	}

	/// <summary>
	/// Gets the item's description.
	/// </summary>
	/// <returns>The item's description.</returns>
	public string getDescription()
	{
		return this.description;
	}

	/// <summary>
	/// Gets the item's icon sprite.
	/// </summary>
	/// <returns>The item's icon sprite.</returns>
	public Sprite GetIcon()
	{
		return this.icon;
	}

	/// <summary>
	/// Gets the item's more detailed preview sprite.
	/// </summary>
	/// <returns>The item's preview sprite.</returns>
	public Sprite GetPreview()
	{
		return this.preview;
	}

	/// <summary>
	/// Gets the item's maximum stack size.
	/// </summary>
	/// <returns>The item's maximum stack size.</returns>
	public int GetMaxStackSize()
	{
		return this.maxStackSize;
	}

	/// <summary>
	/// Gets whether or not the item is trashable. This is typically 
	/// false for key items and true for non-vital items.
	/// </summary>
	/// <returns>True if the item is trashable, false otherwise.</returns>
	public bool IsTrashable()
	{
		return this.trashable;
	}
}