using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ItemSubmissionMenu : Menu
{
    [SerializeField] private ItemBox itemBoxPrefab;
    [SerializeField] private RectTransform itemBoxContainer;
    public Button submitButton;
    public KeyCode closeKey = KeyCode.E;

    private InventoryMenu invMenu;
    private ItemBox currentItemBox;

    public event Action<Item> OnSubmissionPassed;
    public Predicate<Item> CheckSubmittedItem;

    private void Awake()
    {
        invMenu = GetComponentInParent<InventoryMenu>();
        if (invMenu == null)
            Debug.LogError("An ItemSubmission should be a descendant of an InventoryMenu");
        else
            invMenu.OnItemClicked += OnInventoryItemClicked;

        submitButton.onClick.AddListener(OnSubmit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(closeKey))
            Close();

        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData data = new(EventSystem.current)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> res = new();
            EventSystem.current.RaycastAll(data, res);

            if (!res.Any(r => r.gameObject.transform.IsChildOf(UIManager.Instance.FullscreenMenuContainer.transform)))
                Close();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        invMenu.OnItemClicked -= OnInventoryItemClicked;
        ReturnOwnedItem();
    }

    protected override Tween AnimateOnOpen()
    {
        return GetComponent<CanvasGroup>().DOFade(1, 0.3f);
    }

    protected override Tween AnimateOnClose()
    {
        ReturnOwnedItem();
        return GetComponent<CanvasGroup>().DOFade(0, 0.3f);
    }

    private void OnSubmit()
    {
        if (currentItemBox == null)
            return;

        var item = currentItemBox.Item;
        if (CheckSubmittedItem(item))
        {
            Destroy(currentItemBox.gameObject);
            currentItemBox = null;
            Close();
            OnSubmissionPassed?.Invoke(item);
        }
    }

    private void OnInventoryItemClicked(ItemSlot slot, ItemBox sourceItemBox)
    {
        if (currentItemBox != null)
            return;
        
        currentItemBox = Instantiate(itemBoxPrefab, itemBoxContainer);
        currentItemBox.Item = slot.item;
        currentItemBox.AnimateItemTransfer(sourceItemBox.GetComponent<RectTransform>(), itemBoxContainer, false);
        currentItemBox.OnClick += ReturnOwnedItem;
        GameManager.Instance.Player.Inventory.RemoveItem(slot);
    }

    private void ReturnOwnedItem()
    {
        if (currentItemBox == null)
            return;

        var slot = GameManager.Instance.Player.Inventory.AddItem(currentItemBox.Item);
        var targetItemBox = invMenu.GetUIForSlot(slot);
        var newItemBox = Instantiate(itemBoxPrefab, invMenu.transform);
        targetItemBox.Visible = false;
        newItemBox.Item = currentItemBox.Item;

        currentItemBox.OnClick -= ReturnOwnedItem;
        Destroy(currentItemBox.gameObject);
        currentItemBox = null;

        newItemBox.AnimateItemTransfer(itemBoxContainer.GetComponent<RectTransform>(), targetItemBox.transform, false).onComplete += () =>
        {
            targetItemBox.Visible = true;
            Destroy(newItemBox.gameObject);
        };
    }
}
