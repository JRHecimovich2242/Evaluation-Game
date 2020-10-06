using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float health = 100f;
    [SerializeField] float damage = 25f;
    [SerializeField] float knockbackStrength = 100f;
    [SerializeField] float knockbackTime = .5f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] ParticleSystem deathVFX;
    [SerializeField] float knockbackScale = 1f;

    private PlayerController _playerController;
    private Rigidbody2D _myRigidbody;
    private float _stunTime = 0f;
    private float _stunDuration = 0f;
    private float _attackTime = 0f;


    Vector3 vectorToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Time.time - _stunTime >= _stunDuration)
        {
            MoveTowardsPlayer();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("We eating?");
        if (collision.gameObject.CompareTag("Player") && (Time.time - _attackTime >= attackCooldown))
        {
            //Debug.Log("Eat that Player");
            //Deal damage to player
            _attackTime = Time.time;
            //PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            vectorToPlayer = transform.position - _playerController.transform.position;
            vectorToPlayer.Normalize();
            _playerController.TakeDamage(damage, vectorToPlayer, knockbackStrength, knockbackTime);
        }
    }

    //Determines a vector towards the player and sets velocity along that vector
    private void MoveTowardsPlayer()
    {
        vectorToPlayer = _playerController.transform.position - transform.position;
        vectorToPlayer.Normalize();
        //_myRigidbody.velocity = vectorToPlayer * moveSpeed;
        float z_rotation = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, z_rotation + 90);
        _myRigidbody.velocity = transform.up * -moveSpeed;
    }

    //knockbackDir is a normalized Vector3 representing the unit vector originating from the damaging entity and pointing towards the player
    public void TakeDamage(float damage, Vector3 knockbackDir, float knockbackStrength, float knockbackTime)
    {
        FindObjectOfType<EnemyAudio>().PlayHurtSound();
        health -= damage;
        if(health <= 0f)
        {
            //Handle enemy death
            var dropper = GetComponent<PowerupDropper>();
            if (dropper)
            {
                dropper.DropPickup();
            }
            
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        _myRigidbody.AddForce(knockbackDir * knockbackStrength * knockbackScale);
        _stunTime = Time.time;
        _stunDuration = knockbackTime;
    }
}
