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

	public string getDisplayName()
	{
		return this.displayName;
	}

	public string getDescription()
	{
		return this.description;
	}

	public Sprite GetIcon()
	{
		return this.icon;
	}

	public Sprite GetPreview()
	{
		return this.preview;
	}

	public int GetMaxStackSize()
	{
		return this.maxStackSize;
	}

	public bool IsTrashable()
	{
		return this.trashable;
	}
}