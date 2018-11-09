using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller))]
public class Player : MonoBehaviour {

    public float jumpHeight = 4;
    public float TimeToJumpApex = 0.4f;
    float gravity ;
    float moveSpeed = 6;
    float jumpVelocity;
    float velocitySmoothing;

    float groundedAcceleration = 0.2f;
    float airAcceleration = 0.1f;

    public Vector3 velocity;
    Controller controller;




	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * TimeToJumpApex;
        Debug.Log("gravity :" + gravity);
        Debug.Log("jumpVelocity :" + jumpVelocity);
    }
	
	// Update is called once per frame
	void Update () {
 

        //Avoid accumulating gravity
        if (controller.collisions.above || controller.collisions.above) {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below) {
            velocity.y = jumpVelocity;
        }
        float targetVelocity = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocitySmoothing, (controller.collisions.below) ? groundedAcceleration : airAcceleration);
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
    }

    void ApplyGravity() {
        velocity.y += gravity * Time.deltaTime;
    }
}
