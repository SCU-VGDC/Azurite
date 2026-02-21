using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DialogController : MonoBehaviour
{
	[Tooltip("This event is called whenever the dialog opens/changes.")]
	[SerializeField] public UnityEvent<DialogController> onDialogChange = new UnityEvent<DialogController>();

	[Tooltip("This event is called whenever the dialog finishes.")]
	[SerializeField] public UnityEvent onDialogEnd = new UnityEvent();

	[Tooltip("The initial title of the dialog sequence.")]
	[SerializeField] private string title = "";

	[Tooltip("Whether or not to save the dialog when closed prematurely.")]
	[SerializeField] private bool keepState = false;

	[Tooltip("The dialog menu prefab.")]
	[SerializeField] private DialogMenuController menuPrefab = null;

	private DialogEntry current = null;
	private string titleOverride = "";

	private List<DialogEntry> currentEntries = new List<DialogEntry>();
	private List<DialogEntry> selectableEntries = new List<DialogEntry>();

	public void Awake()
	{
		this.titleOverride = this.title;
		this.CacheEntries();
	}

	private void CacheEntries()
	{
		Transform currentDialog = this.current == null ? this.transform : this.current.GetActual().transform;
		
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
		this.current = null;
		this.titleOverride = this.name;
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
		return this.titleOverride;
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
		this.current = entry != null && this.HasOptions() ? entry : this.GetDefaultNext();

		if(this.current.HasTitleOverride())
		{
			this.titleOverride = this.current.GetTitleOverride();
		}

		this.CacheEntries();
		this.onDialogChange.Invoke(this);
	}
}