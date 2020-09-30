using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float pickupValue = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("Ammo pickup");
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().RestoreHealth(pickupValue);
            Destroy(gameObject);
        }
    }
}
