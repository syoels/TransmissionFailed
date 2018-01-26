using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractController : MonoBehaviour
{
	public enum Command {
		RIGHT, 
		LEFT, 
		UP,
		STOP
	} 

	protected const int RIGHT_DIRECTION = 1;
	protected const int LEFT_DIRECTION = -1;

	public abstract float moveSpeed { get; }
	public Rigidbody2D rb;
	public SpriteRenderer sr;

	// Jump
    public float MAX_VELOCITY = 7f;
	private float TOUCH_GROUND_THRESHOLD = 1f;
	public abstract float jumpForce { get; }
	private bool jump = false;
	private bool grounded = false;

	// Keys
	public KeyCode RIGHT = KeyCode.RightArrow; 
	public KeyCode LEFT = KeyCode.LeftArrow; 
	public KeyCode UP = KeyCode.UpArrow; 
	public KeyCode CONTROL = KeyCode.LeftControl; 


    protected GameManager gm; 

	// Use this for initialization
	protected virtual void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
        gm = FindObjectOfType<GameManager>();
        sr = GetComponentInChildren<SpriteRenderer> ();
	}

	void FixedUpdate()
	{
	}

	protected void HandleJumpInput(){
		float downY = transform.position.y - TOUCH_GROUND_THRESHOLD;
		Vector3 endGroundCheck = new Vector3(transform.position.x, downY, transform.position.z);
		grounded = IsGrounded ();
        if (grounded && rb.velocity.magnitude <= MAX_VELOCITY) {
			rb.AddForce(new Vector2(0f, jumpForce));
		}
	}

	protected bool IsGrounded() {
		float downY = transform.position.y - TOUCH_GROUND_THRESHOLD;
		Vector3 endGroundCheck = new Vector3(transform.position.x, downY, transform.position.z);
		grounded = Physics2D.Linecast(transform.position, endGroundCheck, 1 << LayerMask.NameToLayer("Ground"));
		return grounded;
	}
        
	protected void MoveHorizontal(int direction) {
		rb.velocity = new Vector2 (moveSpeed * direction, rb.velocity.y);
		sr.flipX = direction == RIGHT_DIRECTION;
	}

	protected void Stop() {
		rb.velocity = new Vector2 (0f, rb.velocity.y);
	}

}

