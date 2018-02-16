using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : AbstractController {

    public float controlRadius = 10f;
    private float haloScaleFactor = 0.285f;
    private Transform halo;

	public override float moveSpeed { get { return 3.25f; }}
	public override float jumpForce { get { return 140f; }} 
    private bool isControlling = false;
    private bool isControllingPrev = false;

    // Audio
    Transform sounds;
    AudioSource soundHalo; 

	// Animation
	int anim_control_trigger;
	int anim_control_release_trigger;
	public Animator haloAnimator;
	int anim_halo_in_control;

    protected override void Start() {
        base.Start();
        initAudioSources();
		anim_control_trigger = Animator.StringToHash ("control");
		anim_control_release_trigger = Animator.StringToHash ("controlRelease");

        
        halo = transform.Find("Halo");
        halo.localScale = new Vector3(controlRadius * haloScaleFactor, controlRadius * haloScaleFactor, 1f);
        haloAnimator = halo.GetComponent<Animator> ();
		anim_halo_in_control = Animator.StringToHash ("inControl"); 
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

        isControllingPrev = isControlling;
        isControlling = Input.GetKey(CONTROL);
		if (isControlling) {
			animator.SetTrigger (anim_control_trigger);
		} else {
			animator.SetTrigger (anim_control_release_trigger);
		}
		haloAnimator.SetBool (anim_halo_in_control, isControlling);

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

        handleSounds();
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

    void OnTriggerEnter2D(Collider2D c) {
        if (c.tag == "Death") {
            gm.GameOver();

        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, controlRadius);
    }

    private void initAudioSources(){
        sounds = transform.Find("Sounds");
        soundHalo = sounds.Find("Halo").GetComponent<AudioSource>();
    }

    private void handleSounds(){
        if (!isControllingPrev && isControlling) {
            soundHalo.Play();
        }
    }


}
