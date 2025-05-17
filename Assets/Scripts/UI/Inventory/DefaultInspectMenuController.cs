using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefaultInspectMenuController : InspectMenuBase
{
	/// <summary>The item name text box.</summary>
	[Tooltip("The item name text box.")]
	[SerializeField] private TextMeshProUGUI title = null;

	/// <summary>The item description text box.</summary>
	[Tooltip("The item description text box.")]
	[SerializeField] private TextMeshProUGUI descripiton = null;

	/// <summary>The item preview image.</summary>
	[Tooltip("The item preview image.")]
	[SerializeField] private Image preview = null;

	public override InspectMenuBase Init(Item item)
	{
		// Fill in the menu with item information.
		this.title.SetText(item.GetDisplayName());
		this.descripiton.SetText(item.GetDescription());
		this.preview.sprite = item.GetPreview();
		return this;
	}

	public override void Update()
	{
		base.Update();

		// Go back to the inventory when space is pressed.
		if(Input.GetKeyDown(KeyCode.Space))
		{
			this.Back();
		}
	}
}