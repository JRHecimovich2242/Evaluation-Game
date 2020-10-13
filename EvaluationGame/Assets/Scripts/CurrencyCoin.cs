using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyCoin : MonoBehaviour
{
    //[SerializeField] float _despawnDelay = 10f;
    [SerializeField] int _coinValue = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, _despawnDelay);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameSession>().IncreaseCurrency(_coinValue);
            FindObjectOfType<PlayerController>().PlayPickupNoise();
            Destroy(gameObject);
        }
    }
}
