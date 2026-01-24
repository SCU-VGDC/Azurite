using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogEntryMenuController : MenuBase
{
	[Tooltip("The text box of the title.")]
    [SerializeField] protected Button button = null;

	[Tooltip("The text box of the title.")]
    [SerializeField] protected TextMeshProUGUI text = null;

	public DialogEntryMenuController Init(DialogController dialog, int dialogIndex, string dialogText)
	{
		this.button.onClick.AddListener(() => { dialog.Select(dialogIndex); });
		this.text.SetText(dialogText);
		return this;
	}
}