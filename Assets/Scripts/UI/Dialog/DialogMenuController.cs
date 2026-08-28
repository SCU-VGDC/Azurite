using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogMenuController : MenuBase
{
    [SerializeField] protected DialogOptionButton optionButtonPrefab = null;

    [SerializeField] protected TextMeshProUGUI titleText = null;
    [SerializeField] protected TextMeshProUGUI bodyText = null;
    [SerializeField] protected Image iconImage = null;

    [Tooltip("The content panel containing the text and options.")]
    [SerializeField] protected VerticalLayoutGroup contentContainer = null;

    [Tooltip("The icon indicating that the dialogue can continue.")]
    [SerializeField] protected AnimatedArrowIcon nextArrow = null;

    private Dialog currentDialog;
    private readonly List<DialogOptionButton> optionButtons = new();
    private Awaitable bodyDisplayTask;

    public void Init(Dialog dialog)
    {
        if (currentDialog != null)
            return;

        currentDialog = dialog;
        currentDialog.onStepChanged.AddListener(_ => DisplayCurrentStep());
        currentDialog.onFinished.AddListener(() =>
        {
            currentDialog = null;
            Close();
        });
        DisplayCurrentStep();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !currentDialog.HasOptions)
        {
            if (bodyDisplayTask == null || bodyDisplayTask.IsCompleted)
            {
                currentDialog.Advance();
                DisplayCurrentStep();
            }
            else
            {
                bodyDisplayTask?.Cancel();
                bodyText.text = currentDialog.Body;
                nextArrow.Show();
            }
        }
    }

    private void DisplayCurrentStep()
    {
        foreach (var option in optionButtons)
            Destroy(option.gameObject);
        optionButtons.Clear();

        iconImage.sprite = currentDialog.Icon;
        titleText.text = currentDialog.Title;

        bodyDisplayTask?.Cancel();
        bodyDisplayTask = DisplayBodyAsync(currentDialog.Body);
    }

    private async Awaitable DisplayBodyAsync(string text)
    {
        bodyText.text = string.Empty;
        nextArrow.Hide();
        foreach (char c in text)
        {
            bodyText.text += c;
            await Awaitable.WaitForSecondsAsync(0.02f);
        }
        nextArrow.Show();

        foreach (var option in currentDialog.Options)
        {
            var button = Instantiate(optionButtonPrefab, contentContainer.transform).Init(currentDialog, option);
            optionButtons.Add(button);
        }
    }
}