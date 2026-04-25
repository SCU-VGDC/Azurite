using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogEntryMenuController : MenuBase
{
	[Tooltip("The entry's button.")]
    [SerializeField] protected Button button = null;

	[Tooltip("The entry's text box.")]
    [SerializeField] protected TextMeshProUGUI text = null;

	public DialogEntryMenuController Init(Dialog dialog, DialogEntry entry)
	{
		this.button.onClick.AddListener(() => dialog.Select(entry));
		this.text.SetText(entry.GetText());
		return this;
	}

	public void ShowSelectable()
	{
		this.text.fontStyle = FontStyles.Underline;
	}

	public Button GetButton()
	{
		return this.button;
	}

	public TextMeshProUGUI GetText()
	{
		return this.text;
	}
}