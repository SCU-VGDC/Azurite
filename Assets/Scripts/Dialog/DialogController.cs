using UnityEngine;
using UnityEngine.Events;

public class DialogController : MonoBehaviour
{
	[Tooltip("This event is called whenever the dialog opens/changes.")]
	[SerializeField] public UnityEvent<string, DialogEntry[]> onOpenDialog = new UnityEvent<string, DialogEntry[]>();

	[Tooltip("This event is called whenever the dialog finishes.")]
	[SerializeField] public UnityEvent onEnd = new UnityEvent();

	[Tooltip("Set this field to change the title of the dialog moving forward.")]
	[SerializeField] private string title = "";

	[Tooltip("Whether or not to save the dialog when closed.")]
	[SerializeField] private bool keepState = false;

	[Tooltip("The dialog menu prefab.")]
	[SerializeField] private DialogMenuController menuPrefab = null;

	private DialogEntry current = null;
	private string titleOverride = "";

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.M))
		{
			this.Open();
		}
	}

	public void Select(DialogEntry selected)
	{
		Debug.Log("Flag");
		if(selected == null)
		{
			foreach(DialogEntry entry in this.GetDialogEntries())
			{
				if(entry.HasNext())
				{
					this.current = entry.Get();
					this.current.Select();
					this.onOpenDialog.Invoke(this.GetTitle(), this.GetDialogEntries());
					return;
				}
			}

			this.onEnd.Invoke();
			return;
		}

		this.current = selected.Get();
		this.current.Select();
		this.onOpenDialog.Invoke(this.GetTitle(), this.GetDialogEntries());
	}

	public void Reset()
	{
		this.current = null;
		this.titleOverride = "";
		this.onOpenDialog.Invoke(this.GetTitle(), this.GetDialogEntries());
	}

	public void Open()
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
			this.onOpenDialog.Invoke(this.GetTitle(), this.GetDialogEntries());
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

	private DialogEntry[] GetDialogEntries()
	{
		Transform currentDialog = this.current == null ? this.transform : this.current.transform;
		DialogEntry[] entries = new DialogEntry[currentDialog.childCount];

		for (int i = currentDialog.transform.childCount; --i >= 0;)
		{
			entries[i] = currentDialog.GetChild(i).GetComponent<DialogEntry>();
		}

		return entries;
	}
}