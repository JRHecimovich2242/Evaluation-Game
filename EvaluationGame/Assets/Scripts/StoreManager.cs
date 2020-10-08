using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    private PlayerController _player;
    private GameSession _gameSession;

    [SerializeField] int _healthUpgradeCost = 10;
    [SerializeField] int _ammoUpgradeCost = 10;
    [SerializeField] int _gunUpgradeCost = 10;
    [SerializeField] int _ammoUpgradeValue = 5;
    [SerializeField] int _healthUpgradeValue = 10;
    [SerializeField] int _gunTypeUpgradeCost = 50;
    

    private int _numGunUpgrades = 0;
    private int _numHealthUpgrades = 0;
    private int _numAmmoUpgrades = 0;
    private bool _gunTypeUpgradeActive = true;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableStoreUI()
    {
        GetComponent<Canvas>().enabled = true;
        FindObjectOfType<UpdateAmmoCost>().Active = true;
        FindObjectOfType<UpdateHealthCost>().Active = true;
        if (_numGunUpgrades < 10)
        {
            FindObjectOfType<UpdateGunCost>().Active = true;
        }
        if (_gunTypeUpgradeActive)
        {
            FindObjectOfType<UpdateGunTypeCost>().Active = true;
        }

        
    }

    public void DisableStoreUI()
    {
        FindObjectOfType<UpdateAmmoCost>().Active = false;
        FindObjectOfType<UpdateHealthCost>().Active = false;
        if(_numGunUpgrades < 10)
        {
            FindObjectOfType<UpdateGunCost>().Active = false;
        }
        
        GetComponent<Canvas>().enabled = false;
        _gameSession.ResumeGame();
    }

    //Upgrade functions are called on their corresponding button press
    public void UpgradeHealth()
    {
        if (_gameSession.SpendCurrency(_healthUpgradeCost))
        {
            _player.IncreaseMaxHealth(_healthUpgradeValue);
            _numHealthUpgrades++;
            if (_numHealthUpgrades % 3 == 0 && _numHealthUpgrades > 0)
            {
                _healthUpgradeCost += 5;
            }
        }
        
    }

    public void UpgradeAmmo()
    {
        if (_gameSession.SpendCurrency(_ammoUpgradeCost))
        {
            _player.IncreaseMaxAmmo(_ammoUpgradeValue);
            _numAmmoUpgrades++;
            if (_numAmmoUpgrades % 3 == 0 && _numAmmoUpgrades > 0)
            {
                _ammoUpgradeCost += 5;
            }
        }
        
    }

    public void UpgradeGun()
    {
        if(_numGunUpgrades < 12)
        {
            if (_gameSession.SpendCurrency(_gunUpgradeCost))
            {
                _player.UpdateFireRate();
                _numGunUpgrades++;
                if (_numGunUpgrades % 3 == 0 && _numGunUpgrades > 0)
                {
                    _gunUpgradeCost += 5;

                }
            }
        }
        
        if (_numGunUpgrades >= 12)
        {
            //Disable gun upgrade buttons
            DisableGunUpgradeButtons("Gun Upgrade");
        }
    }

    public void UpgradeGunType()
    {
        if (_gameSession.SpendCurrency(_gunTypeUpgradeCost))
        {
            FindObjectOfType<PlayerController>().StartTripleShot();
            FindObjectOfType<PlayerController>().PermaTripleshot = true;
            DisableGunTypeUpgrade("Gun Type");
        }
        
    }

    public int GetAmmoUpgradeCost()
    {
        return _ammoUpgradeCost;
    }

    public int GetHealthUpgradeCost()
    {
        return _healthUpgradeCost;
    }

    public int GetGunUpgradeCost()
    {
        return _gunUpgradeCost;
    }

    public int GetGunTypeUpgradeCost()
    {
        return _gunTypeUpgradeCost;
    }

    private void DisableGunUpgradeButtons(string tag)
    {
        GameObject[] canvasItems = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject item in canvasItems)
        {
            item.SetActive(false);
        }
    }


    public void DisableGunTypeUpgrade(string tag)
    {
        GameObject[] canvasItems = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject item in canvasItems)
        {
            item.SetActive(false);
        }
        _gunTypeUpgradeActive = false;
    }
}
