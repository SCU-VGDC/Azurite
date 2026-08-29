using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    public bool allowMultipleChildrenOpen = false;
    public bool destroyOnClose = true;
    public UnityEvent onOpen = new();
    public UnityEvent onClose = new();

    public UnityEvent<Menu> onChildOpen = new();
    public UnityEvent<Menu> onChildClose = new();

    public bool IsOpen { get; private set; } = false;

    protected Menu parent;

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

    protected virtual void Start()
    {
        transform.parent.TryGetComponent(out parent);
    }

    protected virtual void OnDestroy()
    {
        CurrentTween?.Kill();
    }

    private void OnTransformParentChanged()
    {
        transform.parent.TryGetComponent(out parent);
    }

    protected virtual Tween AnimateOnOpen() { return null; }

    protected virtual Tween AnimateOnClose() { return null; }

    private void OnChildOpen(Menu child)
    {
        onChildOpen.Invoke(child);

        if (!allowMultipleChildrenOpen)
            foreach (var otherChild in GetComponentsInChildren<Menu>().Where(menu => menu != this && menu != child))
                otherChild.Close();
    }

    private void OnChildClose(Menu child)
    {
        onChildClose.Invoke(child);
    }

    public virtual void Open()
    {
        IsOpen = true;
        CurrentTween = AnimateOnOpen();
        onOpen.Invoke();

        if (parent != null)
            parent.OnChildOpen(this);
    }

    public virtual void Close()
    {
        IsOpen = false;
        CurrentTween = AnimateOnClose();
        onClose.Invoke();

        if (parent != null)
            parent.OnChildClose(this);

        if (destroyOnClose)
            if (CurrentTween != null)
                CurrentTween.OnComplete(() => Destroy(gameObject));
            else
                Destroy(gameObject);
        else
            foreach (var child in GetComponentsInChildren<Menu>().Where(menu => menu != this))
                child.Close();
    }
}