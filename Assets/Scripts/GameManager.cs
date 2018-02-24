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
    //TODO: move all UI functions to UIManager
	public Canvas canvas;
	private Animator canvasAnimator;
    public float NOTIFIACTION_TIME = 2.5f;
    public Text villagersLeftNotification; 
    public Text notifications; 
    public Text villagersSaved; 

    // Level related
	private int anim_game_over_trigger;
    private int anim_start_level_trigger;
	public GameObject gameOver;
    public SceneField nextLevel;

    // Debug Cheats :)
    public bool cheatsEnabled = false; 
    public Vector2 velocityToAllVillagers = new Vector2(5f, 2f);
    public KeyCode keyToAddVelocity = KeyCode.V;

    public CameraFollow cam;

	// Use this for initialization
	void Start () {
		canvasAnimator = canvas.GetComponent<Animator> ();
		anim_game_over_trigger = Animator.StringToHash ("gameOver");
        anim_start_level_trigger = Animator.StringToHash ("startLevel");
        canvasAnimator.SetTrigger(anim_start_level_trigger);

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
        if (cheatsEnabled) {
            HandleCheats();
        }
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
        if (WinConditionsApply()) {
            GameWon();
        }
    }

    public void VillagerSaved(){
        labAnimator.SetTrigger(labAnim_save_trigger);
        SetNotificationText("You Saved a Villager!\nRock On!!");
		savedVillagers++; 
		UpdateUIText ();
        if (WinConditionsApply()) {
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
	
    private bool WinConditionsApply(){
    	bool allVillagersAreGone = (deadVillagers + savedVillagers == totalVillagers);
        return (allVillagersAreGone && percentAlive > villagersToSavePercent);
    }

    //TODO: move all UI functions to UIManager
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
	
	 // Restart level
	 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene ("Intro"); // To restart entire game
    }

    IEnumerator LoadNextLevel(float time){
        yield return new WaitForSeconds(time);
        if (this.nextLevel != null) {
            SceneManager.LoadScene(this.nextLevel);   
        } else {
            SceneManager.LoadScene("Intro");
        }
    }

    private void HandleCheats(){
        if (!cheatsEnabled) {
            Debug.Log("Cheats activated even though flag is off !");
        }
        VillagerController[] villagers = FindObjectsOfType<VillagerController>();
        if (Input.GetKeyDown(keyToAddVelocity)) {
            foreach (VillagerController vc in villagers) {
                Rigidbody2D rb = vc.GetComponent<Rigidbody2D>();
                Vector2 velocity = new Vector2(velocityToAllVillagers.x, velocityToAllVillagers.y);
                velocity.x *= vc.directionModifier;
                rb.velocity += velocity;
            }
        }
        foreach (VillagerController vc in villagers) {
            Rigidbody2D rb = vc.GetComponent<Rigidbody2D>();
            Vector3 from = vc.transform.position;
            Vector3 to = vc.transform.position +
                new Vector3(rb.velocity.x, rb.velocity.y, 0f);
            Debug.DrawLine(from, to);
            if (vc.directionModifier == -1 &&
               from.x < to.x) {
                Debug.Log("RIGHT SURPRISE");
            } 
            if (vc.directionModifier == 1 &&
                from.x > to.x) {
                Debug.Log("LEFT SURPRISE");
            } 
        }

    }


}
