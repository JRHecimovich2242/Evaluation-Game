using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunProjectile : MonoBehaviour
{

    [SerializeField] float damage = 100f;
    [SerializeField] float knockbackStrength = 100f;
    [SerializeField] float knockbackTime = .5f;

    private Rigidbody2D _myRigidbody;
    private Vector2 _moveDirection;
    private bool _doneDamage = false;


    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _moveDirection = _myRigidbody.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collided");
        GameObject other = collision.gameObject;
        if(other.CompareTag("Enemy") && !_doneDamage)
        {
            //In some cases a single projectile was doing damage twice, this should fix that
            _doneDamage = true;
            _moveDirection.Normalize();
            other.GetComponent<EnemyController>().TakeDamage(damage, _moveDirection, knockbackStrength, knockbackTime);
            Destroy(this.gameObject);
        }
        else if(collision.CompareTag("Environment"))
        {
            Destroy(this.gameObject);
        }
    }
}
