using DG.Tweening;
using UnityEngine;

public class InventoryMenu : Menu
{
    [SerializeField] private ItemBox itemBoxPrefab;
    [SerializeField] private Transform itemBoxContainer;
    public KeyCode toggleKey = KeyCode.Tab;

    protected override Tween AnimateOnOpen()
    {
        var rt = (RectTransform)transform;
        return DOTween.Sequence()
            .Append(rt.DOAnchorPos(new Vector2(-rt.rect.size.x, 0), 0.5f));
    }

    protected override Tween AnimateOnClose()
    {
        var rt = (RectTransform)transform;
        return DOTween.Sequence()
            .Append(rt.DOAnchorPos(Vector2.zero, 0.5f));
    }

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.Player.Inventory.onItemAdded.AddListener(OnItemAdded);
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
}
