using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    [SerializeField] private float healthBarWidth;
    [SerializeField] private float healthBarSmooth;
    [SerializeField] private float healthBarSmoothEase;

    [SerializeField] Text ammoCountText;
    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        healthBarWidth = 1;
        healthBarSmooth = healthBarWidth;
        _playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();

    }

    private void UpdateHealthBar()
    {
        healthBarWidth = _playerController.GetHealthFraction();
        healthBarSmooth += (healthBarWidth - healthBarSmooth) * Time.deltaTime * healthBarSmoothEase;
        healthBar.transform.localScale = new Vector2(healthBarSmooth, transform.localScale.y);
    }

    
}
