using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRunFunctionOnNearbyVillagers : MonoBehaviour {


    public float radius = 10f;

    void Start(){
        
    }

    void Update(){

        if (Input.GetKeyDown(KeyCode.Space)){
            testVillagerShowAffected[] villagers = FindObjectsOfType<testVillagerShowAffected>();
            foreach (testVillagerShowAffected villager in villagers) {

                if ((transform.position - villager.transform.position).magnitude <= radius) {
                    villager.changeSpriteColor();
                }

            }
        }

    }




    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
