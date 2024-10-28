using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody2D PlayerRigidBody;
    Vector2 speed = new(0, 0);
    Vector2 input = new(0,0);
    //Values have ranges on them to ensure sane values and to ensure NAN or infinity conditions are never encountered
    [SerializeField] [Range(1, 20)] float deceleration = 0.1f;
    [SerializeField] [Range(1, 50)] float playerMaxSpeed = 50;
    [SerializeField] [Range(1f, 10)] float acceleration = 1f;
    

    //Start is called before the first frame update
    void Start()
    {
    }


    //Update is called once per frame
    void Update()
    {
        input.x  = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        input = input.normalized;

        //Friction code to have player come to stop when no key pressed
        float decelNow = Time.deltaTime * deceleration;
        if (speed.magnitude > decelNow)
            speed += decelNow * new Vector2(input.x == 0 ? -MathF.Sign(speed.x) : 0, input.y == 0 ? -MathF.Sign(speed.y) : 0).normalized;
        else if (input.magnitude == 0)
            //At certian point zeroing out player movement is not noticable to the player's eye
            speed = Vector2.zero;

        //applies force to player if they are not moving faster than whatever playerMaxSpeed is set to
        speed += Time.deltaTime * acceleration * input;
        speed = speed.normalized * MathF.Min(speed.magnitude, playerMaxSpeed);

        PlayerRigidBody.velocity = speed;
    }
}
