using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour {

	bool isBeingControlled = false;
	VillagerMovement villagerMovement;
	// Use this for initialization
	void Start () {
		villagerMovement = GetComponent<VillagerMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		OnControllingPlayerMoveLeft ();
	}

	public void OnControllingPlayerMoveLeft() {
		villagerMovement.MoveLeft ();
	}

	public void OnControllingPlayerMoveRight() {
		villagerMovement.MoveRight ();
	}
}
