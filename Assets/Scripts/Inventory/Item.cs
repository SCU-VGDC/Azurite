using UnityEngine;

[CreateAssetMenu(menuName = "Azurite Objects/Item")]
public class Item : ScriptableObject
{
	public enum Category
	{
		TOOL,
		FLOWER,
		MONSTER
	}

	/// <summary>The item's display name.</summary>
	[Tooltip("The item's display name'.")]
	[SerializeField] private string displayName = "New Item";

	/// <summary>The item's description/lore.</summary>
	[Tooltip("The item's description/lore.")]
	[SerializeField] private string description = "I am an item!";

	/// <summary>The item's icon.</summary>
	[Tooltip("The item's icon.")]
	[SerializeField] private Sprite icon = null;

	/// <summary>The item's preview image for the inspect UI.</summary>
	[Tooltip("The item's description/lore.")]
	[SerializeField] private Sprite preview = null;

	/// <summary>The item's max stack size.</summary>
	[Tooltip("The item's max stack size.")]
	[SerializeField] private int maxStackSize = 99;

	/// <summary>A prefab of the item's inspect UI.</summary>
	[Tooltip("A prefab of the item's inspect UI.")]
	[SerializeField] private InspectMenuBase inspectMenuPrefab = null;

	/// <summary>The item's categories. Used for showing relevant items during interactions.</summary>
	[Tooltip("The item's categories. Used for showing relevant items during interactions.")]
	[SerializeField] private Category[] categories = null;

	/// <summary>
	/// Gets the item's display name.
	/// </summary>
	/// <returns>The item's display name.</returns>
	public string GetDisplayName()
	{
		return this.displayName;
	}

	/// <summary>
	/// Gets the item's description.
	/// </summary>
	/// <returns>The item's description.</returns>
	public string GetDescription()
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
	/// Get the item's inspect UI prefab.
	/// </summary>
	/// <returns>The item's inspect UI prefab.</returns>
	public InspectMenuBase GetInspectMenuPrefab()
	{
		return this.inspectMenuPrefab;
	}

	/// <summary>
	/// Gets the item's categories.
	/// </summary>
	/// <returns>The items categories.</returns>
	public Category[] GetCategories()
	{
		return this.categories;
	}
}