using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody2D PlayerRigidBody;
    Vector2 playerInput;
    //Values have ranges on them to ensure sane values and to ensure NAN or infinity conditions are never encountered
    [SerializeField] [Range(0, 10)] float playerSpeed = 1.0f;
    public bool freezeMovement = false;

    //Update is called once per frame
    void Update()
    {
        playerInput.x  = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!freezeMovement) PlayerRigidBody.velocity = playerInput.normalized * playerSpeed; // without this line, player cannot move. at all.
        else PlayerRigidBody.velocity = new Vector2(0,0);
    }
}
