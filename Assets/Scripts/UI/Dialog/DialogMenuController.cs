using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogMenuController : MenuBase
{
    [Tooltip("The text box of the title.")]
    [SerializeField] protected TextMeshProUGUI title = null;

    [Tooltip("The content panel containing the text and options.")]
    [SerializeField] protected VerticalLayoutGroup content = null;

    [Tooltip("The text box prefab for dialog text.")]
    [SerializeField] protected TextMeshProUGUI textPrefab = null;

	[Tooltip("The button prefab for dialog options.")]
	[SerializeField] protected Button optionPrefab = null;

	private DialogController dialogTree = null;
	private DialogEntry[] currentEntries = null;

	public DialogMenuController Init(DialogController dialog)
	{
		this.dialogTree = dialog;
		this.dialogTree.onEnd.AddListener(this.Close);
		this.dialogTree.onOpenDialog.AddListener(this.SetDialog);
		return this;
	}

	public override void Update()
	{
		if(!Input.GetKeyDown(KeyCode.Return))
		{
			return;
		}

		for(int i = 0; i < this.content.transform.childCount; ++i)
		{
			Button button = this.content.transform.GetChild(i).GetComponent<Button>();

			if(button != null)
			{
				button.onClick.Invoke();
				return;
			}
		}
	}

	public void AddEntry(DialogEntry entry)
	{
		if(entry.HasNext() || entry.IsForceSelectable())
		{
			Button option = Instantiate(this.optionPrefab, this.content.transform);
			TextMeshProUGUI prompt = option.GetComponentInChildren<TextMeshProUGUI>();

			option.onClick.AddListener(() => { this.dialogTree.Select(entry); });
			prompt.SetText(entry.GetText());
			
			return;
		}
		
		TextMeshProUGUI dialog = Instantiate(this.textPrefab, this.content.transform);
        dialog.SetText(entry.GetText());
	}

	public void SetDialog(string titleText, DialogEntry[] entries)
	{
		this.currentEntries = entries;
		this.title.SetText(titleText);

		for(int i = this.content.transform.childCount; --i >= 0;)
		{
			Destroy(this.content.transform.GetChild(i).gameObject);
		}

		foreach(DialogEntry dialog in this.currentEntries)
		{
			this.AddEntry(dialog);
		}
	}
}