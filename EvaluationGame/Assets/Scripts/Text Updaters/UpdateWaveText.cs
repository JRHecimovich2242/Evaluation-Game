using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWaveText : MonoBehaviour
{

    private Text _waveText;
    private GameSession _gameSession;
    // Start is called before the first frame update
    void Start()
    {
        _gameSession = FindObjectOfType<GameSession>();
        _waveText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWave();   
    }

    private void UpdateWave()
    {
        _waveText.text = (_gameSession.GetWave() + 1).ToString();
    }
}
