using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    PlayerController player;
    GameSession gameSession;

    [SerializeField] int healthUpgradeCost = 10;
    [SerializeField] int ammoUpgradeCost = 10;
    [SerializeField] int gunUpgradeCost = 10;
    [SerializeField] int ammoUpgradeValue = 5;
    [SerializeField] int healthUpgradeValue = 10;

    private int numGunUpgrades = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        gameSession = FindObjectOfType<GameSession>();
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
        gameSession.ResumeGame();
    }

    public void UpgradeHealth()
    {
        if (gameSession.SpendCurrency(healthUpgradeCost))
        {
            player.IncreaseMaxHealth(healthUpgradeValue);
        }
    }

    public void UpgradeAmmo()
    {
        if (gameSession.SpendCurrency(ammoUpgradeCost))
        {
            player.IncreaseMaxAmmo(ammoUpgradeValue);
        }
    }

    public void UpgradeGun()
    {
        if(numGunUpgrades < 10)
        {
            if (gameSession.SpendCurrency(gunUpgradeCost))
            {
                player.UpdateFireRate();
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
        return ammoUpgradeCost;
    }

    public int GetHealthUpgradeCost()
    {
        return healthUpgradeCost;
    }

    public int GetGunUpgradeCost()
    {
        return gunUpgradeCost;
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
