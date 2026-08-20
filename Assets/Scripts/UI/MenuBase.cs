using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class MenuBase : MonoBehaviour
{
    [Tooltip("This event is called when when the menu is opened.")]
    public UnityEvent onOpen = new();

    [Tooltip("This event is called when when the menu is closed.")]
    public UnityEvent onClose = new();

    public UnityEvent<MenuBase> onChildOpen = new();
    public UnityEvent<MenuBase> onChildClose = new();

    public bool destroyOnClose = true;

    public bool IsOpen { get; private set; } = false;

    protected MenuBase parent;
    protected List<MenuBase> children;

    private Sequence _currentSequence;
    public Sequence CurrentSequence
    {
        get => _currentSequence;
        private set
        {
            _currentSequence?.Kill();
            _currentSequence = value;
        }
    }

    private void Start()
    {
        transform.parent.TryGetComponent(out parent);
        children = GetComponentsInChildren<MenuBase>().Where(menu => menu != this).ToList();

        foreach (var child in children)
        {
            child.onOpen.AddListener(() =>
            {
                CurrentSequence = AnimateOnChildOpen(child);
                onChildOpen.Invoke(child);
            });
            child.onClose.AddListener(() =>
                CurrentSequence = AnimateOnChildClose(child).AppendCallback(() => onChildClose.Invoke(child))
            );
        }
    }

    protected virtual void OnDestroy()
    {
        CurrentSequence?.Kill();
    }

    // Create the animation for opening
    protected virtual Sequence AnimateOnOpen() { return DOTween.Sequence(); }
    
    // Create the animation for closing
    protected virtual Sequence AnimateOnClose() { return DOTween.Sequence(); }

    // Create the animation for when any child opens
    protected virtual Sequence AnimateOnChildOpen(MenuBase child) { return DOTween.Sequence(); }

    // Create the animation for when any child closes
    protected virtual Sequence AnimateOnChildClose(MenuBase child) { return DOTween.Sequence(); }

    public virtual void Open()
    {
        IsOpen = true;
        CurrentSequence = AnimateOnOpen();
        onOpen.Invoke();
    }

    public virtual void Close()
    {
        IsOpen = false;
        CurrentSequence = AnimateOnClose().AppendCallback(onClose.Invoke);

        foreach (var child in children)
            child.Close();

        if (destroyOnClose) Destroy(gameObject);
    }
}