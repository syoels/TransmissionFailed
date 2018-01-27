﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : AbstractController {

    public float controlRadius = 10f;

	public override float moveSpeed { get { return 3.25f; }}
	public override float jumpForce { get { return 140f; }} 
    private bool isControlling = false;

	// Animation
	int anim_control_trigger;

	protected override void Start() {
		base.Start();
		anim_control_trigger = Animator.StringToHash ("control");
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}

    void FixedUpdate() {
		HandleMovement ();
    }

//	protected override void InitAnimationParams() {
//		base.InitAnimationParams();
//		ChooseNewTarget();
//	}

    private void HandleMovement(){
		bool isMovingHorizontally = false;

        isControlling = Input.GetKey(KeyCode.LeftControl);
		if (isControlling) {
			animator.SetTrigger (anim_control_trigger);
		}

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
            if (isControlling && (transform.position - villager.transform.position).magnitude <= controlRadius) {
                villager.IsBeingControlled = true;
                villager.ExecuteCommand(c);
            } else if (villager.IsBeingControlled) {
                villager.IsBeingControlled = false;
            }
        }
            
        
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, controlRadius);
    }

}
