using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAmmoCost : MonoBehaviour
{

    private Text _ammoText;
    private StoreManager _storeManager;
    public bool Active = false;
    // Start is called before the first frame update
    void Start()
    {
        _ammoText = GetComponent<Text>();
        _storeManager = FindObjectOfType<StoreManager>().GetComponent<StoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            _ammoText.text = _storeManager.GetAmmoUpgradeCost().ToString() + " Coins";
        }
    }
}
