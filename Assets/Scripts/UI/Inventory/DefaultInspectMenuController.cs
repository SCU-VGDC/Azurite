using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefaultInspectMenuController : InspectMenuBase
{
    [Tooltip("The item name text box.")]
    [SerializeField] private TextMeshProUGUI title = null;

    [Tooltip("The item description text box.")]
    [SerializeField] private TextMeshProUGUI descripiton = null;

    [Tooltip("The item preview image.")]
    [SerializeField] private Image preview = null;

    public override InspectMenuBase Init(Item item)
    {
        title.SetText(item.DisplayName);
        descripiton.SetText(item.Description);
        preview.sprite = item.Preview;
        return this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Close();
        }
    }
}