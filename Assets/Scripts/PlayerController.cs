using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    // Commands to control Villagers
    public enum Command {
        RIGHT, 
        LEFT, 
        UP
    }
    public float controlRadius = 10f;


    public int moveSpeed = 1;
    public Rigidbody2D rb; 

    // Jump
    private float TOUCH_GROUND_THRESHOLD = 1f;
    [SerializeField] private bool isGrounded;
    public float jumpForce = 140f;
    private bool jump = false;
    private bool grounded = false;

    // Keys
    public KeyCode RIGHT = KeyCode.RightArrow; 
    public KeyCode LEFT = KeyCode.LeftArrow; 
    public KeyCode UP = KeyCode.UpArrow; 
    public KeyCode CONTROL = KeyCode.LeftControl; 

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate(){
    }


    private void HandleJumpInput(){
        float downY = transform.position.y - TOUCH_GROUND_THRESHOLD;
        Vector3 endGroundCheck = new Vector3(transform.position.x, downY, transform.position.z);
        grounded = Physics2D.Linecast(transform.position, endGroundCheck, 1 << LayerMask.NameToLayer("Ground"));
        if (Input.GetKey(UP) && grounded) {
            jump = true;
        }
    }

    private void HandleMovement(){
        if (Input.GetKey(LEFT)) {
            MoveHorizontal(-1);
        }
        if (Input.GetKey(RIGHT)) {
            MoveHorizontal(1);
        }
        if (jump) {
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
        if (Input.GetKey(CONTROL)) {
            
        }
    }

    private void MoveHorizontal(int direction) {
        Vector2 movement = Vector2.zero;
        movement.x = (transform.right*Time.deltaTime*moveSpeed*direction).x;
        movement += (Vector2)(transform.position);
        rb.MovePosition(movement);
    }

    private void CommandAllVillagers(Command c){
        VillagerController[] villagers = FindObjectsOfType<VillagerController>();
        foreach (VillagerController villager in villagers) {
            if ((transform.position - villager.transform.position).magnitude <= controlRadius) {
                //villager.ExecuteCommand(c);
            }
        }
        
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, controlRadius);
    }

}
