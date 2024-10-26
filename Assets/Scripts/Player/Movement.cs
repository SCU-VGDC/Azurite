using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody2D PlayerRigidBody;
    [SerializeField] float playerSpeed = 100;

    float inputX = 0f;
    float inputY = 0f;

    // Update is called once per frame
    void Update()
    {
        inputX  = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 movement = new(playerSpeed * inputX, playerSpeed * inputY);
        movement *= Time.deltaTime;

        //transform.Translate(movement);
        PlayerRigidBody.velocity = movement;
    }
}
