using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogMenuController : MenuBase
{
    [Tooltip("The text box of the title.")]
    [SerializeField] protected TextMeshProUGUI title = null;

    [Tooltip("The content panel containing the text and options.")]
    [SerializeField] protected VerticalLayoutGroup content = null;

	[Tooltip("The button prefab for dialog options.")]
	[SerializeField] protected DialogEntryMenuController optionPrefab = null;

	DialogController dialog = null;

	public DialogMenuController Init(DialogController dialogController)
	{
		this.dialog = dialogController;
		dialog.onDialogEnd.AddListener(this.Close);
		dialog.onDialogChange.AddListener(this.SetDialog);
		this.onOpen.AddListener(() => { this.GenerateEntries(this.dialog); });
		return this;
	}

	public override void Update()
	{
		base.Update();

		if(Input.GetKeyDown(KeyCode.Return))
		{
			this.dialog.Select(null);
		}
	}

	public void SetDialog(DialogController dialog)
	{
		DialogEntryMenuController[] entries = this.content.GetComponentsInChildren<DialogEntryMenuController>();

		if(entries.Length == 0)
		{
			this.GenerateEntries(dialog);
			return;
		}

		entries[0].onClose.AddListener(() => { this.GenerateEntries(dialog); });

		for(int i = entries.Length; --i >= 0;)
		{
			entries[i].Close();
		}
	}

	private void GenerateEntries(DialogController dialog)
	{
		DialogEntry[] entries = dialog.GetEntries();
		this.title.SetText(dialog.GetTitle());
		
		for(int i = 0; i < entries.Length; ++i)
		{
			DialogEntryMenuController entry = Instantiate(this.optionPrefab, this.content.transform).Init(dialog, entries[i]);

			if(dialog.HasOptions() && entries[i].IsSelectable())
			{
				entry.ShowSelectable();
			}

			entry.Open();
		}
	}
}