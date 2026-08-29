using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DialogMenu : Menu
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
    private CancellationTokenSource bodyDisplayToken;

    public void Init(Dialog dialog)
    {
        if (currentDialog != null)
            return;

        currentDialog = dialog;
        dialog.onFinished.AddListener(Close);
        dialog.onStepChanged.AddListener(OnStepChanged);
        Open();
        dialog.StartDialogSequence();
    }

    protected override Tween AnimateOnOpen()
    {
        return GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
    }

    protected override Tween AnimateOnClose()
    {
        return GetComponent<CanvasGroup>().DOFade(0f, 0.3f);
    }

    private void Update()
    {
        if (currentDialog == null)
            return;

        if (Input.GetMouseButtonDown(0) && !currentDialog.HasOptions)
        {
            if (bodyText.text == currentDialog.Body)
                currentDialog.Advance();
            else
            {
                bodyDisplayToken?.Cancel();
                bodyText.text = currentDialog.Body;
                nextArrow.Show();
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        bodyDisplayToken?.Cancel();

        if (currentDialog != null)
        {
            currentDialog.onFinished.RemoveListener(Close);
            currentDialog.onStepChanged.RemoveListener(OnStepChanged);
        }
    }

    private void OnStepChanged(DialogStep _)
    {
        DisplayCurrentStep();
    }

    private void DisplayCurrentStep()
    {
        foreach (var option in optionButtons)
            Destroy(option.gameObject);
        optionButtons.Clear();

        iconImage.sprite = currentDialog.Icon;
        titleText.text = currentDialog.Title;

        bodyDisplayToken?.Cancel();
        bodyDisplayToken = new CancellationTokenSource();
        DisplayBodyAsync(currentDialog.Body, bodyDisplayToken.Token);
    }

    private void OnTextDisplayFinished()
    {
        if (!currentDialog.HasOptions)
            nextArrow.Show();
        else
            foreach (var option in currentDialog.Options)
            {
                var button = Instantiate(optionButtonPrefab, contentContainer.transform).Init(currentDialog, option);
                optionButtons.Add(button);
            }
    }

    private async void DisplayBodyAsync(string text, CancellationToken cancellationToken)
    {
        bodyText.text = string.Empty;
        nextArrow.Hide();
        foreach (char c in text)
        {
            await Awaitable.WaitForSecondsAsync(0.02f);
            if (cancellationToken.IsCancellationRequested)
                return;
            bodyText.text += c;
        }

        OnTextDisplayFinished();
    }
}