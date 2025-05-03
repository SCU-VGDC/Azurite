using System;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] Rigidbody2D PlayerRigidBody;
	Vector2 playerInput;
	//Values have ranges on them to ensure sane values and to ensure NAN or infinity conditions are never encountered
	[SerializeField] [Range(0, 10)] float playerSpeed = 1.0f;

	/// <summary>The inventory UI controller prefab.</summary>
	[Tooltip("The inventory UI controller prefab.")]
	[SerializeField] private InventoryUIController inventoryPrefab = null;

	/// <summary>The player's inventory UI.</summary>
	private InventoryUIController inventory = null;

	public void Start()
	{
		// Retrieve the main canvas.
		GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");

		if(canvas == null)
		{
			Debug.LogError("Failed to find the main canvas.");
			return;
		}
		
		// Get the player's inventory.
		if(!this.TryGetComponent(out Inventory inv))
		{
			Debug.LogError("Failed to find the player inventory.");
			return;
		}

		// Create the inventory UI.
		this.inventory = Instantiate(this.inventoryPrefab, canvas.transform);
		this.inventory.Init(inv);
		this.inventory.Close();
	}

	void Update()
	{
		// Toggle the inventory when "inventory" is pressed.
		if(Input.GetButtonDown("Inventory"))
		{
			if(this.inventory.IsOpen())
			{
				this.inventory.Close();
			}
			else
			{
				this.inventory.Open();
			}
		}

		// FOR TESTING
		// Add an item to the inventory when "U" is pressed.
		if(Input.GetKeyDown(KeyCode.U))
		{
			ItemStack stack = this.GetComponent<ItemStack>();
			stack.AddTo(this.inventory.GetInventory());
		}

		// Only allow player movement when the inventory is closed.
		if(!this.inventory.IsOpen())
		{
			playerInput.x = Input.GetAxisRaw("Horizontal");
			playerInput.y = Input.GetAxisRaw("Vertical");
		}
	}

	void FixedUpdate()
	{
		PlayerRigidBody.velocity = playerInput.normalized * playerSpeed; // without this line, player cannot move. at all.
	}
}
