using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthCost : MonoBehaviour
{
    private Text _healthText;
    private StoreManager _storeManager;
    public bool Active = false;
    // Start is called before the first frame update
    void Start()
    {
        _healthText = GetComponent<Text>();
        _storeManager = FindObjectOfType<StoreManager>().GetComponent<StoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCostText();
    }

    private void UpdateCostText()
    {
        if (Active)
        {
            _healthText.text = _storeManager.GetHealthUpgradeCost().ToString() + " Coins";
        }
    }
}
