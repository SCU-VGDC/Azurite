using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemDisplay;
    [SerializeField] ItemHoverDisplay hoverDisplayPrefab;

    private Item item;
    public Item Item
    {
        get => item;
        set
        {
            itemDisplay.sprite = value.Icon;
            item = value;
        }
    }

    private ItemHoverDisplay hoverDisplay;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverDisplay == null)
        {
            hoverDisplay = Instantiate(hoverDisplayPrefab, transform);
            hoverDisplay.DisplayItem(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverDisplay != null)
        {
            Destroy(hoverDisplay.gameObject);
            hoverDisplay = null;
        }
    }
}
