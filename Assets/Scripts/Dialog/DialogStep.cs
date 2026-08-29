using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Storage class for each step in a dialogue sequence
public class DialogStep : MonoBehaviour
{
    public Dialog OwnerDialog => GetComponentInParent<Dialog>();

    [field: SerializeField] public bool ChildrenAreOptions { get; private set; } = false;
    [field: SerializeField] public bool EndDialog { get; private set; } = false;

    [field: SerializeField] public string Title { get; private set; } = string.Empty;
    public bool HasTitle => !string.IsNullOrEmpty(Title);

    [field: SerializeField] public Sprite Icon { get; private set; } = null;
    public bool HasIcon => Icon != null;

    [field: SerializeField] [field: TextArea] public string Body { get; private set; } = string.Empty;

    [Tooltip("Optionally specify the next dialog step to proceed to. If null, proceeds to the next sibling")]
    [field: SerializeField] public DialogStep NextStepOverride { get; private set; } = null;

    public DialogStep NextStep
    {
        get
        {
            if (EndDialog)
                return null;

            if (NextStepOverride != null)
                return NextStepOverride;

            int index = transform.GetSiblingIndex();
            if (index + 1 < transform.parent.childCount)
                return transform.parent.GetChild(index + 1).GetComponent<DialogStep>();

            return null;
        }
    }

    public DialogStep[] Options => ChildrenAreOptions ? GetComponentsInChildren<DialogStep>().Where(s => s != this && s.transform.parent == transform).ToArray() : new DialogStep[0];
}