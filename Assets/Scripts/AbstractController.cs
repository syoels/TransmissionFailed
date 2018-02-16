using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
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

    // Animation
    public Animator animator;
    protected int anim_isWalking_bool; 
    protected int anim_ySpeed_float; 
    protected int anim_jump_trigger; 
    protected int anim_isGrounded_bool;

	// Jump
    public float MAX_VELOCITY = 8f;
	private float TOUCH_GROUND_THRESHOLD = 1f;
	public abstract float jumpForce { get; }
	protected bool grounded = false;

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
        animator = GetComponent<Animator>();
        InitAnimationParams();
	}

    protected virtual void InitAnimationParams(){
        anim_isWalking_bool = Animator.StringToHash("isWalking");
        anim_ySpeed_float = Animator.StringToHash("Y_Speed"); 
        anim_jump_trigger = Animator.StringToHash("jump");
        anim_isGrounded_bool = Animator.StringToHash("isGrounded");
    }

    protected virtual void Update(){
        animator.SetFloat(anim_ySpeed_float, rb.velocity.y);
		animator.SetBool(anim_isGrounded_bool, IsGrounded());
    }

	void FixedUpdate()
	{
	}

	protected void HandleJumpInput() {
		grounded = IsGrounded ();
        if (grounded && rb.velocity.magnitude <= MAX_VELOCITY) {
            animator.SetBool(anim_isWalking_bool, false);
			rb.AddForce(new Vector2(0f, jumpForce));
            animator.SetTrigger(anim_jump_trigger);
		}
	}

	protected bool IsGrounded() {
		float downY = transform.position.y - TOUCH_GROUND_THRESHOLD;
		Vector3 endGroundCheck = new Vector3(transform.position.x, downY, transform.position.z);
		grounded = Physics2D.Linecast(transform.position, endGroundCheck, 1 << LayerMask.NameToLayer("Ground"));
        if (!grounded) {
            animator.SetBool(anim_isWalking_bool, false);
        }
		return grounded;
	}
        
	protected void MoveHorizontal(int direction) {
        animator.SetBool(anim_isWalking_bool, true);
		rb.velocity = new Vector2 (moveSpeed * direction, rb.velocity.y);
		sr.flipX = direction == RIGHT_DIRECTION;
	}

	protected void Stop() {
		rb.velocity = new Vector2 (0f, rb.velocity.y);
        animator.SetBool(anim_isWalking_bool, false);
	}

}

