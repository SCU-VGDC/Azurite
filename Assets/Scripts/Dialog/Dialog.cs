using UnityEngine;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    public string defaultTitle = "";
    public Sprite defaultIcon = null;
    public bool allowEarlyExit = true;

    public UnityEvent<DialogStep> onStepChanged = new();
    public UnityEvent onFinished = new();

    private DialogStep _currentStep;
    public DialogStep CurrentStep
    {
        get => _currentStep;
        set
        {
            _currentStep = value;
            value.OnEnterStep();
            onStepChanged.Invoke(value);

            if (value.SkipDisplay)
                Advance();
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
        _currentStep = null;
    }

    public void StartDialogSequence()
    {
        int i = 0;
        while (i < transform.childCount)
        {
            if (transform.GetChild(i).TryGetComponent<DialogStep>(out var step) && step.isActiveAndEnabled && step.TransitionAllowed)
            {
                CurrentStep = step;
                return;
            }
            i++;
        }
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