using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {

	public Button newGame;
    public Button next;
    public Button esc;


    public GameObject blackPanel; 
    public SceneField nextLevel;


    enum CurrentState {
        START,
        COMICS
    };
    private CurrentState currentState = CurrentState.START;
    private KeyCode[] startGameKeys = { KeyCode.Space, KeyCode.RightArrow, KeyCode.KeypadEnter};
    private KeyCode[] startComicsKeys = { KeyCode.Space, KeyCode.KeypadEnter, KeyCode.Return};
    private KeyCode[] exitComicsKeys = { KeyCode.Escape, KeyCode.LeftArrow};



	void Start()
	{



		Button btnNew = newGame.GetComponent<Button>();
        Button btnEsc = esc.GetComponent<Button>();
        Button btnNxt = next.GetComponent<Button>();
        btnNew.onClick.AddListener(ShowComics);
        btnEsc.onClick.AddListener(HideComics);
        btnNxt.onClick.AddListener(StartGame);
	}

    void Update(){
        if (Input.anyKey) {
            if (currentState == CurrentState.START && 
                IsOneOfKeysArrayPressed(startComicsKeys)) {
                ShowComics();
            } else if (currentState == CurrentState.COMICS && 
                IsOneOfKeysArrayPressed(exitComicsKeys)) {
                HideComics();
            } else if (currentState == CurrentState.COMICS && 
                IsOneOfKeysArrayPressed(startGameKeys)) {
                StartGame();
            } 
        }
    }

    public void ShowComics(){
        blackPanel.gameObject.SetActive(true);
        currentState = CurrentState.COMICS;
    }

    public void HideComics(){
        blackPanel.gameObject.SetActive(false);
        currentState = CurrentState.START;
    }

    public void StartGame()
    {
        if (this.nextLevel != null) {
            SceneManager.LoadScene(nextLevel);
        } else {
            SceneManager.LoadScene("Level_1_tutorial");
        }
	}

    private bool IsOneOfKeysArrayPressed(KeyCode[] keys){
        foreach (KeyCode c in keys) {
            if (Input.GetKeyDown(c)) {
                return true;
            }
        }
        return false;
    }
}
