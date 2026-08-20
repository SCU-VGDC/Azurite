using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogMenuController : MenuBase
{
    [SerializeField] protected TextMeshProUGUI titleText = null;
    [SerializeField] protected TextMeshProUGUI bodyText = null;
    [SerializeField] protected DialogOptionButton optionButtonPrefab = null;
    [SerializeField] protected Image iconImage = null;

    [Tooltip("The content panel containing the text and options.")]
    [SerializeField] protected VerticalLayoutGroup contentContainer = null;

    [Tooltip("The icon indicating that the dialogue can continue.")]
    [SerializeField] protected AnimatedArrowIcon nextArrow = null;

    private readonly List<DialogOptionButton> optionButtons = new();

    public void UpdateFromDialog(Dialog dialog)
    {
        titleText.text = dialog.CurrentTitle;
        iconImage.sprite = dialog.CurrentIcon;
        bodyText.text = dialog.CurrentStep.Text;

        optionButtons.ForEach(Destroy);
        optionButtons.Clear();
        
        foreach (var option in dialog.CurrentStep.Options)
        {
            var button = Instantiate(optionButtonPrefab, contentContainer.transform).Init(dialog, option);
            optionButtons.Add(button);
        }
    }
}