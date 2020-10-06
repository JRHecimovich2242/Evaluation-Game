using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGunCost : MonoBehaviour
{
    private Text _gunText;
    private StoreManager _storeManager;
    public bool Active;
    // Start is called before the first frame update
    void Start()
    {
        _gunText = GetComponent<Text>();
        _storeManager = FindObjectOfType<StoreManager>().GetComponent<StoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            _gunText.text = _storeManager.GetGunUpgradeCost().ToString() + " Coins";
        }
    }
}
