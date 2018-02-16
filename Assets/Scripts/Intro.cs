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



	void Start()
	{
        Debug.Log("In start");
		Button btnNew = newGame.GetComponent<Button>();
        Button btnEsc = esc.GetComponent<Button>();
        Button btnNxt = next.GetComponent<Button>();
        btnNew.onClick.AddListener(ShowComics);
        btnEsc.onClick.AddListener(HideComics);
        btnNxt.onClick.AddListener(StartGame);
	}

    public void ShowComics(){
        blackPanel.gameObject.SetActive(true);
    }

    public void HideComics(){
        blackPanel.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("In StartGame");
        if (this.nextLevel != null) {
            SceneManager.LoadScene(nextLevel);
        } else {
            SceneManager.LoadScene("Level_1_tutorial");
        }
	}
}
