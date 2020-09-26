using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //Cached component references
    Rigidbody2D _myRigidbody;


    //
    private Vector2 _moveInput;
    private Vector3 _cursorPosition;
    private Vector3 _difference;
    private float _shotTime = 0f;
    private float _stunTime = 0f;
    private float _stunDuration = 0f;
    private bool _isAlive = true;

    [SerializeField] float health = 100f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float shootingCooldown = .5f;
    [SerializeField] GameObject gunBarrel;
    [SerializeField] Rigidbody2D projectile;
    [SerializeField] ParticleSystem hurtParticles;
    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (_isAlive)
        {
            if (Time.time - _stunTime >= _stunDuration)
            {
                Move();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            FaceCursor();
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
    }

    private void Move()
    {
        _myRigidbody.velocity = new Vector2((Input.GetAxis("Horizontal") * moveSpeed), (Input.GetAxis("Vertical") * moveSpeed));
    }

    private void FaceCursor()
    {
        _cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _difference = _cursorPosition - transform.position;
        _difference.Normalize();
        float z_rotation = Mathf.Atan2(_difference.y, _difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, z_rotation);
    }

    private void Shoot()
    {
        if (Time.time - _shotTime >= shootingCooldown){
            _shotTime = Time.time;
            Rigidbody2D bullet = Instantiate(projectile, gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
            //bullet.AddForce(transform.right * bulletSpeed);
            bullet.velocity = transform.right * bulletSpeed;
        }

    }
    //knockbackDir is a normalized Vector3 representing the unit vector originating from the damaging entity and pointing towards the player
    public void TakeDamage(float damage, Vector3 knockbackDir, float knockbackStrength, float knockbackTime)
    {
        health -= damage;
        Instantiate(hurtParticles, transform.position, Quaternion.identity);
        if(health <= 0)
        {
            _isAlive = false;
            //GAME OVER
            FindObjectOfType<GameSession>().EndGame();
        }
        _myRigidbody.AddForce(knockbackDir * knockbackStrength);
        _stunTime = Time.time;
        _stunDuration = knockbackTime;
    }
}
