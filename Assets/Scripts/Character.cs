using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Character : MonoBehaviour {

    public float maxJumpHeight = 4; // how many units high we want to jump
    public float minJumpHeight = 1; // how many units high we want to jump
    public float timeToJumpApex = .3f; // how long in seconds it takes for us to reach the top of our jump
    public float moveSpeed = 10;

    public string color;
    [HideInInspector] public string originalColor;
    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public bool isFinishedLevel = false;
    [HideInInspector] public bool changedColor = false;
    [HideInInspector] public Vector3 velocity;

    float gravity;
    float maxJumpVelocity, minJumpVelocity;
    float velocityXSmoothing;
    float accelerationTimeAirborn = .2f;
    float accelerationTimeGrounded = .05f;
    
    private PlayerController controller;

	void Start () {
        controller = GetComponent<PlayerController>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        sprite = GetComponent<SpriteRenderer>();
        originalColor = color;
	}
	
	void Update () {
        if (controller.collisionInfo.above || controller.collisionInfo.below) {
            velocity.y = 0;
        }

        if (base.GetComponent<PlayerController>().enabled) {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.below) {
                velocity.y = maxJumpVelocity;
            }
            if (Input.GetKeyUp(KeyCode.Space)) {
                if (velocity.y > minJumpVelocity)
                    velocity.y = minJumpVelocity;
            }

            float targetVelX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelX, ref velocityXSmoothing, (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborn);
            //velocity.x = input.x * moveSpeed;
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime, input);
        }
        else {
            velocity.x = 0;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, new Vector2(0, 0));
        }
	}

    public void ChangeColor(string c) {
        sprite.sprite = Resources.Load<Sprite>(c);
        //GameObject.Find(originalColor + "trail").GetComponent<TrailRendererWith2DCollider>().ChangeTrailMaterial(Resources.Load<Material>("Trails/" + c));
        base.GetComponent<TrailRendererWith2DCollider>().ChangeTrailMaterial(Resources.Load<Material>("Trails/" + c));
        GameObject.Find(color).name = c;
        color = c;
        changedColor = true;
    }

}
