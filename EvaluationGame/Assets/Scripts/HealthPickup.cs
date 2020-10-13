using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float pickupValue = 10f;
    [SerializeField] float despawnDelay = 10f;

    private float _startTime = 0f;
    private float _timeElapsed = 0f;
    private Color _spriteColor;

    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteColor = _spriteRenderer.color;
        Destroy(gameObject, despawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed = Time.time - _startTime;
        _spriteColor.a = Mathf.Lerp(1f, 0f, _timeElapsed / despawnDelay);
        _spriteRenderer.color = _spriteColor;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        //Debug.Log("Health pickup");
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().RestoreHealth(pickupValue);
            Destroy(gameObject);
        }
    }

    
}
