using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    private static readonly int DownHash = Animator.StringToHash("Down");
    private static readonly int UpHash = Animator.StringToHash("Up");

    public PlayerInteractionController InteractionController => GetComponentInChildren<PlayerInteractionController>();

    // Values have ranges on them to ensure sane values and to ensure NAN or infinity conditions are never encountered
    [SerializeField][Range(0, 10)] private float playerSpeed = 1.0f;
    [SerializeField] private Rigidbody2D PlayerRigidBody;

    public bool Frozen => freezeReasons.Count > 0;

    private Vector2 playerInput;
    public Inventory Inventory { get; private set; }
    private Animator animator;
    private readonly HashSet<string> freezeReasons = new();

    private void Start()
    {
        Inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        PlayerRigidBody.linearVelocity = !Frozen ? playerInput.normalized * playerSpeed : Vector2.zero;

        animator.SetBool(UpHash, playerInput.y > 0);
        animator.SetBool(DownHash, playerInput.y < 0);
    }

    private void LateUpdate()
    {
        //GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    public void Freeze(string reason)
    {
        freezeReasons.Add(reason);
    }

    public void Unfreeze(string reason)
    {
        freezeReasons.Remove(reason);
    }
}
