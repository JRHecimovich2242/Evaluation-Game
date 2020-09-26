using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    private WaveManager _waveManager; 

    private float _gameStartTime = 0f;
    private int _activeGameTime = 0;
    private int _currentWave = 0;
    private bool _gameActive = false;

    private void Awake()
    {
        SetUpSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting game Session");
        _waveManager = FindObjectOfType<WaveManager>();
        //Debug.Log(_waveManager.name);
        //Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameActive)
        {
            _activeGameTime = (int)(Time.time - _gameStartTime);
        }
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateCurrentWave(int currWave)
    {
        _currentWave = currWave;
    }

    //Called from start button on canvas
    public void StartGame()
    {
        Debug.Log("Starting game");
        _gameStartTime = Time.time;
        _waveManager.GameActive = true;
        _gameActive = true;
        //Time.timeScale = 1;
    }

    public int GetScore()
    {
        return _activeGameTime;
    }

    public int GetWave()
    {
        return _currentWave;
    }

    public void ResetGame()
    {
        Debug.Log("Heyu");
        Destroy(gameObject);
    }

    public void EndGame()
    {
        _waveManager.enabled = false;
        _gameActive = false;
        SceneManager.LoadScene(2);

    }
}
