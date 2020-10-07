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

    private int numGunUpgrades = 0;

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
        if (numGunUpgrades < 10)
        {
            FindObjectOfType<UpdateGunCost>().Active = true;
        }
        
    }

    public void DisableStoreUI()
    {
        FindObjectOfType<UpdateAmmoCost>().Active = false;
        FindObjectOfType<UpdateHealthCost>().Active = false;
        if(numGunUpgrades < 10)
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
        }
    }

    public void UpgradeAmmo()
    {
        if (_gameSession.SpendCurrency(_ammoUpgradeCost))
        {
            _player.IncreaseMaxAmmo(_ammoUpgradeValue);
        }
    }

    public void UpgradeGun()
    {
        if(numGunUpgrades < 10)
        {
            if (_gameSession.SpendCurrency(_gunUpgradeCost))
            {
                _player.UpdateFireRate();
                numGunUpgrades++;
            }
        }
        if(numGunUpgrades >= 10)
        {
            //Disable gun upgrade buttons
            DisableGunUpgradeButtons("Gun Upgrade");
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

    private void DisableGunUpgradeButtons(string tag)
    {
        GameObject[] canvasItems = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject item in canvasItems)
        {
            item.SetActive(false);
        }
    }
}
