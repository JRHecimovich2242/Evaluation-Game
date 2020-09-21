using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunProjectile : MonoBehaviour
{
    Rigidbody2D _myRigidbody;
    Vector2 moveDirection; 


    [SerializeField] float damage = 100f;
    [SerializeField] float knockbackStrength = 100f;
    [SerializeField] float knockbackTime = .5f;
    [SerializeField] ParticleSystem shrapnel;
    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        moveDirection = _myRigidbody.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collided");
        GameObject other = collision.gameObject;
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Shot enemy");
            moveDirection.Normalize();
            Debug.Log("oo");
            other.GetComponent<EnemyController>().TakeDamage(damage, moveDirection, knockbackStrength, knockbackTime);
            //Instantiate(shrapnel);
            Debug.Log("Ayyy");
            Destroy(this.gameObject);
        }
        else if(collision.tag == "Environment")
        {
            Destroy(this.gameObject);
        }
    }
}
