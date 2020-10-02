using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDropper : MonoBehaviour
{
    [SerializeField] HealthPickup healthPickupPrefab;
    [SerializeField] AmmoPickup ammoPickupPrefab;
    [SerializeField] TripleshotPickup tripleshotPickupPrefab;
    [SerializeField] int percentChanceHealthDrop = 30;
    [SerializeField] int percentChanceAmmoDrop = 30;
    [SerializeField] int percentChanceTripleshotDrop = 30;
    private int healthLowerBound = 0;
    private int healthUpperBound = 30;
    private int ammoLowerBound = 31;
    private int ammoUpperBound = 50;
    private int tripleshotLowerBound = 51;
    private int tripleshotUpperBound = 60;

    // Start is called before the first frame update
    void Start()
    {
        if(percentChanceAmmoDrop + percentChanceHealthDrop + percentChanceTripleshotDrop > 100)
        {
            Debug.LogError("Drop chances cannot sum to be greater than 100");
            percentChanceHealthDrop = 30;
            percentChanceAmmoDrop = 20;
            percentChanceTripleshotDrop = 10;
        }
        healthLowerBound = 0;
        healthUpperBound = percentChanceHealthDrop;
        ammoLowerBound = percentChanceHealthDrop + 1;
        ammoUpperBound = percentChanceHealthDrop + percentChanceAmmoDrop;
        tripleshotLowerBound = ammoUpperBound + 1;
        tripleshotUpperBound = ammoUpperBound + percentChanceTripleshotDrop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropPickup()
    {
        int randNum = Random.Range(0, 100);
        if (healthLowerBound <= randNum && randNum <= healthUpperBound)
        {
            //Drop ammo pickuo
            Instantiate(healthPickupPrefab, this.transform.position, Quaternion.identity);
        }
        else if (ammoLowerBound <= randNum && randNum <= ammoUpperBound)
        {
            //Drop health pickup
            Instantiate(ammoPickupPrefab, this.transform.position, Quaternion.identity);
        }
        else if (tripleshotLowerBound <= randNum && randNum <= tripleshotUpperBound)
        {
            //Drop tripleshot pickup
            Instantiate(tripleshotPickupPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
