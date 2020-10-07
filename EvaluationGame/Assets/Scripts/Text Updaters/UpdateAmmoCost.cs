using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAmmoCost : MonoBehaviour
{

    private Text _ammoCostText;
    private StoreManager _storeManager;
    public bool Active = false;
    // Start is called before the first frame update
    void Start()
    {
        _ammoCostText = GetComponent<Text>();
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
            _ammoCostText.text = _storeManager.GetAmmoUpgradeCost().ToString() + " Coins";
        }
    }
}
