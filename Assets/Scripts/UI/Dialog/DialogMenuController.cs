using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogMenuController : MenuBase
{
    [Tooltip("The text box of the title.")]
    [SerializeField] protected TextMeshProUGUI title = null;
    
    [Tooltip("The inventory to display.")]
    [SerializeField] protected VerticalLayoutGroup content = null;
}