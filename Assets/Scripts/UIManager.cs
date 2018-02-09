using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public Fade levelMarker;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void LoadLevelMarker(){
        if (levelMarker != null) {
            levelMarker.FadeIn();
        }
    }
}
