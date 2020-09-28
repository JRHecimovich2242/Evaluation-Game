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
    private bool _hitboxActive = true;
    [SerializeField] bool _tripleshotActive = false;
    [SerializeField] float _tripleshotAmmo = 20f;
    private int _tripleshotAngle = 30;
    

    [SerializeField] float health = 100f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float shootingCooldown = .5f;
    [SerializeField] float damageCooldown = 1f;
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
        if (!_tripleshotActive)
        {
            if (Time.time - _shotTime >= shootingCooldown)
            {
                _shotTime = Time.time;
                Rigidbody2D bullet = Instantiate(projectile, gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
                //bullet.AddForce(transform.right * bulletSpeed);
                bullet.velocity = transform.right * bulletSpeed;
            }
        }
        else
        {
            if(_tripleshotAmmo <= 0)
            {
                _tripleshotActive = false;
            }
            else if(Time.time - _shotTime >= shootingCooldown)
            {
                Debug.Log(transform.right);
                //Fire tripleshot
                _shotTime = Time.time;
                Rigidbody2D bullet1 = Instantiate(projectile, gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
                //bullet.AddForce(transform.right * bulletSpeed);
                bullet1.velocity = RotateVector(transform.right, _tripleshotAngle) * bulletSpeed;
                _shotTime = Time.time;
                Rigidbody2D bullet2 = Instantiate(projectile, gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
                //bullet.AddForce(transform.right * bulletSpeed);
                bullet2.velocity = RotateVector(transform.right, -_tripleshotAngle) * bulletSpeed;
                Rigidbody2D bullet3 = Instantiate(projectile, gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
                bullet3.velocity = transform.right * bulletSpeed;
            }
        }

    }
    //knockbackDir is a normalized Vector3 representing the unit vector originating from the damaging entity and pointing towards the player
    public void TakeDamage(float damage, Vector3 knockbackDir, float knockbackStrength, float knockbackTime)
    {
        if (_hitboxActive)
        {
            health -= damage;
            Instantiate(hurtParticles, transform.position, Quaternion.identity);
            if (health <= 0)
            {
                _isAlive = false;
                //GAME OVER
                FindObjectOfType<GameSession>().EndGame();
            }
            //_myRigidbody.AddForce(knockbackDir * knockbackStrength);
            //_stunTime = Time.time;
            //_stunDuration = knockbackTime;
            StartCoroutine(DisableHurt());
        }
    }

    IEnumerator DisableHurt()
    {
        _hitboxActive = false;
        yield return new WaitForSeconds(damageCooldown);
        _hitboxActive = true;
    }

    public void UpdateFireRate(float newFiringDelay)
    {
        shootingCooldown = newFiringDelay;
    }

    public void StartTripleShot()
    {

    }

    private Vector3 RotateVector(Vector3 vect, int angle)
    {
        float rads = angle * Mathf.Deg2Rad;
        return new Vector3((vect.x * Mathf.Cos(rads) - vect.y * Mathf.Sin(rads)),
                           (vect.x * Mathf.Sin(rads) + vect.y * Mathf.Cos(rads)),
                           vect.z);
    }
}
