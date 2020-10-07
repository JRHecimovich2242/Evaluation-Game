using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject _healthBar;
    [SerializeField] float _healthBarWidth;
    [SerializeField] float _healthBarSmooth;
    [SerializeField] float _healthBarSmoothEase;

    [SerializeField] Text ammoCountText;
    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _healthBarWidth = 1;
        _healthBarSmooth = _healthBarWidth;
        _playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();

    }

    private void UpdateHealthBar()
    {
        _healthBarWidth = _playerController.GetHealthFraction();
        _healthBarSmooth += (_healthBarWidth - _healthBarSmooth) * Time.deltaTime * _healthBarSmoothEase;
        _healthBar.transform.localScale = new Vector2(_healthBarSmooth, transform.localScale.y);
    }

    
}
