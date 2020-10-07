using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDropper : MonoBehaviour
{
    [SerializeField] HealthPickup _healthPickupPrefab;
    [SerializeField] CurrencyCoin _coinPrefab;
    [SerializeField] TripleshotPickup _tripleshotPickupPrefab;
    [SerializeField] int _percentChanceHealthDrop = 30;
    [SerializeField] int _percentChanceCoinDrop = 30;
    [SerializeField] int _percentChanceTripleshotDrop = 30;
    private int _healthLowerBound = 0;
    private int _healthUpperBound = 30;
    private int _coinLowerBound = 31;
    private int _coinUpperBound = 50;
    private int _tripleshotLowerBound = 51;
    private int _tripleshotUpperBound = 60;

    // Start is called before the first frame update
    void Start()
    {
        if(_percentChanceCoinDrop + _percentChanceHealthDrop + _percentChanceTripleshotDrop > 100)
        {
            //If the set drop chances are incompatible, set them to some preset values
            Debug.LogError("Drop chances cannot sum to be greater than 100");
            _percentChanceHealthDrop = 30;
            _percentChanceCoinDrop = 20;
            _percentChanceTripleshotDrop = 10;
        }
        _healthLowerBound = 0;
        _healthUpperBound = _percentChanceHealthDrop;
        _coinLowerBound = _percentChanceHealthDrop + 1;
        _coinUpperBound = _percentChanceHealthDrop + _percentChanceCoinDrop;
        _tripleshotLowerBound = _coinUpperBound + 1;
        _tripleshotUpperBound = _coinUpperBound + _percentChanceTripleshotDrop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropPickup()
    {
        int randNum = Random.Range(0, 101);
        if (_healthLowerBound <= randNum && randNum <= _healthUpperBound)
        {
            //Drop ammo pickuo
            Instantiate(_healthPickupPrefab, this.transform.position, Quaternion.identity);
        }
        else if (_coinLowerBound <= randNum && randNum <= _coinUpperBound)
        {
            //Drop health pickup
            Instantiate(_coinPrefab, this.transform.position, Quaternion.identity);
        }
        else if (_tripleshotLowerBound <= randNum && randNum <= _tripleshotUpperBound)
        {
            //Drop tripleshot pickup
            Instantiate(_tripleshotPickupPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
