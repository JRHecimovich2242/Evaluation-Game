using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //Cached component references
    Rigidbody2D _myRigidbody;
    AudioSource _myAudioSource;
    BoxCollider2D _myBoxCollider;

    //
    private Vector2 _moveInput;
    private Vector3 _cursorPosition;
    private Vector3 _difference;
    private float _shotTime = 0f;
    private float _stunTime = 0f;
    private float _stunDuration = 0f;
    private bool _isAlive = true;
    private bool _hitboxActive = true;
    private float _health = 0f;
    private int _currentAmmo = 0;
    public bool InShop = false;
    public bool PermaTripleshot = false;

    [Header("Player Attributes")]
    [SerializeField] int _maxAmmo = 30;
    [SerializeField] bool _tripleshotActive = false;
    [SerializeField] int _tripleshotAngle = 30;
    [SerializeField] float _reloadTime = 2f;
    [SerializeField] bool _reloading = false;
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _moveLimiter = 0.7f;
    [SerializeField] float _bulletSpeed = 5f;
    [SerializeField] float _shootingCooldown = .5f;
    [SerializeField] float _damageCooldown = 1f;
    [Header("Prefab References")]
    [SerializeField] GameObject _gunBarrel;
    [SerializeField] Rigidbody2D _projectile;
    [SerializeField] ParticleSystem _hurtParticles;
    [SerializeField] AudioClip _gunSound;
    [SerializeField] AudioClip _hurtNoise;
    [SerializeField] AudioClip _pickupNoise;
    [SerializeField] AudioClip _reloadSound;

    Coroutine reload;
    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAudioSource = GetComponent<AudioSource>();
        _myBoxCollider = GetComponent<BoxCollider2D>();
        _currentAmmo = _maxAmmo;
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
            if (Input.GetKey(KeyCode.Mouse0) && !_reloading && !InShop)
            {
                Shoot();
            }
            if (Input.GetKeyDown(KeyCode.R) && !_reloading && !InShop)
            {
                reload = StartCoroutine(Reload());
            }
        }
    }

    private void Move()
    {
        float yInput = Input.GetAxisRaw("Vertical");
        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0 && yInput != 0)
        {
            //Utilize moveLimiter to prevent diagonal movement from being faster than only X or only Y movement
            xInput *= _moveLimiter;
            yInput *= _moveLimiter;
        }
        _myRigidbody.velocity = new Vector2((xInput * _moveSpeed), (yInput * _moveSpeed));
        
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
        if(_currentAmmo > 0)
        {
            if (Time.time - _shotTime >= _shootingCooldown)
            {
                _myAudioSource.PlayOneShot(_gunSound);
                _currentAmmo--;
                _shotTime = Time.time;
                Rigidbody2D bullet = Instantiate(_projectile, _gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
                bullet.velocity = transform.right * _bulletSpeed;
                if (_tripleshotActive)
                {
                    Rigidbody2D bullet1 = Instantiate(_projectile, _gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
                    bullet1.velocity = RotateVector(transform.right, _tripleshotAngle) * _bulletSpeed;
                    _shotTime = Time.time;
                    Rigidbody2D bullet2 = Instantiate(_projectile, _gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
                    bullet2.velocity = RotateVector(transform.right, -_tripleshotAngle) * _bulletSpeed;
                }
            }
        }
        else
        {
            reload = StartCoroutine(Reload());
        }
    }


    //knockbackDir is a normalized Vector3 representing the unit vector originating from the damaging entity and pointing towards the player
    public void TakeDamage(float damage, Vector3 knockbackDir, float knockbackStrength, float knockbackTime)
    {
        if (_hitboxActive)
        {
            _health -= damage;
            _myAudioSource.PlayOneShot(_hurtNoise);
            Instantiate(_hurtParticles, transform.position, Quaternion.identity);
            if (_health <= 0)
            {
                _isAlive = false;
                //GAME OVER
                FindObjectOfType<GameSession>().EndGame();
                _myRigidbody.velocity = Vector2.zero;
            }
            //_stunTime = Time.time;
            //_stunDuration = knockbackTime;
            //_myRigidbody.AddForce(-knockbackDir * knockbackStrength);
            StartCoroutine(DisableHurt());
        }
    }

    IEnumerator DisableHurt()
    {
        //Begin a short cooldown to disable the player's hitbox so it isnt swarmed by zombies and destroyed too quickly
        _hitboxActive = false;
        yield return new WaitForSeconds(_damageCooldown);
        _hitboxActive = true;
    }

    public void UpdateFireRate()
    {
        _shootingCooldown *= .9f;
        if(_shootingCooldown <= 0)
        {
            _shootingCooldown = .1f;
        }
        _myAudioSource.PlayOneShot(_pickupNoise);
    }

    private Vector3 RotateVector(Vector3 vect, int angle)
    {
        float rads = angle * Mathf.Deg2Rad;
        return new Vector3((vect.x * Mathf.Cos(rads) - vect.y * Mathf.Sin(rads)),
                           (vect.x * Mathf.Sin(rads) + vect.y * Mathf.Cos(rads)),
                           vect.z);
    }

    public float GetHealthFraction()
    {
        return _health / _maxHealth;
    }

    public int GetCurrentAmmo()
    {
        return _currentAmmo;
    }

    public int GetMaxAmmo()
    {
        return _maxAmmo;
    }

    IEnumerator Reload()
    {
        _myAudioSource.PlayOneShot(_reloadSound);
        _reloading = true;
        yield return new WaitForSeconds(_reloadTime);
        if (!PermaTripleshot)
        {
            _tripleshotActive = false;
        }
        _currentAmmo = _maxAmmo;
        _reloading = false;
    }

    public void StartTripleShot()
    {
        if (_reloading)
        {
            StopCoroutine(reload);
            _reloading = false;
        }
        _tripleshotActive = true;
        _currentAmmo = _maxAmmo;
        _myAudioSource.PlayOneShot(_pickupNoise);
    }

    //Increase maxAmmo by pickupValue
    public void IncreaseMaxAmmo(int pickupValue)
    {
        _maxAmmo += pickupValue;
        _myAudioSource.PlayOneShot(_pickupNoise);
    }

    //Restore health by pickupValue
    public void RestoreHealth(float pickupValue)
    {
        _health += pickupValue;
        if(_health > _maxHealth)
        {
            _health = _maxHealth;
        }
        _myAudioSource.PlayOneShot(_pickupNoise);
    }

    public void IncreaseMaxHealth(float upgradeValue)
    {
        _maxHealth += upgradeValue;
        _health += upgradeValue;
        _myAudioSource.PlayOneShot(_pickupNoise);
    }

    public void PlayPickupNoise()
    {
        _myAudioSource.PlayOneShot(_pickupNoise);
    }
}
