﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleshotPickup : MonoBehaviour
{
    [SerializeField] float despawnDelay = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, despawnDelay);
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
            other.GetComponent<PlayerController>().StartTripleShot();
            Destroy(gameObject);
        }
    }
}
