using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHighScoreText : MonoBehaviour
{
    Text highScore;
    // Start is called before the first frame update
    void Start()
    {
        highScore = GetComponent<Text>();
        highScore.text = PlayerPrefs.GetInt("highscore").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
