using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    public bool restrictPlayerActions = false;
    public bool allowMultipleChildrenOpen = false;
    public bool canBeClosedBySiblings = true;
    public bool destroyOnClose = true;
    public UnityEvent onOpen = new();
    public UnityEvent onClose = new();

    public UnityEvent<Menu> onChildOpen = new();
    public UnityEvent<Menu> onChildClose = new();

    public bool IsOpen { get; private set; } = false;

    protected Menu Parent => transform.parent.GetComponent<Menu>();

    private Tween _currentTween;
    protected Tween CurrentTween
    {
        get => _currentTween;
        private set
        {
            if (value == null) return;

            _currentTween?.Kill();
            _currentTween = value;
        }
    }

    protected virtual void OnDestroy()
    {
        CurrentTween?.Kill();
        UIManager.Instance.OnMenuClosed(this);
    }

    protected virtual Tween AnimateOnOpen() { return null; }

    protected virtual Tween AnimateOnClose() { return null; }

    private void OnChildOpen(Menu child)
    {
        onChildOpen.Invoke(child);

        if (!allowMultipleChildrenOpen)
            foreach (var otherChild in GetComponentsInChildren<Menu>().Where(menu => menu != this && menu != child && menu.transform.parent == transform))
                otherChild.Close();
    }

    private void OnChildClose(Menu child)
    {
        onChildClose.Invoke(child);
    }

    public virtual void Open()
    {
        if (Parent != null && !Parent.CanChildOpen())
            return;

        IsOpen = true;
        CurrentTween = AnimateOnOpen();
        onOpen.Invoke();
        UIManager.Instance.OnMenuOpened(this);

        if (Parent != null)
            Parent.OnChildOpen(this);
    }

    public virtual void Close()
    {
        IsOpen = false;
        CurrentTween = AnimateOnClose();
        onClose.Invoke();
        UIManager.Instance.OnMenuClosed(this);

        if (Parent != null)
            Parent.OnChildClose(this);

        if (destroyOnClose)
            if (CurrentTween != null)
                CurrentTween.OnComplete(() => Destroy(gameObject));
            else
                Destroy(gameObject);
        else
            foreach (var child in GetComponentsInChildren<Menu>().Where(menu => menu != this && menu.transform.parent == transform))
                child.Close();
    }

    protected bool CanChildOpen()
    {
        if (allowMultipleChildrenOpen)
            return true;

        var openMenu = GetComponentsInChildren<Menu>().FirstOrDefault(m => m != this && m.IsOpen && m.transform.parent == transform);
        return openMenu == null || openMenu.canBeClosedBySiblings;
    }
}