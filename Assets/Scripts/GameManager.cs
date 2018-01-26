using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TODO: 
public class GameManager : MonoBehaviour {

    [SerializeField] private int totalVillagers; 
    [SerializeField] private int livingVillagers; 
    [SerializeField] private int savedVillagers; 
    [Range(0f, 100f)]
    public float villagersToSavePercent = 75f;
    [SerializeField] private float percentAlive = 100f;

    public float NOTIFIACTION_TIME = 2.5f;
    public Text villagersLeftNotification; 
    public Text notifications; 
    public Text villagersSaved; 


	// Use this for initialization
	void Start () {
        totalVillagers = FindObjectsOfType<VillagerController>().Length;
        livingVillagers = totalVillagers;
        savedVillagers = 0;
		UpdateUIText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void VillagerDied(){
        SetNotificationText("Another Villager Burned Himself To Death!!!\nHURRY UP!");
		livingVillagers--;
		UpdateUIText ();
        percentAlive = ((float)livingVillagers / totalVillagers) * 100f; 
        if (livingVillagers <= villagersToSavePercent) {
            GameOver();
        }
    }

    public void VillagerSaved(){
        SetNotificationText("You Saved a Villager!\nRock On!!");
		savedVillagers++; 
		UpdateUIText ();
        if (savedVillagers == livingVillagers) {
            GameWon();
        }
    }

    private void GameOver(){
        Debug.Log("Game over.. :(");
        SetNotificationText("Game Over.. boo hoo :(");
    }

    private void GameWon(){
        Debug.Log("You won!! woo hoo");
        SetNotificationText("You Won! Woo hoo!");
    }

    private void UpdateUIText(){
        villagersLeftNotification.text = "Villagers Left: " + livingVillagers + " / " + totalVillagers;
        villagersSaved.text = "Villagers Saved: " + savedVillagers; 
    }

    private void SetNotificationText(string txt){
        notifications.text = txt;
        StartCoroutine(ResetNotification(NOTIFIACTION_TIME));
    }
        

    IEnumerator ResetNotification(float time){
        yield return new WaitForSeconds(time);
        notifications.text = "";
    }
}
