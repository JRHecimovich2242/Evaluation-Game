using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCurrency : MonoBehaviour
{
    private Text _coinCountText;
    private GameSession _gameSession;
    // Start is called before the first frame update
    void Start()
    {
        _coinCountText = GetComponent<Text>();
        _gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _coinCountText.text = _gameSession.GetCurrency().ToString() + " Coins";
    }
}
