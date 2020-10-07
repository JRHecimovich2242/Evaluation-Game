using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int _pickupValue = 10;
    [SerializeField] float _despawnDelay = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _despawnDelay);
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
            other.GetComponent<PlayerController>().IncreaseMaxAmmo(_pickupValue);
            Destroy(gameObject);
        }
    }
}
