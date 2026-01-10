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

	private int firstSelectable = -1;
	private bool hasOptions = false;

	public DialogMenuController Init(DialogController dialog)
	{
		dialog.onDialogEnd.AddListener(this.Close);
		dialog.onDialogChange.AddListener(this.SetDialog);
		return this;
	}

	public override void Update()
	{
		base.Update();

		if(Input.GetKeyDown(KeyCode.Return))
		{
			this.content.transform.GetChild(this.firstSelectable).GetComponent<Button>().onClick.Invoke();
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
		DialogEntry[] entries = dialog.GetDialogEntries();
		this.title.SetText(dialog.GetTitle());

		this.firstSelectable = -1;
		this.hasOptions = false;

		int selectableCount = 0;
		
		for(int i = entries.Length; --i >= 0 && !this.hasOptions;)
		{
			if(entries[i].IsSelectable())
			{
				this.hasOptions = ++selectableCount == 2;

				if(this.firstSelectable < 0)
				{
					this.firstSelectable = i;
				}
			}
		}

		if(this.firstSelectable < 0)
		{
			this.firstSelectable = 0;
		}

		for(int i = 0; i < entries.Length; ++i)
		{
			Instantiate(this.optionPrefab, this.content.transform).Init(dialog, this.hasOptions ? i : -1, entries[i].GetText()).Open();
		}
	}
}