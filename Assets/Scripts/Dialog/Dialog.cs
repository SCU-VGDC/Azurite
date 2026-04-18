using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
	[Tooltip("This event is called whenever the dialog opens/changes.")]
	[SerializeField] public UnityEvent<Dialog> onDialogChange = new UnityEvent<Dialog>();

	[Tooltip("This event is called whenever the dialog finishes.")]
	[SerializeField] public UnityEvent onDialogEnd = new UnityEvent();

	[Tooltip("The initial title of the dialog sequence.")]
	[SerializeField] private string title = "";

	[Tooltip("The initial title of the dialog sequence.")]
	[SerializeField] private Sprite icon = null;

	[Tooltip("Whether or not to save the dialog when closed prematurely.")]
	[SerializeField] private bool keepState = false;

	[Tooltip("The dialog menu prefab.")]
	[SerializeField] private DialogMenuController menuPrefab = null;

	private DialogEntry currentEntry = null;
	private string currentTitle = "";
	private Sprite currentIcon = null;

	private List<DialogEntry> currentEntries = new List<DialogEntry>();
	private List<DialogEntry> selectableEntries = new List<DialogEntry>();

	public void Awake()
	{
		this.currentTitle = this.title;
		this.currentIcon = this.icon;
		this.CacheEntries();
	}

	private void CacheEntries()
	{
		Transform currentDialog = this.currentEntry == null ? this.transform : this.currentEntry.GetActual().transform;
		
		this.currentEntries.Clear();
		this.selectableEntries.Clear();

		for(int i = 0; i < currentDialog.transform.childCount; ++i)
		{
			if(currentDialog.GetChild(i).TryGetComponent<DialogEntry>(out DialogEntry entry))
			{
				this.currentEntries.Add(entry);

				if(!entry.IsSelectable())
				{
					continue;
				}

				this.selectableEntries.Add(entry);
			}
		}
	}

	public void Reset()
	{
		this.currentEntry = null;
		this.currentTitle = this.title;
		this.currentIcon = this.icon;
		this.CacheEntries();
		this.onDialogChange.Invoke(this);
	}

	public bool IsMenuOpen()
	{
		GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");
		return canvas != null && canvas.transform.GetComponentInChildren<DialogMenuController>() != null;
	}

	public DialogMenuController GetOpenMenu()
	{
		GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");
		return canvas != null ? canvas.transform.GetComponentInChildren<DialogMenuController>() : null;
	}

	public void OpenMenu()
	{
		GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");

		if(canvas == null || canvas.transform.GetComponentInChildren<MenuBase>() != null)
		{
			Debug.Log("A menu is already open!");
			return;
		}

		DialogMenuController menu = Instantiate(this.menuPrefab, canvas.transform).Init(this);

		if(!this.keepState)
		{
			menu.onClose.AddListener(this.Reset);
		}

		menu.Open();
	}

	public string GetTitle()
	{
		return this.currentTitle;
	}

	public Sprite GetIcon()
	{
		return this.currentIcon;
	}

	public DialogEntry[] GetEntries()
	{
		return this.currentEntries.ToArray();
	}

	public DialogEntry[] GetSelectableEntries()
	{
		return this.selectableEntries.ToArray();
	}

	public bool HasNext()
	{
		return this.selectableEntries.Count > 0;
	}

	public bool HasOptions()
	{
		return this.selectableEntries.Count > 1;
	}

	public DialogEntry GetDefaultNext()
	{
		return this.HasNext() ? this.selectableEntries[0] : null;
	}

	public void Select(DialogEntry entry)
	{
		// If the entry is not part of this dialog, return
		if(entry != null && !this.currentEntries.Contains(entry))
		{
			Debug.LogWarning("Attempted to select a dialog entry that does not exist.");
			return;
		}

		// If their is no next dialog, end.
		if(!this.HasNext())
		{
			this.onDialogEnd.Invoke();
			return;
		}

		// If the entry has no children during a fork, return.
		if(this.HasOptions() && entry != null && !entry.IsSelectable())
		{
			Debug.LogWarning("Attempted to select a dialog entry that has no options.");
			return;
		}

		// If only one branch exists, select it regardless of the entry.
		this.currentEntry = entry != null && this.HasOptions() ? entry : this.GetDefaultNext();

		if(this.currentEntry.HasTitleOverride())
		{
			this.currentTitle = this.currentEntry.GetTitleOverride();
		}

		if(this.currentEntry.HasIconOverride())
		{
			this.currentIcon = this.currentEntry.GetIconOverride();
		}

		this.CacheEntries();
		this.onDialogChange.Invoke(this);
	}
}