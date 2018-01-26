using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractController : MonoBehaviour
{
	public enum Command {
		RIGHT, 
		LEFT, 
		UP
	}

	protected const int RIGHT_DIRECTION = 1;
	protected const int LEFT_DIRECTION = -1;

	public abstract int moveSpeed { get; }
	public Rigidbody2D rb; 

	// Jump
	private float TOUCH_GROUND_THRESHOLD = 1f;
	public abstract float jumpForce { get; }
	private bool jump = false;
	private bool grounded = false;

	// Keys
	public KeyCode RIGHT = KeyCode.RightArrow; 
	public KeyCode LEFT = KeyCode.LeftArrow; 
	public KeyCode UP = KeyCode.UpArrow; 
	public KeyCode CONTROL = KeyCode.LeftControl; 
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
	}

	void FixedUpdate()
	{
	}

	protected void HandleJumpInput(){
		float downY = transform.position.y - TOUCH_GROUND_THRESHOLD;
		Vector3 endGroundCheck = new Vector3(transform.position.x, downY, transform.position.z);
		grounded = Physics2D.Linecast(transform.position, endGroundCheck, 1 << LayerMask.NameToLayer("Ground"));
		if (grounded) {
			rb.AddForce(new Vector2(0f, jumpForce));
		}
	}

	protected void AttemptJump() {
		if (jump) {
			rb.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}
	}

	protected void MoveHorizontal(int direction) {
		Vector2 movement = Vector2.zero;
		movement.x = (transform.right*Time.deltaTime*moveSpeed*direction).x;
		movement += (Vector2)(transform.position);
		rb.MovePosition(movement);
	}

}

