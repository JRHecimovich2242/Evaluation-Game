using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDropper : MonoBehaviour
{
    [SerializeField] HealthPickup healthPickupPrefab;
    [SerializeField] CurrencyCoin coinPrefab;
    [SerializeField] TripleshotPickup tripleshotPickupPrefab;
    [SerializeField] int percentChanceHealthDrop = 30;
    [SerializeField] int percentChanceCoinDrop = 30;
    [SerializeField] int percentChanceTripleshotDrop = 30;
    private int healthLowerBound = 0;
    private int healthUpperBound = 30;
    private int coinLowerBound = 31;
    private int coinUpperBound = 50;
    private int tripleshotLowerBound = 51;
    private int tripleshotUpperBound = 60;

    // Start is called before the first frame update
    void Start()
    {
        if(percentChanceCoinDrop + percentChanceHealthDrop + percentChanceTripleshotDrop > 100)
        {
            //If the set drop chances are incompatible, set them to some preset values
            Debug.LogError("Drop chances cannot sum to be greater than 100");
            percentChanceHealthDrop = 30;
            percentChanceCoinDrop = 20;
            percentChanceTripleshotDrop = 10;
        }
        healthLowerBound = 0;
        healthUpperBound = percentChanceHealthDrop;
        coinLowerBound = percentChanceHealthDrop + 1;
        coinUpperBound = percentChanceHealthDrop + percentChanceCoinDrop;
        tripleshotLowerBound = coinUpperBound + 1;
        tripleshotUpperBound = coinUpperBound + percentChanceTripleshotDrop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropPickup()
    {
        int randNum = Random.Range(0, 101);
        if (healthLowerBound <= randNum && randNum <= healthUpperBound)
        {
            //Drop ammo pickuo
            Instantiate(healthPickupPrefab, this.transform.position, Quaternion.identity);
        }
        else if (coinLowerBound <= randNum && randNum <= coinUpperBound)
        {
            //Drop health pickup
            Instantiate(coinPrefab, this.transform.position, Quaternion.identity);
        }
        else if (tripleshotLowerBound <= randNum && randNum <= tripleshotUpperBound)
        {
            //Drop tripleshot pickup
            Instantiate(tripleshotPickupPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
