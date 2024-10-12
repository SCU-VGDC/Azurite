using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] Rigidbody2D PlayerRigidBody;
    [SerializeField] float playerSpeed;

    // Start is called before the first frame update
    void Start()
    {
    }

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
        Vector3 movement = new Vector3(playerSpeed * inputX, playerSpeed * inputY, 0);

        movement *= Time.deltaTime;

        //transform.Translate(movement);
        PlayerRigidBody.velocity = movement;
    }
}
