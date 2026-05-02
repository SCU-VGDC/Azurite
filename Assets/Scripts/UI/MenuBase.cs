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

    public UnityEvent<int> onChildOpen = new();

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
        onClose.AddListener(() => Destroy(gameObject));

        transform.parent.TryGetComponent(out parent);
        children = GetComponentsInChildren<MenuBase>().Where(menu => menu != this).ToList();

        foreach (var child in children)
            child.onOpen.AddListener(() => CurrentSequence = AnimateOnChildOpen(child));
    }

    protected virtual void OnDestroy()
    {
        CurrentSequence?.Kill();
    }

    // Create and return an animation for opening this menu
    protected abstract Sequence AnimateOnOpen();
    
    // Create and return an animation for closing this menu
    protected abstract Sequence AnimateOnClose();
    
    // Create and return an animation when a child menu opens
    protected abstract Sequence AnimateOnChildOpen(MenuBase child);


    public virtual void Open()
    {
        CurrentSequence = AnimateOnOpen();
    }

    public virtual void Close()
    {
        CurrentSequence = AnimateOnClose().AppendCallback(onClose.Invoke);
    }
}