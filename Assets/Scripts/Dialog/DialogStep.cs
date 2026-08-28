using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Storage class for each step in a dialogue sequence
public class DialogStep : MonoBehaviour
{
    public Dialog OwnerDialog => GetComponentInParent<Dialog>();

    [field: SerializeField] public string Title { get; private set; } = "";
    public bool HasTitle => !string.IsNullOrEmpty(Title);

    [field: SerializeField] public Sprite Icon { get; private set; } = null;
    public bool HasIcon => Icon != null;

    [field: SerializeField] public string Body { get; private set; } = "";

    [Tooltip("Optionally specify the next dialog step to proceed to. If null, proceeds to the next sibling")]
    [field: SerializeField] public DialogStep NextStepOverride { get; private set; } = null;

    public DialogStep NextStep =>
        NextStepOverride != null
        ? NextStepOverride
        : transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<DialogStep>();

    public DialogStep[] Options => GetComponentsInChildren<DialogStep>().Where(s => s != this).ToArray();
}