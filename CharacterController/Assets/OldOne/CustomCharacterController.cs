using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour {
    public Vector3 move;
    public float moveSpeed; //scalar value (distance/time)
    private Vector3 velocity; //vector value (speed * direction)
    public float gravity;
    public LayerMask discludePlayer;
    public float rayDist;
    private bool grounded = false;
    private RaycastHit groundHit;
    private bool hasJumped = false;
    public float jumpForce = 6.9f;
    public float jumpHeight;
    private void Update() {
       
        ApplyGravity();
        Move();
        isGrounded();
        GroundPosition();
        Jump();

    }

    private void Move() {
        move = new Vector3(Input.GetAxis("Horizontal"), 0, 0); // axis value of movement
        velocity = new Vector3(velocity.x, velocity.y, velocity.z) + move * moveSpeed;
        Debug.Log("Velocity : " + velocity);
        transform.position += velocity * Time.deltaTime;

        velocity = Vector3.zero;
    }

    private void ApplyGravity() {
        if (!grounded)
            velocity.y -= gravity;
        Debug.Log("Velocity : " + velocity);
    }

    private bool isGrounded() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        Debug.DrawRay(ray.origin, ray.direction * rayDist, Color.blue);
        if(Physics.Raycast(ray, out hit, rayDist, discludePlayer)) {
            groundHit = hit;
            grounded = true;
        } else {
            grounded = false;
        }

        return grounded;    
    }

    bool newGroundedState = true;
    private void GroundPosition() {

        if (isGrounded() && newGroundedState) {
            newGroundedState = false;
            transform.position = groundHit.point;
            transform.position += transform.position;
        }

        if (hasJumped) {
            newGroundedState = true;
            hasJumped = false;
        }
    }

    private void Jump() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            transform.position += Vector3.up;
            velocity.y += jumpForce + jumpHeight;
            hasJumped = true;
        }

    }

}
