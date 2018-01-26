using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: 
public class GameManager : MonoBehaviour {

    [SerializeField] int totalVillagers; 
    [SerializeField] int livingVillagers; 
    [SerializeField] int savedVillagers; 
    [Range(0f, 100f)]
    public float villagersToSavePercent = 75f;
    [SerializeField] float percentAlive = 100f;

	// Use this for initialization
	void Start () {
        totalVillagers = FindObjectsOfType<VillagerController>().Length;
        livingVillagers = totalVillagers;
        savedVillagers = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void VillagerDied(){
        livingVillagers--;
        percentAlive = ((float)livingVillagers / totalVillagers) * 100f; 
        if (livingVillagers <= villagersToSavePercent) {
            GameOver();
        }
    }

    public void VillagerSaved(){
        savedVillagers++; 
        if (savedVillagers == livingVillagers) {
            GameWon();
        }
    }

    private void GameOver(){
        Debug.Log("Game over.. :(");
    }

    private void GameWon(){
        Debug.Log("You won!! woo hoo");
    }
}
