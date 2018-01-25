using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testVillagerShowAffected : MonoBehaviour {

    private SpriteRenderer sr;
    public int framesToColor = 24; 
    private int framesToColorCounter = 0;
    public Color origColor; 
    public Color changeColor; 


    // Use this for initialization
    void Start () {
        sr = GetComponentInChildren<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update () {
        if (framesToColorCounter > 0) {
            framesToColorCounter--;
        } else {
            //sr.color = origColor;
        }

    }

    // doesnt really color but never mind.. :)
    public void changeSpriteColor(){
        framesToColorCounter = framesToColor;
        sr.color = changeColor;
    }
}
