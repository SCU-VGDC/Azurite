using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class ItemBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image itemDisplay;
    [SerializeField] private TextMeshProUGUI countDisplay;
    [SerializeField] ItemHoverDisplay hoverDisplayPrefab;

    public event Action OnClick;

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

    private int count;
    public int Count
    {
        get => count;
        set
        {
            countDisplay.text = value.ToString();
            countDisplay.enabled = value > 1;
            count = value;
        }
    }

    private bool visible = true;
    public bool Visible
    {
        get => visible;
        set
        {
            var cg = GetComponent<CanvasGroup>();
            cg.blocksRaycasts = value;
            cg.alpha = value ? 1 : 0;
            visible = value;
        }
    }

    private ItemHoverDisplay hoverDisplay;
    private Tweener transferMotion;
    private float transferMotionAlpha = 0;
    private Vector2 transferStartPos;
    private Transform transferTargetParent;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    public void CenterPivot()
    {
        var rt = GetComponent<RectTransform>();
        rt.anchorMin = Vector2.one / 2;
        rt.anchorMax = Vector2.one / 2;
        rt.pivot = Vector2.one / 2;
    }

    public Tweener AnimateItemTransfer(RectTransform start, Transform target)
    {
        transferMotionAlpha = 0;
        transferStartPos = start.position;
        transferTargetParent = target;

        var rt = GetComponent<RectTransform>();
        rt.anchorMin = start.anchorMin;
        rt.anchorMax = start.anchorMax;
        rt.pivot = start.pivot;
        rt.position = transferStartPos;
        rt.sizeDelta = start.rect.size;

        transferMotion?.Kill();
        transferMotion = DOVirtual.Float(0, 1, 0.55f, value => transferMotionAlpha = value).SetEase(Ease.OutQuart);
        return transferMotion;
    }

    public Tweener AnimateItemTransfer(Transform target)
    {
        return AnimateItemTransfer(GetComponent<RectTransform>(), target);
    }

    private void Update()
    {
        if (transferMotion == null)
            return;

        transform.position = Vector2.Lerp(transferStartPos, transferTargetParent.position, transferMotionAlpha);
        if (!transferMotion.active)
        {
            transferMotion = null;
            transform.SetParent(transferTargetParent, false);
        }
    }

    private void OnDestroy()
    {
        OnClick = null;
        transferMotion?.Kill();
        transferMotion = null;
    }
}
