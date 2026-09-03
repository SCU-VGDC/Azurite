using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Storage class for each step in a dialogue sequence
public class DialogStep : MonoBehaviour
{
    public UnityEvent onEnterStep;

    public Dialog OwnerDialog => GetComponentInParent<Dialog>();

    public virtual bool TransitionAllowed => true;

    [field: SerializeField] public bool ChildrenAreOptions { get; private set; } = false;
    [field: SerializeField] public bool EndDialog { get; private set; } = false;
    [field: SerializeField] public bool SkipDisplay { get; private set; } = false;

    [field: SerializeField] public string Title { get; private set; } = string.Empty;
    public bool HasTitle => !string.IsNullOrEmpty(Title);

    [field: SerializeField] public Sprite Icon { get; private set; } = null;
    public bool HasIcon => Icon != null;

    [field: SerializeField] public int ActionCount { get; private set; } = 0;

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
            while (++index < transform.parent.childCount)
                if (transform.parent.GetChild(index).TryGetComponent<DialogStep>(out var next) && next.isActiveAndEnabled && next.TransitionAllowed)
                    return next;

            return null;
        }
    }

    public DialogStep[] Options => ChildrenAreOptions ? GetComponentsInChildren<DialogStep>().Where(s => s != this && s.TransitionAllowed && s.transform.parent == transform).ToArray() : new DialogStep[0];

    public virtual void OnEnterStep()
    {
        onEnterStep.Invoke();
        ActionManager.Instance.IncrementAction(ActionCount);
    }
}