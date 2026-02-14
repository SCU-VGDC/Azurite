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
    private DialogMenuController menuInstance = null;

    public void Select(int dialogIndex)
    {
        if (menuInstance == null)
            OpenMenu();

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
            Debug.Log($"{entry} {entry.IsSelectable()}");
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

    public void OpenMenu()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");
        menuInstance = Instantiate(menuPrefab, canvas.transform).Init(this);

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