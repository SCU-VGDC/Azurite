using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody2D PlayerRigidBody;
    Vector2 speed = new(0, 0);
    Vector2 input = new(0,0);
    Vector2 movement = new(0,0);
    const float minFriction = 0.009f;
    const float maxFriction = 0.04f;
    //Values have ranges on them to ensure sane values and to ensure NAN or infinity conditions are never encountered
    [SerializeField] [Range(maxFriction, minFriction)] float friction = 0.1f;
    [SerializeField] [Range(1, 50)] float playerMaxSpeed = 50;
    [SerializeField] [Range(1f, 3)] float multiplyer = 1f;
    

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
	//Friction code to have player come to stop when no key pressed
	//Checks that key is pressed using input vect and checks that movement vect is beyond the minFriction threshold
      	if(input.x == 0 && (movement.x > minFriction || movement.x < -minFriction)){
		speed.x += (speed.x*friction)*-1;
	//At certian point zeroing out player movement is not noticable to the player's eye
	} else if (input.x == 0){
		speed.x = 0;
	}
	if(input.y == 0 && (movement.y > minFriction || movement.y < -minFriction)){
		speed.y += (speed.y*friction)*-1;
	} else if (input.y == 0){
		speed.y = 0;
	}


	//applies force to player if they are not moving faster than whatever playerMaxSpeed is set to
	if(movement.x <= playerMaxSpeed && movement.x >= -playerMaxSpeed){
		speed.x += input.x * multiplyer; 
	}
	if(movement.y <= playerMaxSpeed && movement.y >= -playerMaxSpeed){
		speed.y += input.y * multiplyer;
	}

	movement = speed;
        movement *= Time.deltaTime;
        PlayerRigidBody.velocity = movement;
    }
}
