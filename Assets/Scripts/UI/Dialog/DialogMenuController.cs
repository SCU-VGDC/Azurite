using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogMenuController : MenuBase
{
    [Tooltip("The text box of the title.")]
    [SerializeField] protected TextMeshProUGUI title = null;

	[Tooltip("The icon object.")]
    [SerializeField] protected Image icon = null;

    [Tooltip("The content panel containing the text and options.")]
    [SerializeField] protected VerticalLayoutGroup content = null;

	[Tooltip("The next button .")]
    [SerializeField] protected Button nextButton = null;

	[Tooltip("The button prefab for dialog options.")]
	[SerializeField] protected DialogEntryMenuController optionPrefab = null;

	public override void Update()
	{
		base.Update();

		if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && this.nextButton.GetComponent<MenuBase>().IsOpen())
		{
			this.nextButton.onClick.Invoke();
		}
	}

	public DialogMenuController Init(Dialog dialog)
	{
		dialog.onDialogEnd.AddListener(this.Close);
		dialog.onDialogChange.AddListener(this.SetDialog);
		this.onOpen.AddListener(() => this.GenerateEntries(dialog));
		this.nextButton.onClick.AddListener(() => dialog.Select(null));
		return this;
	}

	public void SetDialog(Dialog dialog)
	{
		DialogEntryMenuController[] entries = this.content.GetComponentsInChildren<DialogEntryMenuController>();

		if(entries.Length == 0)
		{
			this.GenerateEntries(dialog);
			return;
		}

		entries[0].onClose.AddListener(() => this.GenerateEntries(dialog));

		for(int i = entries.Length; --i >= 0;)
		{
			entries[i].Close();
		}

		this.nextButton.GetComponent<MenuBase>().Hide();
	}

	private void GenerateEntries(Dialog dialog)
	{
		DialogEntry[] entries = dialog.GetEntries();

		this.title.SetText(dialog.GetTitle());
		this.icon.sprite = dialog.GetIcon();
		
		for(int i = 0; i < entries.Length; ++i)
		{
			DialogEntryMenuController entry = Instantiate(this.optionPrefab, this.content.transform).Init(dialog, entries[i]);

			if(dialog.HasOptions() && entries[i].IsSelectable())
			{
				entry.ShowSelectable();
			}

			entry.Open();
		}

		if(!dialog.HasOptions())
		{
			this.nextButton.GetComponent<MenuBase>().Open();
		}
	}
}