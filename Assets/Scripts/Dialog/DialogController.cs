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

	// TESTING CODE PLEASE REMOVE
	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.M))
		{
			if (this.IsMenuOpen())
			{
				this.GetOpenMenu().Close();
			}
			else
			{
				this.OpenMenu();
			}
		}
	}
	// END OF TESTING CODE

	public void Select(int dialogIndex)
	{
		if(dialogIndex >= 0)
		{
			DialogEntry entry = this.GetDialogEntries()[dialogIndex];

			if(entry.IsSelectable())
			{
				this.current = entry.GetActual();
				this.current.Select();
				this.onDialogChange.Invoke(this);
			}

			return;
		}

		foreach(DialogEntry entry in this.GetDialogEntries())
		{
			if(entry.IsSelectable())
			{
				this.current = entry.GetActual();
				this.current.Select();
				this.onDialogChange.Invoke(this);
				return;
			}
		}

		this.onDialogEnd.Invoke();
	}

	public void Reset()
	{
		this.current = null;
		this.titleOverride = "";
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

		Instantiate(this.menuPrefab, canvas.transform).Init(this);

		if(this.keepState)
		{
			this.onDialogChange.Invoke(this);
			return;
		}

		this.Reset();
	}

	public string GetTitle()
	{
		if(this.current != null && this.current.GetTitleOverride().Length != 0)
		{
			this.titleOverride = this.current.GetTitleOverride();
		}

		return this.titleOverride.Length > 0 ? this.titleOverride : this.title;
	}

	public DialogEntry[] GetDialogEntries()
	{
		Transform currentDialog = this.current == null ? this.transform : this.current.transform;
		DialogEntry[] entries = new DialogEntry[currentDialog.childCount];

		for(int i = currentDialog.transform.childCount; --i >= 0;)
		{
			entries[i] = currentDialog.GetChild(i).GetComponent<DialogEntry>();
		}

		return entries;
	}
}