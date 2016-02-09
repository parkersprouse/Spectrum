/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed = 5;
    public float jumpForce = 1000f; // The gravity vs. jump force calculation is: gravity scale * 0.006 = jump force
    public LayerMask groundLayer;
    public float groundedRadius = 0.5f;
    public Transform groundCheck;

    private Rigidbody2D body;
    private Character character;
    private bool jump, left, right, grounded;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        character = base.GetComponent<Character>();
        Physics2D.IgnoreLayerCollision(9, 9, true);
    }

    void Update() {
        // Movement Input Checks
        if (Input.GetButtonDown("Jump") && grounded) {
            jump = true;
        }

        if (Input.GetAxisRaw("Horizontal") < 0) {
            left = true;
            right = false;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0) {
            right = true;
            left = false;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0) {
            left = false;
            right = false;
        }
    }

    void FixedUpdate() {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, groundLayer);

        if (left) {
            body.velocity = new Vector2(-speed, body.velocity.y);
        }
        else if (right) {
            body.velocity = new Vector2(speed, body.velocity.y);
        }

        if (jump) {
            body.AddForce(new Vector2(0f, jumpForce));
        }

        jump = false;
        if (!left && !right) {
            body.velocity = new Vector2(0, body.velocity.y);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(new Vector2(groundCheck.position.x, groundCheck.position.y), groundedRadius);
    }

    void OnTriggerEnter2D(Collider2D other) {
        //if (!base.GetComponent<Character>().changedColor) {
            AdjustColor(other.name);
        //}
    }

    // "c" is the name of the trail
    // the switch that follows the intial checks are the colors of the characters that are colliding with the trail
    private void AdjustColor(string c) {
        if (c == "red") {
            switch (character.color) {
                case "blue":
                    character.ChangeColor("purple");
                    break;
                case "yellow":
                    character.ChangeColor("orange");
                    break;
                case "green":
                    character.ChangeColor("brown");
                    break;
                case "black":
                    break;
                case "white":
                    break;
            }
        }

        else if (c == "blue") {
            switch (character.color) {
                case "red":
                    character.ChangeColor("purple");
                    break;
                case "yellow":
                    character.ChangeColor("green");
                    break;
                case "orange":
                    character.ChangeColor("brown");
                    break;
            }
        }

        else if (c == "yellow") {
            switch (character.color) {
                case "red":
                    character.ChangeColor("orange");
                    break;
                case "blue":
                    character.ChangeColor("green");
                    break;
                case "purple":
                    character.ChangeColor("brown");
                    break;
            }
        }

        else if (c == "green") {
            switch (character.color) {
                case "red":
                case "purple":
                case "orange":
                    character.ChangeColor("brown");
                    break;
            }
        }

        else if (c == "orange") {
            switch (character.color) {
                case "blue":
                case "purple":
                case "green":
                    character.ChangeColor("brown");
                    break;
            }
        }

        else if (c == "purple") {
            switch (character.color) {
                case "yellow":
                case "green":
                case "orange":
                    character.ChangeColor("brown");
                    break;
            }
        }
    }

}
*/


using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour {

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public LayerMask collisionLayer;
    public CollisionInfo collisionInfo;
    [HideInInspector]
    public BoxCollider2D boxCollider;
    [HideInInspector]
    public Vector2 playerInput;

    float horizontalRaySpacing, verticalRaySpacing;

    private RaycastOrigins raycastOrigins;
    private Character character;

    const float skinWidth = .015f;

    struct RaycastOrigins {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    public struct CollisionInfo {
        public bool above, below, left, right;

        public void Reset() {
            above = below = left = right = false;
        }
    }

    public virtual void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
        character = GetComponent<Character>();
    }

    public virtual void Start() {
        CalculateRaySpacing();
    }

    public void Move(Vector3 vel, Vector2 input) {
        UpdateRaycastOrigins();
        collisionInfo.Reset();
        playerInput = input;

        if (vel.x != 0)
            HorizontalCollisions(ref vel);

        if (vel.y != 0)
            VerticalCollisions(ref vel);

        transform.Translate(vel);
    }

    void VerticalCollisions(ref Vector3 vel) {
        float directionY = Mathf.Sign(vel.y);
        float rayLength = Mathf.Abs(vel.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + vel.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionLayer);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) {
                vel.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisionInfo.below = directionY == -1;
                collisionInfo.above = directionY == 1;
            }
        }
    }

    void HorizontalCollisions(ref Vector3 vel) {
        float directionX = Mathf.Sign(vel.x);
        float rayLength = Mathf.Abs(vel.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayer);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit) {
                vel.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisionInfo.left = directionX == -1;
                collisionInfo.right = directionX == 1;
            }
        }
    }

    void UpdateRaycastOrigins() {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing() {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    void OnTriggerEnter2D(Collider2D other) {
        //if (!base.GetComponent<Character>().changedColor) {
        AdjustColor(other.name);
        //}
    }

    // "c" is the name of the trail
    // the switch that follows the intial checks are the colors of the characters that are colliding with the trail
    private void AdjustColor(string c) {
        if (c == "red") {
            switch (character.color) {
                case "blue":
                    character.ChangeColor("purple");
                    break;
                case "yellow":
                    character.ChangeColor("orange");
                    break;
                case "green":
                    character.ChangeColor("brown");
                    break;
                case "black":
                    break;
                case "white":
                    break;
            }
        }

        else if (c == "blue") {
            switch (character.color) {
                case "red":
                    character.ChangeColor("purple");
                    break;
                case "yellow":
                    character.ChangeColor("green");
                    break;
                case "orange":
                    character.ChangeColor("brown");
                    break;
            }
        }

        else if (c == "yellow") {
            switch (character.color) {
                case "red":
                    character.ChangeColor("orange");
                    break;
                case "blue":
                    character.ChangeColor("green");
                    break;
                case "purple":
                    character.ChangeColor("brown");
                    break;
            }
        }

        else if (c == "green") {
            switch (character.color) {
                case "red":
                case "purple":
                case "orange":
                    character.ChangeColor("brown");
                    break;
            }
        }

        else if (c == "orange") {
            switch (character.color) {
                case "blue":
                case "purple":
                case "green":
                    character.ChangeColor("brown");
                    break;
            }
        }

        else if (c == "purple") {
            switch (character.color) {
                case "yellow":
                case "green":
                case "orange":
                    character.ChangeColor("brown");
                    break;
            }
        }
    }

}
