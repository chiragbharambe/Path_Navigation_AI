using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timescore : MonoBehaviour
{
    // Start is called before the first frame update
 public Text ScoreText;
 public Text HighScore;
 public Text PlayerScore;
 private float timer;
 private int score;

 void Start()
 {
 	HighScore.text = "HighScore:" + PlayerPrefs.GetInt("HighScore",0).ToString();
    PlayerScore.text = "PlayerScore:" + PlayerPrefs.GetInt("PlayerScore",0).ToString();
 }
 void Update () {

    timer += Time.deltaTime;

    if (timer > 5f) {

        score += 5;

        //We only need to update the text if the score changed.
        ScoreText.text = "Score:" + score.ToString();
//diversityText.text = "Diversity: " + currentGeneration.diversity;
//	}
        //Reset the timer to 0.
        timer = 0;

    }

    PlayerPrefs.SetInt("PlayerScore",score);
 if (score > PlayerPrefs.GetInt("HighScore",0))
 {
 PlayerPrefs.SetInt("HighScore",score);

 HighScore.text = "HighScore:" + score.ToString();
    }
}

  }