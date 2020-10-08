using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGunTypeCost : MonoBehaviour
{
    private Text _gunTypeCostText;
    private StoreManager _storeManager;
    public bool Active;
    // Start is called before the first frame update
    void Start()
    {
        _gunTypeCostText = GetComponent<Text>();
        _storeManager = FindObjectOfType<StoreManager>().GetComponent<StoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGunTypeCostText();
    }

    private void UpdateGunTypeCostText()
    {
        if (Active)
        {
            Debug.Log("OOOOOOOOOOOOO");
            _gunTypeCostText.text = _storeManager.GetGunTypeUpgradeCost().ToString() + " Coins";
        }
    }
}
