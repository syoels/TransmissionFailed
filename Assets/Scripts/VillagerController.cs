using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : AbstractController {

	int temprature = 0;
	bool isBeingControlled = false;

	public override float moveSpeed { get { return 2f; }}
	public override float jumpForce { get { return 140f; }} 

	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
		isBeingControlled = Input.GetKey (KeyCode.LeftControl);
	}

	void FixedUpdate() {
		if (!isBeingControlled) {
			//MoveHorizontal (RIGHT_DIRECTION);
		}
	}

	public void ExecuteCommand(Command c) {
		if (isBeingControlled) {
			switch (c) {
			case Command.LEFT:
				MoveHorizontal (LEFT_DIRECTION);
				break;
			case Command.RIGHT:
				MoveHorizontal (RIGHT_DIRECTION);
				break;
			case Command.UP:
				HandleJumpInput ();
				break;
			case Command.STOP:
				Stop ();
				break;
			}
		}
	}

    void OnTriggerEnter2D(Collider2D c){
        if (c.tag == "Victory") {
            onReachedVictoryPoint();
        }
    }

    private void onReachedVictoryPoint(){
        gm.VillagerSaved();
        this.enabled = false;
    }


}
