using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {

    public Text ScoreText;
	// Use this for initialization
	void Start () {
        ScoreText.text = "Score : " + ("" + GameMaster.PreviousScore).PadLeft(7, '0') + " Points. \nGit Gud ";
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("P1A"))
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }
        
    }
}
