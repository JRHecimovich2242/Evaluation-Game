using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    private WaveManager _waveManager;
    private StoreManager _storeManager;

    private float _gameStartTime = 0f;
    private int _activeGameTime = 0;
    private int _currentWave = 0;
    private bool _gameActive = false;
    private int _currency = 0;
    private float _timePaused = 0f;

    private void Awake()
    {
        SetUpSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Starting game Session");
        _waveManager = FindObjectOfType<WaveManager>();
        _storeManager = FindObjectOfType<StoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameActive)
        {
            _activeGameTime = (int)(Time.time - _gameStartTime - _timePaused);
        }
        else{
            _timePaused += Time.deltaTime;
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
        //Debug.Log("Resetting");
        Destroy(gameObject);
    }

    public void EndGame()
    {
        Time.timeScale = .7f;
        _waveManager.enabled = false;
        _gameActive = false;
        if(GetScore() > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", GetScore());
        }
        StartCoroutine(LoadGameOver());

    }
    IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }

    public void IncreaseCurrency(int value)
    {
        _currency += value;
    }

    public bool SpendCurrency(int upgradeValue)
    {
        if (_currency - upgradeValue >= 0)
        {
            _currency -= upgradeValue;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetCurrency()
    {
        return _currency;
    }

    //SuspendGame enables the store UI and disables player movement
    public void SuspendGame()
    {
        _storeManager.EnableStoreUI();
        _gameActive = false;
        FindObjectOfType<PlayerController>().InShop = true;
    }

    //ResumeGame reenables player movement and sets the game state to active
    public void ResumeGame()
    {
        _waveManager.ResumeGame();
        _gameActive = true;
        FindObjectOfType<PlayerController>().InShop = false;
    }
}
