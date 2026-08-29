using System;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    private static readonly int DownHash = Animator.StringToHash("Down");
    private static readonly int UpHash = Animator.StringToHash("Up");

    [SerializeField] Rigidbody2D PlayerRigidBody;
    Vector2 playerInput;
    //Values have ranges on them to ensure sane values and to ensure NAN or infinity conditions are never encountered
    [SerializeField][Range(0, 10)] float playerSpeed = 1.0f;

    /// <summary>The player's inventory.</summary>
    public Inventory Inventory { get; private set; }

    public bool freezeMovement = false;

    private Animator animator;

    public void Start()
    {
        Inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!freezeMovement) PlayerRigidBody.linearVelocity = playerInput.normalized * playerSpeed; // without this line, player cannot move. at all.
        else PlayerRigidBody.linearVelocity = new Vector2(0, 0);
        animator.SetBool(UpHash, playerInput.y > 0);
        animator.SetBool(DownHash, playerInput.y < 0);
    }

    void LateUpdate()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
