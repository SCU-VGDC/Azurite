using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefaultInspectMenuController : InspectMenuBase
{
	[Tooltip("The item name text box.")]
	[SerializeField] private TextMeshProUGUI title = null;

	[Tooltip("The item description text box.")]
	[SerializeField] private TextMeshProUGUI descripiton = null;

	[Tooltip("The item preview image.")]
	[SerializeField] private Image preview = null;

	public override InspectMenuBase Init(Item item)
	{
		this.title.SetText(item.GetDisplayName());
		this.descripiton.SetText(item.GetDescription());
		this.preview.sprite = item.GetPreview();
		return this;
	}

	public override void Update()
	{
		base.Update();

		if(Input.GetKeyDown(KeyCode.Space))
		{
			this.Close();
		}
	}
}