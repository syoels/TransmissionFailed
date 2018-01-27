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
        Debug.Log("In ShowComics");
        blackPanel.gameObject.SetActive(true);
    }

    public void HideComics(){
        Debug.Log("In HideComics");
        blackPanel.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("In StartGame");
		SceneManager.LoadScene ("Level_1");
	}
}
