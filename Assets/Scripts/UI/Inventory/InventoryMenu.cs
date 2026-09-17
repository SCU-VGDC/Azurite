using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InventoryMenu : Menu
{
    public KeyCode toggleKey = KeyCode.Tab;

    public event Action<ItemBox> OnItemClicked;

    [SerializeField] private ItemBox itemBoxPrefab;
    [SerializeField] private Transform itemBoxContainer;

    private readonly Dictionary<ItemSlot, ItemBox> uiMap = new();

    public ItemBox GetUIForSlot(ItemSlot slot)
    {
        return uiMap[slot];
    }

    protected override Tween AnimateOnOpen()
    {
        var rt = (RectTransform)transform;
        return rt.DOAnchorPos(new Vector2(-rt.rect.size.x, 0), 0.5f);
    }

    protected override Tween AnimateOnClose()
    {
        var rt = (RectTransform)transform;
        return rt.DOAnchorPos(Vector2.zero, 0.5f);
    }

    protected virtual void Start()
    {
        GameManager.Instance.Player.Inventory.onItemAdded.AddListener(OnItemAdded);
        GameManager.Instance.Player.Inventory.onItemRemoved.AddListener(OnItemRemoved);
        GameManager.Instance.Player.Inventory.onItemCountChanged.AddListener(OnItemCountChanged);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (IsOpen)
                Close();
            else
                Open();
        }
    }

    private void OnItemAdded(ItemSlot slot)
    {
        var box = Instantiate(itemBoxPrefab, itemBoxContainer);
        box.Item = slot.item;
        box.Count = slot.count;
        box.OnClick += () => OnItemClicked?.Invoke(box);
        uiMap[slot] = box;
    }

    private void OnItemRemoved(ItemSlot slot)
    {
        if (uiMap.TryGetValue(slot, out var box))
        {
            uiMap.Remove(slot);
            Destroy(box.gameObject);
        }
    }

    private void OnItemCountChanged(ItemSlot slot, int count)
    {
        if (uiMap.TryGetValue(slot, out var box))
            box.Count = count;
    }
}
