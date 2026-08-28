using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class MenuBase : MonoBehaviour
{
    public UnityEvent onOpen = new();
    public UnityEvent onClose = new();

    public UnityEvent<MenuBase> onChildOpen = new();
    public UnityEvent<MenuBase> onChildClose = new();

    public bool destroyOnClose = true;

    public bool IsOpen { get; private set; } = false;

    protected MenuBase parent;

    private Sequence _currentSequence;
    protected Sequence CurrentSequence
    {
        get => _currentSequence;
        private set
        {
            if (value == null) return;

            _currentSequence?.Kill();
            _currentSequence = value;
        }
    }

    protected virtual void Start()
    {
        transform.parent.TryGetComponent(out parent);
    }

    protected virtual void OnDestroy()
    {
        CurrentSequence?.Kill();
    }

    private void OnTransformParentChanged()
    {
        transform.parent.TryGetComponent(out parent);
    }

    // Create the animation for opening
    protected virtual Sequence AnimateOnOpen() { return null; }
    
    // Create the animation for closing
    protected virtual Sequence AnimateOnClose() { return null; }

    // Create the animation for when any child opens
    protected virtual Sequence AnimateOnChildOpen(MenuBase child) { return null; }

    // Create the animation for when any child closes
    protected virtual Sequence AnimateOnChildClose(MenuBase child) { return null; }

    private void OnChildOpen(MenuBase child)
    {
        CurrentSequence = AnimateOnChildOpen(child);
        onChildOpen.Invoke(child);
    }

    private void OnChildClose(MenuBase child)
    {
        CurrentSequence = AnimateOnChildClose(child).AppendCallback(() => onChildClose.Invoke(child));
    }

    public virtual void Open()
    {
        IsOpen = true;
        CurrentSequence = AnimateOnOpen();
        onOpen.Invoke();

        if (parent != null)
            parent.OnChildOpen(this);
    }

    public virtual void Close()
    {
        IsOpen = false;
        CurrentSequence = AnimateOnClose();
        onClose.Invoke();

        if (parent != null)
            parent.OnChildClose(this);

        foreach (var child in GetComponentsInChildren<MenuBase>().Where(menu => menu != this))
            child.Close();

        if (destroyOnClose)
            Destroy(gameObject);
    }
}