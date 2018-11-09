using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Controller : MonoBehaviour {
    const float skinWidth = 0.015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    public LayerMask collisionMask;
    public CollisionInfo collisions;

    BoxCollider collider;
    RaycastOrigins raycastOrigins;
	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider>();
        CalculateRaySpacing();
    }

    void HorizontalCollisions(ref Vector3 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit hit; 

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (Physics.Raycast(rayOrigin, Vector2.right * directionX, out hit, rayLength, collisionMask)) {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                if(directionX == -1) {
                    collisions.left = true;
                } else {
                    collisions.right = true;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity) {
        //Direction Y velocity
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;


        for (int i = 0; i < verticalRayCount; i++) {
            //raycast origin function of direction on Y axis
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit hit;

           

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, rayLength, collisionMask)) {
                velocity.y = (hit.distance - skinWidth ) * directionY;

                //Update raylength to the hit distance to avoid passing through object when a coner ray detect an obstacle and the other the ground
                rayLength = hit.distance;

                if(directionY == -1) {
                    collisions.below = true;
                } else {
                    collisions.above = true;
                }
            }
        }
    }
    void CalculateRaySpacing() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
    void UpdateRaycastOrigins() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }


    public void Move(Vector3 velocity) {

        UpdateRaycastOrigins();
        collisions.Reset();

        //colision detection
        if(velocity.x != 0)
            HorizontalCollisions(ref velocity);
        if(velocity.y != 0)
            VerticalCollisions(ref velocity);


        //final move
        transform.Translate(velocity);
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;
        
        public void Reset() {
            above = below = false;
            left = right = false;
        }

    }
}
