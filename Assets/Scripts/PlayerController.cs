﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : AbstractController {

    public float controlRadius = 10f;

	public override int moveSpeed { get { return 1; }}
	public override float jumpForce { get { return 140f; }} 
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
	}

    void FixedUpdate() {
		HandleMovement ();
    }

    private void HandleMovement(){
        if (Input.GetKey(LEFT)) {
			MoveHorizontal(LEFT_DIRECTION);
			CommandAllVillagers (Command.LEFT);
        }
        if (Input.GetKey(RIGHT)) {
			MoveHorizontal(RIGHT_DIRECTION);
			CommandAllVillagers (Command.RIGHT);
        }
		if (Input.GetKey (UP)) {
			HandleJumpInput ();
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
