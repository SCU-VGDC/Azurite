using DG.Tweening;
using System.Linq;
using UnityEngine;

public class InventoryMenu : Menu
{
    [SerializeField] private ItemBox itemBoxPrefab;
    [SerializeField] private Transform itemBoxContainer;
    public KeyCode toggleKey = KeyCode.Tab;

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

    private void OnItemAdded(Item item)
    {
        Instantiate(itemBoxPrefab, itemBoxContainer).Item = item;
    }

    private void OnItemRemoved(Item item)
    {
        foreach (var itemBox in itemBoxContainer.GetComponentsInChildren<ItemBox>().Where(ib => ib.Item == item))
            Destroy(itemBox.gameObject);
    }
}
