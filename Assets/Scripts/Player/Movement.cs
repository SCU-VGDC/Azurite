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
    [SerializeField] [Range(0, 1)] float playerMaxSpeed = 0.7f;

    //Update is called once per frame
    void Update()
    {
        playerInput.x  = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // prevents player from moving faster diagonally by forcing them to go at 70% the normal speed instead
        if (playerInput.x != 0 && playerInput.y != 0) {
            playerInput *= playerMaxSpeed;
        }

        PlayerRigidBody.velocity = playerInput * playerSpeed; // without this line, player cannot move. at all.
    }
}
