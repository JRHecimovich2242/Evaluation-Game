using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _health = 100f;
    [SerializeField] float _damage = 25f;
    [SerializeField] float _knockbackStrength = 100f;
    [SerializeField] float _knockbackTime = .5f;
    [SerializeField] float _knockbackScale = 1f;
    [SerializeField] float _attackCooldown = 1f;
    [Header("Prefab References")]
    [SerializeField] ParticleSystem _deathVFX;
    

    private PlayerController _playerController;
    private Rigidbody2D _myRigidbody;

    private float _stunTime = 0f;
    private float _stunDuration = 0f;
    private float _attackTime = 0f;
    private Vector3 _vectorToPlayer;

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
        if (collision.gameObject.CompareTag("Player") && (Time.time - _attackTime >= _attackCooldown))
        {
            //Deal damage to player
            _attackTime = Time.time;
            _vectorToPlayer = transform.position - _playerController.transform.position;
            _vectorToPlayer.Normalize();
            _playerController.TakeDamage(_damage, _vectorToPlayer, _knockbackStrength, _knockbackTime);
        }
    }

    //Determines a vector towards the player and sets velocity along that vector
    private void MoveTowardsPlayer()
    {
        _vectorToPlayer = _playerController.transform.position - transform.position;
        _vectorToPlayer.Normalize();
        float z_rotation = Mathf.Atan2(_vectorToPlayer.y, _vectorToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, z_rotation + 90);
        _myRigidbody.velocity = transform.up * -_moveSpeed;
    }

    //knockbackDir is a normalized Vector3 representing the unit vector originating from the damaging object and pointing towards the damaged object
    public void TakeDamage(float damage, Vector3 knockbackDir, float knockbackStrength, float knockbackTime)
    {
        FindObjectOfType<EnemyAudio>().PlayHurtSound();
        _health -= damage;
        if(_health <= 0f)
        {
            //Handle enemy death
            var dropper = GetComponent<PowerupDropper>();
            if (dropper)
            {
                dropper.DropPickup();
            }
            
            Instantiate(_deathVFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        _myRigidbody.AddForce(knockbackDir * knockbackStrength * _knockbackScale);
        _stunTime = Time.time;
        _stunDuration = knockbackTime;
    }
}
