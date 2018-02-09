using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TODO: 
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    // Stats
    [SerializeField] private int totalVillagers; 
    [SerializeField] private int livingVillagers; 
    [SerializeField] private int savedVillagers; 
    [SerializeField] private int deadVillagers; 
    [Range(0f, 100f)]
    public float villagersToSavePercent = 75f;
    [SerializeField] private float percentAlive = 100f;

    // Lab related
    public GameObject lab;
    private Animator labAnimator;
    private int labAnim_save_trigger;

    // UI
	public Canvas canvas;
	private Animator canvasAnimator;
    public float NOTIFIACTION_TIME = 2.5f;
    public Text villagersLeftNotification; 
    public Text notifications; 
    public Text villagersSaved; 

    // Level related
	private int anim_game_over_trigger;
	public GameObject gameOver;
    public SceneField nextLevel;

    public CameraFollow cam;

	// Use this for initialization
	void Start () {
		canvasAnimator = canvas.GetComponent<Animator> ();
		anim_game_over_trigger = Animator.StringToHash ("gameOver");

        labAnimator = lab.GetComponent<Animator>();
        labAnim_save_trigger = Animator.StringToHash("VillagerSaved");
        deadVillagers = 0;
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
        deadVillagers++;
		UpdateUIText ();
        percentAlive = ((float)livingVillagers / totalVillagers) * 100f; 
        if (percentAlive <= villagersToSavePercent) {
            GameOver();
            return;
        }
        // Win after a villager is dead but player still passed the required capacity
        bool allVillagersAreGone = (deadVillagers + savedVillagers == totalVillagers);
        if (allVillagersAreGone && percentAlive > villagersToSavePercent) {
            GameWon();
        }
    }

    public void VillagerSaved(){
        labAnimator.SetTrigger(labAnim_save_trigger);
        SetNotificationText("You Saved a Villager!\nRock On!!");
		savedVillagers++; 
		UpdateUIText ();
        if (savedVillagers == livingVillagers) {
            GameWon();
        }
    }

    public void GameOver(float timeToExitScene = 5f){
        cam.StopFollowing();
		gameOver.SetActive (true);
        Debug.Log("Game over.. :(");
        SetNotificationText("Game Over.. boo hoo :(");
		canvasAnimator.SetTrigger (anim_game_over_trigger);
        StartCoroutine(Replay(timeToExitScene));
    }

    private void GameWon(){
        Debug.Log("You won!! woo hoo");
        SetNotificationText("You Won! Woo hoo!");
        StartCoroutine(LoadNextLevel(5f));
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

    IEnumerator Replay(float time){
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene ("Intro");
    }

    IEnumerator LoadNextLevel(float time){
        yield return new WaitForSeconds(time);
        if (this.nextLevel != null) {
            SceneManager.LoadScene(this.nextLevel);   
        } else {
            SceneManager.LoadScene("Intro");
        }
    }


}
