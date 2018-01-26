using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour {


    [System.Serializable]
    public struct Instruction {
        public Flamer target;
        public  Vector3 velocity;
    }
    public Instruction[] mapping; 

    private Dictionary<int, Vector3> innerMapping = new Dictionary<int, Vector3>();

	// Use this for initialization
	void Start () {
        foreach (Instruction i in mapping) {
            innerMapping.Add(i.target.GetInstanceID(), i.velocity);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 getInstruction(int instanceID){
        if(innerMapping.ContainsKey(instanceID)){
            return innerMapping[instanceID];
        } else {
            return Vector3.zero;
        }
    }
}
