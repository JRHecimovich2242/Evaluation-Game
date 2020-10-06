using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScoreText : MonoBehaviour
{

    private Text _scoreText;
    private GameSession _gameSession;
    // Start is called before the first frame update
    void Start()
    {
        _gameSession = FindObjectOfType<GameSession>();
        _scoreText = GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        _scoreText.text = _gameSession.GetScore().ToString();
    }
}
