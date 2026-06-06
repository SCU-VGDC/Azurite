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

	/// <summary>The player's inventory.</summary>
	public Inventory inventory;

	public bool freezeMovement = false;


	public void Start()
	{
		this.inventory = this.GetComponent<Inventory>();

		if(this.inventory == null)
		{
			Debug.LogError("Failed to find the player inventory.");
		}
	}

	void Update()
	{
		if(Input.GetButtonDown("Inventory"))
		{
			if(this.inventory.IsMenuOpen())
			{
				this.inventory.GetOpenMenu().Close();
			}
			else
			{
				this.inventory.OpenMenu();
			}
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			if(this.inventory.IsPopupOpen())
			{
				this.inventory.GetOpenPopup().Close();
			}
			else
			{
				BoxCollider2D box = this.GetComponent<BoxCollider2D>();
				this.inventory.OpenPopup(this.transform, new Vector3(0, box.size.y * 0.5f, 0), Item.Category.FLOWER);
			}
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

	public Inventory GetInventory()
	{
		return this.inventory;
	}
}
