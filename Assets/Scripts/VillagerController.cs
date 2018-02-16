using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : AbstractController {
    
    // Temperature
    public float temprature = 0f;
    public float TEMPERATURE_LIMIT = 10f; 
    private float incrementTemperature = 0.05f;
    [Range(0f, 100f)]
    public float chanceToChangeFlamer = 0.02f;
    public Sprite[] thermostatImages; 
    private SpriteRenderer thermostatSr;
    public List<float> thermostatLevels = new List<float>();

    // Mind control
    bool isBeingControlled = false;
    private bool prevIsBeingControlled = false; 
    public bool isHeadBanging = false;

    // Animation
    int anim_isZombie_bool;
    int anim_die_trigger;
    int anim_isHeadBanging_bool;

    // Wait
    private bool waitForLand_ = false;
    private bool WaitForLand {
        get { return !isBeingControlled && waitForLand_; }
        set { waitForLand_ = value;}
    }

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
        InitThermostat();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        UpdateThermostatLevels();
        animator.SetBool(anim_isZombie_bool, isBeingControlled);
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
        anim_isHeadBanging_bool = Animator.StringToHash("isHeadBanging");
    }

    void FixedUpdate() {
        if (WaitForLand) {
            return;
        }
        if (!isBeingControlled && IsGrounded() && !isHeadBanging) {
            MoveHorizontal(directionModifier);
            float y = transform.position.y - 1f;
            float x = transform.position.x + (1f * directionModifier);
            Vector3 endGroundCheck = new Vector3(x, y, transform.position.z);
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
                    WaitForLand = true;
                    directionModifier = velocity.x > 0 ? RIGHT_DIRECTION : LEFT_DIRECTION;
                    animator.SetTrigger(anim_jump_trigger);
                }
            }
        } else if (c.tag == "Death") {
            Explode();

        }
    }

    void OnTriggerExit2D(Collider2D c) {
        if (c.tag == "Flamer") {
            isHeadBanging = false; // in case you didnt come from update
            animator.SetBool(anim_isHeadBanging_bool, false);
        }
    }

    void OnCollisionEnter2D(Collision2D c){
        //TODO: basically we only need this for floors & platforms, consider changing
        WaitForLand = false;
    }


    private void OnReachedVictoryPoint() {
        gm.VillagerSaved();
		gameObject.SetActive(false);
    }

    private void BackToSelfControl() {
        animator.SetBool(anim_isZombie_bool, false);
        ChooseNewTarget();
    }


    private Flamer ChooseNewTarget() {
        animator.SetBool(anim_isHeadBanging_bool, false);
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
            animator.SetBool(anim_isHeadBanging_bool, true);
            Stop();
        }

        temprature += incrementTemperature;

        // burn
        if (temprature >= TEMPERATURE_LIMIT) {
            animator.SetBool(anim_isHeadBanging_bool, false);
            animator.SetTrigger(anim_die_trigger);
        }

    }

    private void InitThermostat(){
        thermostatSr = transform.Find("Thermostat").GetComponent<SpriteRenderer>();
        int levels = thermostatImages.Length; 
        float level = TEMPERATURE_LIMIT / levels;
        for (int i = 0; i < levels; i++) {
            thermostatLevels.Add(level * i);
        }
    }

    private void UpdateThermostatLevels(){
        float[] levels = thermostatLevels.ToArray();
        for (int i = 0; i < levels.Length; i++) {
            if (i == thermostatLevels.Count - 1) {
                if (temprature >= levels[i]) {
                    thermostatSr.sprite = thermostatImages[i];
                    return;
                }
            } else {
                if (temprature >= levels[i] && temprature < levels[i + 1]) {
                    thermostatSr.sprite = thermostatImages[i];
                    return;
                }
            }
        }
    }

    public void Explode(){
        gm.VillagerDied();
        gameObject.SetActive(false);
    }
       
}

