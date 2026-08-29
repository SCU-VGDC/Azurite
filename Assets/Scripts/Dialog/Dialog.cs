using UnityEngine;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    public string defaultTitle = "";
    public Sprite defaultIcon = null;

    public UnityEvent<DialogStep> onStepChanged = new();
    public UnityEvent onFinished = new();

    private DialogStep _currentStep;
    public DialogStep CurrentStep
    {
        get => _currentStep;
        set
        {
            _currentStep = value;
            onStepChanged.Invoke(value);
        }
    }

    public string Title => CurrentStep != null && CurrentStep.HasTitle ? CurrentStep.Title : defaultTitle;
    public Sprite Icon => CurrentStep != null && CurrentStep.HasIcon ? CurrentStep.Icon : defaultIcon;
    public string Body => CurrentStep != null ? CurrentStep.Body : string.Empty;
    public bool HasOptions => CurrentStep != null && CurrentStep.Options.Length > 0;
    public DialogStep[] Options => CurrentStep != null ? CurrentStep.Options : new DialogStep[0];
    public bool Finished => CurrentStep == null || CurrentStep.NextStep == null || CurrentStep.EndDialog;

    private void Reset()
    {
        CurrentStep = null;
    }

    public void StartDialogSequence()
    {
        CurrentStep = transform.GetChild(0).GetComponent<DialogStep>();
    }

    public bool Advance()
    {
        if (CurrentStep == null)
            return false;

        var nextStep = CurrentStep.NextStep;
        if (nextStep == null)
        {
            onFinished.Invoke();
            return false;
        }

        CurrentStep = nextStep;
        return true;
    }
}