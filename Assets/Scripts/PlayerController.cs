using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : AbstractController {

    public float controlRadius = 10f;

	public override float moveSpeed { get { return 2f; }}
	public override float jumpForce { get { return 140f; }} 
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
	}

    void FixedUpdate() {
		HandleMovement ();
    }

    private void HandleMovement(){
		bool isMovingHorizontally = false;

        if (Input.GetKey(LEFT)) {
			MoveHorizontal(LEFT_DIRECTION);
			CommandAllVillagers (Command.LEFT);
			isMovingHorizontally = true;
        }
        if (Input.GetKey(RIGHT)) {
			MoveHorizontal(RIGHT_DIRECTION);
			CommandAllVillagers (Command.RIGHT);
			isMovingHorizontally = true;
        }

		if (!isMovingHorizontally) {
			Stop ();
			CommandAllVillagers (Command.STOP);
		}
		if (Input.GetKey (UP)) {
			HandleJumpInput ();
			CommandAllVillagers (Command.UP);
		}
    }

    private void CommandAllVillagers(Command c){
        VillagerController[] villagers = FindObjectsOfType<VillagerController>();
        foreach (VillagerController villager in villagers) {
            if ((transform.position - villager.transform.position).magnitude <= controlRadius) {
                villager.ExecuteCommand(c);
            }
        }
        
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, controlRadius);
    }

}
