using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target = null; 
    [Range(0f, 1f)]
    public float smoothing = 0.125f;
    public Vector3 offset = new Vector3(1f, 1f, 0f);
    private bool isFollowing = true;
    private int framesToEndFollow = 10;

	// Use this for initialization. //TODO: Doesn't work when not assigned from inpesctor
	void Start () {
        if (target == null){
            target = FindObjectOfType<PlayerController>().transform;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isFollowing || --framesToEndFollow > 0) {
            Vector3 targetPosition = target.position - offset;
            Vector2 positionXY = Vector2.Lerp(transform.position, targetPosition, smoothing);
            transform.position = new Vector3(positionXY.x, positionXY.y, -10f);
        }
	}

    public void StopFollowing(){
        this.isFollowing = false;
    }
}
