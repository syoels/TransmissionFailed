using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerMovement : MonoBehaviour {

	public int moveSpeed = 1;
	public Rigidbody2D rb2d; 
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {
		//MoveLeft ();
	}

	public void MoveLeft() {
		Vector2 movement = Vector2.zero;
		movement.x = (transform.right*Time.deltaTime*-moveSpeed).x;
		movement += (Vector2)(transform.position);
		rb2d.MovePosition(movement);
	}

	public void MoveRight() {
		Vector2 movement = Vector2.zero;
		movement.x = (transform.right*Time.deltaTime*moveSpeed).x;
		movement += (Vector2)(transform.position);
		rb2d.MovePosition(movement);	}

    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("collided with");
        Debug.Log(other);
    }
}
