﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : AbstractController {
    
    // Temperature
    public float temprature = 0f;
    public float TEMPERATURE_LIMIT = 10f; 
    private float incrementTemperature = 0.05f;
    [Range(0f, 100f)]
    public float chanceToChangeFlamer = 0.02f;

    // Mind control
    bool isBeingControlled = false;
    private bool prevIsBeingControlled = false; 
    public bool isHeadBanging = false;

    // Animation
    int anim_isZombie_bool;
    int anim_die_trigger;

    [SerializeField]
    private Flamer target;
    public int directionModifier = LEFT_DIRECTION;

    public override float moveSpeed { get { return 3.25f; } }

    public override float jumpForce { get { return 140f; } }

    public bool IsBeingControlled {
        set { 
            prevIsBeingControlled = isBeingControlled;
            isBeingControlled = value;
            if (prevIsBeingControlled && !isBeingControlled) {
                BackToSelfControl();
            } 
            isHeadBanging = false;
        } 
        get { 
            return isBeingControlled;
        }
    }

    protected override void Start() {
        base.Start();
        ChooseNewTarget();
    }

    // Update is called once per frame
    void Update() {
        if (isHeadBanging) { //decided in set isBeingControlled
            float rnd = Random.Range(0f, 100f); 
            if (rnd <= chanceToChangeFlamer) {
                isHeadBanging = false;
                ChooseNewTarget();
            } else {
                HeadBang();
            }

        }
    } 

    protected override void InitAnimationParams(){
        base.InitAnimationParams();
        anim_die_trigger = Animator.StringToHash("die");
        anim_isZombie_bool = Animator.StringToHash("isZombie");
    }

    void FixedUpdate() {
        if (!isBeingControlled && IsGrounded() && !isHeadBanging) {
            MoveHorizontal(directionModifier);
            float y = transform.position.y - 1f;
            float x = transform.position.x + (1f * directionModifier);
            Vector3 endGroundCheck = new Vector3(x, y, transform.position.z);
            Debug.DrawLine(transform.position, endGroundCheck, Color.red);
            bool nearEdge = Physics2D.Linecast(transform.position, endGroundCheck, 1 << LayerMask.NameToLayer("Ground"));
            if (!nearEdge) {
                directionModifier *= -1;
            }
        }
    }

    public void ExecuteCommand(Command c) {
        if (isBeingControlled) {
            switch (c) {
            case Command.LEFT:
                MoveHorizontal(LEFT_DIRECTION);
                break;
            case Command.RIGHT:
                MoveHorizontal(RIGHT_DIRECTION);
                break;
            case Command.UP:
                HandleJumpInput();
                break;
            case Command.STOP:
                Stop();
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D c) {

        // Reached Lab
        if (c.tag == "Victory") {
            OnReachedVictoryPoint();
        } 

        // Reached desired Flamer
        else if (c.tag == "Flamer" 
            && c.GetComponent<Flamer>().GetInstanceID() == target.GetInstanceID()) {
            HeadBang();
        } 

        // Follow instructions to reach target Flamer
        else if (c.tag == "ControlPoint" && !isBeingControlled && target != null) {
            ControlPoint cp = c.GetComponent<ControlPoint>();
            Vector2 velocity = cp.getInstruction(target.GetInstanceID());
            if (velocity != Vector2.zero) {
                rb.velocity = velocity;
                if (velocity.y >= 0) {
                    animator.SetTrigger(anim_jump_trigger);
                }
            }
        }
    }

    private void OnReachedVictoryPoint() {
        gm.VillagerSaved();
		gameObject.SetActive(false);
    }

    private void BackToSelfControl() {
        ChooseNewTarget();
    }


    private Flamer ChooseNewTarget() {
        Debug.Log("Choosing new target...");
        Flamer[] flamers = FindObjectsOfType<Flamer>();
        if (flamers.Length > 0) {
            //TODO: make this weighted by distance. 
            int rndIndex = (int)Random.Range(0, flamers.Length);
            target = flamers[rndIndex];
            return flamers[rndIndex];
        }
        return null;
    }

    private void HeadBang(){
        if (!isHeadBanging) {
            isHeadBanging = true; // in case you didnt come from update
            Stop();
        }

        temprature += incrementTemperature;

        // burn
        if (temprature >= TEMPERATURE_LIMIT) {
            gm.VillagerDied();
            gameObject.SetActive(false);
        }

    }

}

