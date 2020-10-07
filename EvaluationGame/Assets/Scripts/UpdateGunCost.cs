using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGunCost : MonoBehaviour
{
    private Text _gunCostText;
    private StoreManager _storeManager;
    public bool Active;
    // Start is called before the first frame update
    void Start()
    {
        _gunCostText = GetComponent<Text>();
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
            _gunCostText.text = _storeManager.GetGunUpgradeCost().ToString() + " Coins";
        }
    }
}
