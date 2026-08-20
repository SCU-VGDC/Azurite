using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Storage class for each step in a dialogue sequence
public class DialogStep : MonoBehaviour
{
    // The Dialog object this step belongs to
    public Dialog OwnerDialog => GetComponentInParent<Dialog>();

    [Tooltip("Optionally replace the dialog sequence's title")]
    [field: SerializeField] public string TitleOverride { get; private set; } = "";
    public bool HasTitleOverride => !string.IsNullOrEmpty(TitleOverride);

    [Tooltip("Optionally replace the dialog sequence's icon")]
    [field: SerializeField] public Sprite IconOverride { get; private set; } = null;
    public bool HasIconOverride => IconOverride != null;

    [Tooltip("The text to display in this entry.")]
    [field: SerializeField] public string Text { get; private set; } = "";

    [Tooltip("Optionally specify the next dialog step to proceed to. If null, proceeds to the next sibling")]
    [field: SerializeField] public DialogStep NextStepOverride { get; private set; } = null;

    private List<DialogStep> _options = null;
    public List<DialogStep> Options => _options ??= GetComponentsInChildren<DialogStep>().Where(s => s != this).ToList();
}