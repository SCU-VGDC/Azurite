using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHoverDisplay : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemName;

    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void DisplayItem(Item item)
    {
        itemImage.sprite = item.Icon;
        itemDescription.text = item.Description;
        itemName.text = item.DisplayName;
    }
}
