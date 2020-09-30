using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int pickupValue = 10;
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
            other.GetComponent<PlayerController>().IncreaseMaxAmmo(pickupValue);
            Destroy(gameObject);
        }
    }
}
