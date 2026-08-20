using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    [Tooltip("Whether or not to save the dialog when closed prematurely.")]
    [SerializeField] private bool keepState = false;
    [SerializeField] private DialogMenuController menuPrefab = null;

    public string defaultTitle = "";
    public Sprite defaultIcon = null;

    public UnityEvent<DialogStep> onChanged = new();
    public UnityEvent onFinished = new();

    private DialogMenuController menu;

    private DialogStep _currentStep;
    public DialogStep CurrentStep
    {
        get => _currentStep;
        set
        {
            _currentStep = value;
            menu.UpdateFromDialog(this);
            onChanged.Invoke(value);
        }
    }
    public string CurrentTitle => CurrentStep != null && CurrentStep.HasTitleOverride ? CurrentStep.TitleOverride : defaultTitle;
    public Sprite CurrentIcon => CurrentStep != null && CurrentStep.HasIconOverride ? CurrentStep.IconOverride : defaultIcon;

    /*private void CacheEntries()
    {
        Transform currentDialog = CurrentStep == null ? transform : CurrentStep.GetActual().transform;

        currentEntries.Clear();
        selectableEntries.Clear();

        for (int i = 0; i < currentDialog.transform.childCount; ++i)
        {
            if (currentDialog.GetChild(i).TryGetComponent(out DialogStep entry))
            {
                currentEntries.Add(entry);

                if (!entry.IsSelectable())
                {
                    continue;
                }

                selectableEntries.Add(entry);
            }
        }
    }*/

    private void Reset()
    {
        CurrentStep = null;
    }

    public void StartDialogSequence()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");
        menu = Instantiate(menuPrefab, canvas.transform);
        menu.Open();

        if (!keepState)
            menu.onClose.AddListener(Reset);

        CurrentStep = transform.GetChild(0).GetComponent<DialogStep>();
    }
}