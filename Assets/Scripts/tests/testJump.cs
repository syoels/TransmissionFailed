using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class testJump : MonoBehaviour {

    private float TOUCH_GROUND_THRESHOLD = 1f;
    [SerializeField] private bool isGrounded;
    public float jumpForce = 140f;

    private bool jump = false;
    private bool grounded = false;
    private Rigidbody2D rb;




    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update() {
        this.ProcessPlayerInput();
       
    }

    void FixedUpdate() {
        this.HandleMovement();

        
    }

    private void ProcessPlayerInput() {
        float downY = transform.position.y - TOUCH_GROUND_THRESHOLD;
        Vector3 endGroundCheck = new Vector3(transform.position.x, downY, transform.position.z);

        Debug.DrawLine(transform.position, endGroundCheck, Color.red);

        grounded = Physics2D.Linecast(transform.position, endGroundCheck, 1 << LayerMask.NameToLayer("Ground"));
        if (Input.GetButtonDown("Jump") && grounded) {
            jump = true;
        }
    }

    private void HandleMovement() {
        if (jump) {
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }
}