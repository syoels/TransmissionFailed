using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : AbstractController {

    int temprature = 0;
    bool isBeingControlled = false;
    private bool prevIsBeingControlled = false;
    [SerializeField]
    private Flamer target;

    public override float moveSpeed { get { return 2f; } }

    public override float jumpForce { get { return 140f; } }

    protected override void Start() {
        Debug.Log("starting");
        base.Start();
        chooseNewTarget();
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
        prevIsBeingControlled = isBeingControlled;
        isBeingControlled = Input.GetKey(KeyCode.LeftControl);
        if (prevIsBeingControlled && isBeingControlled) {
//            BackToSelfControl();
        }
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
        if (c.tag == "Victory") {
            onReachedVictoryPoint();
        }
    }

    private void onReachedVictoryPoint() {
        gm.VillagerSaved();
        this.enabled = false;
    }

    private void BackToSelfControl() {
        chooseNewTarget();
    }


    private Flamer chooseNewTarget() {
        Flamer[] flamers = FindObjectsOfType<Flamer>();
        if (flamers.Length > 0) {
            //TODO: make this weighted by distance. 
            int rndIndex = (int)Random.Range(0, flamers.Length);
            target = flamers[rndIndex];
            return flamers[rndIndex];
        }
        return null;
    }


}

