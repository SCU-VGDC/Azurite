using System;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] Rigidbody2D PlayerRigidBody;
	Vector2 playerInput;
	//Values have ranges on them to ensure sane values and to ensure NAN or infinity conditions are never encountered
	[SerializeField] [Range(0, 10)] float playerSpeed = 1.0f;

	/// <summary>The inventory menu controller prefab.</summary>
	[Tooltip("The inventory menu controller prefab.")]
	[SerializeField] private InventoryMenuController inventoryPrefab = null;

	/// <summary>The player's inventory.</summary>
	private Inventory inventory = null;

	/// <summary>The main canvas.</summary>
	private GameObject canvas = null;

	/// <summary>The player's currently opened UI.</summary>
	private MenuBase activeUI = null;

	public void Start()
	{
		// Retrieve the main canvas.
		this.canvas = GameObject.FindGameObjectWithTag("Main Canvas");

		if(this.canvas == null)
		{
			Debug.LogError("Failed to find the main canvas.");
			return;
		}

		this.inventory = this.GetComponent<Inventory>();
		
		// Get the player's inventory.
		if(this.inventory == null)
		{
			Debug.LogError("Failed to find the player inventory.");
			return;
		}
	}

	void Update()
	{
		// Wait until the inventory's animation is done playing before destroying.
		if(this.activeUI != null && this.activeUI.IsHidden())
		{
			this.activeUI.Destroy();
			this.activeUI = null;
		}
		// Toggle the inventory when "inventory" is pressed.
		else if(Input.GetButtonDown("Inventory"))
		{
			if(this.activeUI != null)
			{
				this.activeUI.Hide();
			}
			else
			{
				this.activeUI = Instantiate(this.inventoryPrefab, this.canvas.transform).Init(this.inventory);
				this.activeUI.Show();
			}
		}

		// FOR TESTING
		// Add an item to the inventory when "U" is pressed.
		if(Input.GetKeyDown(KeyCode.U))
		{
			ItemStack stack = this.GetComponent<ItemStack>();
			stack.AddTo(this.inventory);
		}

		// Only allow player movement when the inventory is closed.
		if(this.activeUI == null)
		{
			playerInput.x = Input.GetAxisRaw("Horizontal");
			playerInput.y = Input.GetAxisRaw("Vertical");
		}
		else
		{
			playerInput.x = 0;
			playerInput.y = 0;
		}
	}

	void FixedUpdate()
	{
		PlayerRigidBody.velocity = playerInput.normalized * playerSpeed; // without this line, player cannot move. at all.
	}
}
