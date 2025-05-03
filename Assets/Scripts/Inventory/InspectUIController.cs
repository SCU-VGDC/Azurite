using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectUIController : InspectUIBase
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

	/// <summary>The previous UI.</summary>
	private MonoBehaviour prevUI = null;

	public override void Init(Item item, MonoBehaviour previousUI)
	{
		// Fill in the menu with item information.
		this.title.SetText(item.GetDisplayName());
		this.descripiton.SetText(item.GetDescription());
		this.preview.sprite = item.GetPreview();

		// Disable the previous UI.
		this.prevUI = previousUI;
		this.prevUI.gameObject.SetActive(false);
	}

	public void Update()
	{
		// Enable the previous UI and delete self when Space is pressed.
		if(Input.GetKeyDown(KeyCode.Space))
		{
			this.prevUI.gameObject.SetActive(true);
			Destroy(this.gameObject);
		}
	}
}