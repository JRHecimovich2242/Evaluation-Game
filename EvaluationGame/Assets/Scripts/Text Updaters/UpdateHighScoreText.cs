using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHighScoreText : MonoBehaviour
{
    private Text _highScore;
    // Start is called before the first frame update
    void Start()
    {
        _highScore = GetComponent<Text>();
        _highScore.text = PlayerPrefs.GetInt("highscore").ToString();
    }
}
