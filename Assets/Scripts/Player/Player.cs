using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
	[SerializeField] Rigidbody2D PlayerRigidBody;
	Vector2 playerInput;
	//Values have ranges on them to ensure sane values and to ensure NAN or infinity conditions are never encountered
	[SerializeField][Range(0, 10)] float playerSpeed = 1.0f;

	/// <summary>The inventory menu controller prefab.</summary>
	[Tooltip("The inventory menu controller prefab.")]
	[SerializeField] private InventoryMenuController inventoryPrefab = null;

	/// <summary>The player's inventory.</summary>
	public Inventory Inventory { get; private set; }

	public bool freezeMovement = false;


	public void Start()
	{
		this.Inventory = this.GetComponent<Inventory>();

		if(this.Inventory == null)
		{
			Debug.LogError("Failed to find the player inventory.");
		}
	}

	void Update()
	{
		if(Input.GetButtonDown("Inventory"))
		{
			if(this.Inventory.IsMenuOpen())
			{
				this.Inventory.GetOpenMenu().Close();
			}
			else
			{
				this.Inventory.OpenMenu();
			}
		}

		// FOR TESTING
		// Add an item to the inventory when "U" is pressed.
		if(Input.GetKeyDown(KeyCode.U))
		{
			ItemStack stack = this.GetComponent<ItemStack>();
			stack.AddTo();
		}

		// Only allow player movement when the inventory is closed.
		GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");

		if(canvas != null && canvas.GetComponentInChildren<MenuBase>() != null)
		{
			playerInput.x = 0;
			playerInput.y = 0;
		}
		else
		{
			playerInput.x = Input.GetAxisRaw("Horizontal");
			playerInput.y = Input.GetAxisRaw("Vertical");
		}
	}

	void FixedUpdate()
	{
		if (!freezeMovement) PlayerRigidBody.linearVelocity = playerInput.normalized * playerSpeed; // without this line, player cannot move. at all.
		else PlayerRigidBody.linearVelocity = new Vector2(0, 0);
	}

    void LateUpdate()
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-transform.position.y*100);
    }
}
