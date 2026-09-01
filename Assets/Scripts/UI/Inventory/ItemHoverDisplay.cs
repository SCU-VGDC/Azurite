using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ItemHoverDisplay : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemName;

    private void Awake()
    {
        UpdatePosition();
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        var rt = GetComponent<RectTransform>();
        if (Screen.height - Input.mousePosition.y < rt.rect.size.y)
            rt.pivot = Vector2.one;
        else
            rt.pivot = Vector2.right;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, Input.mousePosition, Camera.main, out var pos);
        rt.anchoredPosition = pos;
    }

    public void DisplayItem(Item item)
    {
        itemImage.sprite = item.Icon;
        itemDescription.text = item.Description;
        itemName.text = item.DisplayName;
    }
}
