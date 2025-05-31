using UnityEngine;

public class MenuBase : MonoBehaviour
{
	/// <summary>The animations to play when opening/closing the menu.</summary>
	[Tooltip("The animations to play when opening/closing the menu.")]
	[SerializeField] protected MenuAnimation[] animations;

	/// <summary>The currently open sub menu.</summary>
	protected MenuBase subMenu = null;

	/// <summary>The previous menu in the hierarchy.</summary>
	protected MenuBase parentMenu = null;

	/// <summary>Whether or not pushMenu was called. Used for post animation logic.</summary>
	protected bool pushRequested = false;

	/// <summary>Whether or not popMenu was called. Used for post animation logic.</summary>
	protected bool popRequested = false;

	public virtual void Update()
	{
		// If a push requesst was made and the closing animation has finished playing, open the sub menu.
		if(this.pushRequested && this.HaveAnimationsRewound() && this.HasSubMenu())
		{
			this.pushRequested = false;
			this.subMenu.Show();
			this.gameObject.SetActive(false);
		}

		// If a pop requesst was made and the closing animation has finished playing, destroy this menu and open the parent.
		if(this.popRequested && this.HaveAnimationsRewound())
		{
			this.popRequested = false;

			if(this.HasParentMenu())
			{
				this.parentMenu.subMenu = null;
				this.parentMenu.Show();
			}

			Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// Show the highest menu on the menu stack.
	/// </summary>
	public virtual void Show()
	{
		// Recurse up the stack.
		if(this.HasSubMenu())
		{
			this.subMenu.Show();
			return;
		}

		if(this.IsShown())
		{
			return;
		}

		// Enable and play the animations.
		this.gameObject.SetActive(true);

		for(int i = 0; i < this.animations.Length; ++i)
		{
			this.animations[i].Play();
		}
	}

	/// <summary>
	/// Hide the highest menu on the menu stack. This will not destroy the menu.
	/// </summary>
	public virtual void Hide()
	{
		// Recurse up the stack.
		if(this.HasSubMenu())
		{
			this.subMenu.Hide();
			return;
		}

		if(this.IsHidden())
		{
			return;
		}

		// Rewind animations.
		for(int i = 0; i < this.animations.Length; ++i)
		{
			this.animations[i].Rewind();
		}
	}

	/// <summary>
	/// Push a new menu onto the menu stack a hide the previous menu.
	/// </summary>
	public virtual void Open(MenuBase newMenu)
	{
		// Block pushes until the current one finishes.
		if(this.pushRequested)
		{
			return;
		}

		// Recurse up the stack.
		if(this.HasSubMenu())
		{
			this.subMenu.Open(newMenu);
			return;
		}

		// If this menu is active, play the closing animation.
		if(this.isActiveAndEnabled)
		{
			this.Hide();
			this.pushRequested = true;
		}

		this.subMenu = newMenu;
		this.subMenu.parentMenu = this;
		this.subMenu.gameObject.SetActive(false);
	}

	/// <summary>
	/// Pop a menu from the menu stack and show the previous menu.
	/// </summary>
	public virtual void Back()
	{
		// Block pops until the current one finishes.
		if(this.popRequested)
		{
			return;
		}
		
		// Recurse up the stack.
		if(this.HasSubMenu())
		{
			this.subMenu.Back();
			return;
		}

		// If this menu is active, play the closing animation.
		if(this.isActiveAndEnabled)
		{
			this.Hide();
			this.popRequested = true;
		}
		// If the menu is hidden, simply self destruct.
		else
		{
			Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// Destroy the entire menu stack.
	/// </summary>
	public void Destroy()
	{
		// Destroy recursively.
		if(this.HasSubMenu())
		{
			this.subMenu.Destroy();
		}

		// Break the chain in case this menu was destroyed independantly.
		if(this.HasParentMenu())
		{
			this.parentMenu.subMenu = null;
		}

		Destroy(this.gameObject);
	}

	/// <summary>
	/// Check if the inventory is open. Useful for waiting for animations.
	/// </summary>
	/// <returns>True if the inventory is open, false otherwise.</returns>
	public virtual bool IsShown()
	{
		// Recurse up the stack.
		if(this.HasSubMenu())
		{
			return this.subMenu.IsShown();
		}

		// If enabled, check animations.
		return this.isActiveAndEnabled && this.HaveAnimationsPlayed();
	}

	/// <summary>
	/// Check if the inventory is closed. Useful for waiting for animations.
	/// </summary>
	/// <returns>True if the inventory is closed, false otherwise.</returns>
	public virtual bool IsHidden()
	{
		// Prevent external objects from detecting that the menu is closed during menu transitions.
		if(this.pushRequested || this.popRequested)
		{
			return false;
		}

		// Recurse up the stack.
		if(this.HasSubMenu())
		{
			return this.subMenu.IsHidden();
		}

		// If disabled, return true. Otherwise check animations.
		return !this.isActiveAndEnabled || this.HaveAnimationsRewound();
	}

	/// <summary>
	/// Check if the inventory is opening. Useful for waiting for animations.
	/// </summary>
	/// <returns>True if the inventory is opening, false otherwise.</returns>
	public virtual bool IsShowing()
	{
		// Recurse up the stack.
		if(this.HasSubMenu())
		{
			return this.subMenu.IsShowing();
		}

		// If enabled, check animations.
		return this.isActiveAndEnabled && this.AreAnimationsPlaying();
	}

	/// <summary>
	/// Check if the menu is closing. Useful for waiting for animations.
	/// </summary>
	/// <returns>True if the menu is closing, false otherwise.</returns>
	public virtual bool IsHiding()
	{
		// Recurse up the stack.
		if(this.HasSubMenu())
		{
			return this.subMenu.IsHiding();
		}

		// If enabled, check animations.
		return this.isActiveAndEnabled && this.AreAnimationsRewinding();
	}

	/// <summary>
	/// True if the menu has an open sub menu. Remember to check this if you want to destroy 
	/// the menu because it may have an open sub menu, even if <c>IsClosed()</c> returns true.
	/// </summary>
	/// <returns>True if the menu has an open sub menu, false otherwise.</returns>
	public virtual bool HasParentMenu()
	{
		return this.parentMenu != null;
	}

	/// <summary>
	/// True if the menu has an open sub menu. Remember to check this if you want to destroy 
	/// the menu because it may have an open sub menu, even if <c>IsClosed()</c> returns true.
	/// </summary>
	/// <returns>True if the menu has an open sub menu, false otherwise.</returns>
	public virtual bool HasSubMenu()
	{
		return this.subMenu != null;
	}

	/// <summary>
	/// Check if the menu's opening animations have finished playing.
	/// </summary>
	/// <returns>True if the menu's opening animations have finished, false otherwise.</returns>
	public bool HaveAnimationsPlayed()
	{
		// Check if all animations have finished playing.
		for(int i = 0; i < this.animations.Length; ++i)
		{
			if(!this.animations[i].HasPlayed())
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Check if the menu's closing animations have finished playing.
	/// </summary>
	/// <returns>True if the menu's closing animations have finished, false otherwise.</returns>
	public bool HaveAnimationsRewound()
	{
		// Check if all animations have finished rewinding.
		for(int i = 0; i < this.animations.Length; ++i)
		{
			if(!this.animations[i].HasRewound())
			{
				return false;
			}
		}

		return true;
	}
	
	/// <summary>
	/// Check if the menu's opening animations are currently playing.
	/// </summary>
	/// <returns>True if at least one of the menu's opening animations is playing, false otherwise.</returns>
	public bool AreAnimationsPlaying()
	{
		// Check if at least one animation is playing.
		for(int i = 0; i < this.animations.Length; ++i)
		{
			if(this.animations[i].IsPlaying())
			{
				return true;
			}
		}

		return false;
	}
	
	/// <summary>
	/// Check if the menu's closing animations are currently playing.
	/// </summary>
	/// <returns>True if at least one of the menu's closing animations is playing, false otherwise.</returns>
	public bool AreAnimationsRewinding()
	{
		// Check if at least one animation is rewinding.
		for(int i = 0; i < this.animations.Length; ++i)
		{
			if(this.animations[i].IsRewinding())
			{
				return true;
			}
		}

		return false;
	}
}