using UnityEngine;
using UnityEngine.Events;

public class MenuBase : MonoBehaviour
{
	[Tooltip("The animations to play when opening/closing the menu.")]
	[SerializeField] protected MenuAnimation[] animations;

	public UnityEvent onOpen = new UnityEvent();
	public UnityEvent onClose = new UnityEvent();
	public UnityEvent onHide = new UnityEvent();

	protected MenuBase parentMenu = null;
	protected MenuBase childMenu = null;
	protected int playingAnimations = 0;

	private bool checkHide = false;
	private bool checkClose = false;

    public void Awake()
	{
		this.onOpen.AddListener(() => this.gameObject.SetActive(true));
		this.onClose.AddListener(() => Destroy(this.gameObject));
		this.onHide.AddListener(() => this.gameObject.SetActive(false));
		this.onClose.AddListener(() => {
			if(this.childMenu != null)
            {
				this.childMenu.Close();
            }
		});
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
		for (int i = animations.Length; --i >= 0;)
		{
			this.animations[i].Play();
		}
	}

	protected void RewindAnimations()
	{
		for(int i = animations.Length; --i >= 0;)
		{
			this.animations[i].Rewind();
		}
	}

	public bool HaveAnimationsPlayed()
	{
		for(int i = animations.Length; --i >= 0;)
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
		for(int i = animations.Length; --i >= 0;)
		{
			if(this.animations[i].GetDirection() != -1f || this.animations[i].GetProgress() > 0f)
			{
				return false;
			}
		}

		return true;
	}
}