using TMPro;
using UnityEngine;
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
	private bool hasOptions = false;

	public DialogMenuController Init(DialogController dialog)
	{
		this.dialogTree = dialog;
		this.dialogTree.onDialogEnd.AddListener(this.Close);
		this.dialogTree.onDialogChange.AddListener(this.SetDialog);
		return this;
	}

	public override void Update()
	{
		base.Update();

		if(!Input.GetKeyDown(KeyCode.Return))
		{
			return;
		}
		
		if(!this.hasOptions)
		{
			this.dialogTree.Select(null);
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

	private void AddEntry(DialogEntry entry, bool useButtons)
	{
		if(useButtons && (entry.HasNext() || entry.IsForceSelectable()))
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

	public void SetDialog(DialogController dialog)
	{
		this.currentEntries = dialog.GetDialogEntries();
		this.title.SetText(dialog.GetTitle());

		for(int i = this.content.transform.childCount; --i >= 0;)
		{
			Destroy(this.content.transform.GetChild(i).gameObject);
		}

		int selectableCount = 0;
		
		for(int i = this.currentEntries.Length; --i >= 0;)
		{
			if(this.currentEntries[i].GetActual().transform.childCount > 0)
			{
				++selectableCount;
			}
			
			if(this.hasOptions = (selectableCount == 2 || this.currentEntries[i].GetActual().IsForceSelectable()))
			{
				break;
			}
		}

		foreach(DialogEntry entry in this.currentEntries)
		{
			this.AddEntry(entry, this.hasOptions);
		}
	}
}