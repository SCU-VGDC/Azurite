using UnityEngine;
using UnityEngine.Events;

public class MenuBase : MonoBehaviour
{
	[Tooltip("This event is called when when the menu is opened.")]
	public UnityEvent onOpen = new UnityEvent();

	[Tooltip("This event is called when when the menu is closed.")]
	public UnityEvent onClose = new UnityEvent();

	[Tooltip("This event is called when when the menu is hidden, usually when a child menu has been opened.")]
	public UnityEvent onHide = new UnityEvent();

	protected MenuBase parentMenu = null;
	protected MenuBase childMenu = null;
	protected int playingAnimations = 0;

	private MenuAnimation[] animations = new MenuAnimation[0];
	private bool checkHide = false;
	private bool checkClose = false;

    public void Awake()
	{
		this.onOpen.AddListener(() => this.gameObject.SetActive(true));
		this.onClose.AddListener(() => Destroy(this.gameObject));
		this.onHide.AddListener(() => this.gameObject.SetActive(false));
		this.onClose.AddListener(() =>
		{
			if(this.childMenu != null)
			{
				this.childMenu.Close();
			}
		});

		MenuAnimation[] childAnimations = this.GetComponentsInChildren<MenuAnimation>();
		MenuAnimation myAnimation = this.GetComponent<MenuAnimation>();

		this.animations = new MenuAnimation[(myAnimation != null ? 1 : 0) + childAnimations.Length];

		for(int i = childAnimations.Length; --i >= 0;)
		{
			this.animations[i] = childAnimations[i];
		}

		if(myAnimation != null)
		{
			this.animations[this.animations.Length - 1] = myAnimation;
		}
    }

    public virtual void Update()
    {
        if(this.checkHide && this.HaveAnimationsRewound())
		{
			this.checkHide = false;
			this.onHide.Invoke();
			
			if(this.checkClose)
			{
				this.checkClose = false;
				this.onClose.Invoke();
            }
        }
    }

    public void SetParent(MenuBase parent)
	{
		this.parentMenu = parent;
	}

	public void Open()
	{
		if(this.IsClosed())
		{
			this.onOpen.Invoke();
		}
		
		this.PlayAnimations();
	}

	public void Close()
	{
		this.Hide();
		this.checkClose = true;
	}

	public void Hide()
	{
		this.RewindAnimations();
		this.checkHide = true;
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
		for (int i = this.animations.Length; --i >= 0;)
		{
			this.animations[i].Play();
		}
	}

	protected void RewindAnimations()
	{
		for(int i = this.animations.Length; --i >= 0;)
		{
			this.animations[i].Rewind();
		}
	}

	public bool HaveAnimationsPlayed()
	{
		for(int i = this.animations.Length; --i >= 0;)
		{
			if(this.animations[i].GetDirection() != 1f || this.animations[i].GetProgress() < 1f)
			{
				return false;
			}
		}

		return true;
	}
	
	public bool HaveAnimationsRewound()
	{
		for(int i = this.animations.Length; --i >= 0;)
		{
			if(this.animations[i].GetDirection() != -1f || this.animations[i].GetProgress() > 0f)
			{
				return false;
			}
		}

		return true;
	}
}