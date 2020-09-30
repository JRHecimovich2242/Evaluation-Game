using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAmmoText : MonoBehaviour
{
    private Text _ammoCountText;
    private PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _ammoCountText = GetComponent<Text>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _ammoCountText.text = _playerController.GetCurrentAmmo().ToString();
    }
}
