using UnityEngine;
using UnityEngine.Events;

public class DialogEntry : MonoBehaviour
{
	[Tooltip("Set this field to change the title of the dialog moving forward.")]
	[SerializeField] public UnityEvent onSelect = new UnityEvent();
	
    [Tooltip("Set this field to change the title of the dialog moving forward.")]
    [SerializeField] private string titleOverride = "";
    
    [Tooltip("The text to display in this entry.")]
	[SerializeField] private string text = "";

	[Tooltip("Set this field to override what the next dialog options should be.")]
	[SerializeField] private DialogEntry nextOverride = null;
	
	[Tooltip("Set to true to force the entry to be selectable. Useful for a singular dialog option.")]
    [SerializeField] private bool overrideSelectable = false;

	public bool HasNext()
	{
		return (this.nextOverride != null && this.nextOverride.HasNext()) || this.transform.childCount > 0;
	}

	public DialogEntry GetActual()
	{
		return this.nextOverride != null ? this.nextOverride.GetActual() : this;
	}
	
	public void Select()
	{
		this.onSelect.Invoke();

		if(this.nextOverride != null)
		{
			this.nextOverride.Select();
		}
	}
	
	public bool IsForceSelectable()
    {
        return this.overrideSelectable;
    }

	public string GetText()
	{
		return this.text;
	}

	public string GetTitleOverride()
	{
		return this.titleOverride;
	}
	
	public DialogController GetController()
	{
		return this.GetComponentInParent<DialogController>();
	}
}