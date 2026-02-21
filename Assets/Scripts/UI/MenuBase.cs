using DG.Tweening;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MenuBase : MonoBehaviour
{
	[Tooltip("This event is called when when the menu is opened.")]
	public UnityEvent onOpen = new UnityEvent();

	[Tooltip("This event is called when when the menu is closed.")]
	public UnityEvent onClose = new UnityEvent();

	[Tooltip("This event is called when when the menu is hidden, usually when a child menu has been opened.")]
	public UnityEvent onHide = new UnityEvent();

	protected Sequence animations = null;
	protected MenuBase parentMenu = null;
	protected MenuBase childMenu = null;

	private bool isHiding = false;
	private bool isClosing = false;

    public void Awake()
	{
		this.onOpen.AddListener(() => this.gameObject.SetActive(true));
		this.onHide.AddListener(() => this.gameObject.SetActive(false));
		this.onClose.AddListener(() => Destroy(this.gameObject));
		
		this.animations = DOTween.Sequence().SetAutoKill(false).Pause();

		this.CollectAnimations(this.animations, this.transform);

		if(this.animations.Duration() == 0)
		{
			this.animations.Join(DOTween.To(() => {return 0f;}, (value) => {}, 0f, 0f));
		}
    }

	private void CollectAnimations(Sequence sequence, Transform transform)
	{
		if(transform.TryGetComponent<MenuBase>(out MenuBase menu) && menu != this)
		{
			return;
		}

		if(transform.TryGetComponent<MenuAnimation>(out MenuAnimation animation))
		{
			sequence.Join(animation.GetTween());
		}

		for(int i = 0; i < transform.childCount; ++i)
		{
			this.CollectAnimations(sequence, transform.GetChild(i).transform);
		}
	}

    public virtual void Update()
    {
		if(!this.HaveAnimationsRewound())
		{
			return;
		}

		if(this.isHiding)
		{
			this.isHiding = false;
			this.onHide.Invoke();
		}

		if(this.isClosing)
		{
			this.isClosing = false;
			this.onClose.Invoke();
		}
    }

	public virtual void OnDestroy()
	{
		if(this.animations != null)
		{
			this.animations.Kill();
		}
	}

	public void SetParent(MenuBase parent)
	{
		this.parentMenu = parent;
	}

	public void Open()
	{
		this.onOpen.Invoke();
		this.PlayAnimations();
	}

	public void Close()
	{
		if(this.childMenu != null)
		{
			this.childMenu.onClose.AddListener(this.onClose.Invoke);
			this.childMenu.Close();
			return;
		}

		this.RewindAnimations();
		this.isClosing = true;
	}

	public void Hide()
	{
		this.RewindAnimations();
		this.isHiding = true;
	}

	public bool IsOpen()
	{
		return this.enabled && !this.HaveAnimationsRewound();
	}
	
	public bool IsClosed()
	{
		return !this.enabled || this.HaveAnimationsRewound();
	}

	protected void PlayAnimations()
	{
		this.animations.PlayForward();
	}

	protected void RewindAnimations()
	{
		this.animations.PlayBackwards();
	}

	public bool HaveAnimationsPlayed()
	{
		return !this.animations.IsBackwards() && this.animations.ElapsedPercentage() == 1;
	}
	
	public bool HaveAnimationsRewound()
	{
		return this.animations.IsBackwards() && this.animations.ElapsedPercentage() == 0;
	}
}