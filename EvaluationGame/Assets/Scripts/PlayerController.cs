using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //Cached component references
    Rigidbody2D _myRigidbody;
    AudioSource _myAudioSource;


    //
    private Vector2 _moveInput;
    private Vector3 _cursorPosition;
    private Vector3 _difference;
    private float _shotTime = 0f;
    private float _stunTime = 0f;
    private float _stunDuration = 0f;
    private bool _isAlive = true;
    private bool _hitboxActive = true;
    [SerializeField] int _maxAmmo = 30;
    private int _currentAmmo = 0;
    [SerializeField] bool _tripleshotActive = false;
    [SerializeField] int _tripleshotAngle = 30;
    [SerializeField] float reloadTime = 2f;
    [SerializeField] bool _reloading = false;
    

    [SerializeField] float _maxHealth = 100f;
    private float _health = 0f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float shootingCooldown = .5f;
    [SerializeField] float damageCooldown = 1f;
    [SerializeField] GameObject gunBarrel;
    [SerializeField] Rigidbody2D projectile;
    [SerializeField] ParticleSystem hurtParticles;
    [SerializeField] AudioClip gunSound;
    [SerializeField] AudioClip hurtNoise;
    [SerializeField] AudioClip pickupNoise;
    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAudioSource = GetComponent<AudioSource>();
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
            if (Input.GetKey(KeyCode.Mouse0) && !_reloading)
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
        if(_currentAmmo > 0)
        {
            if (Time.time - _shotTime >= shootingCooldown)
            {
                _myAudioSource.PlayOneShot(gunSound);
                _currentAmmo--;
                _shotTime = Time.time;
                Rigidbody2D bullet = Instantiate(projectile, gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
                //bullet.AddForce(transform.right * bulletSpeed);
                bullet.velocity = transform.right * bulletSpeed;
                if (_tripleshotActive)
                {
                    Rigidbody2D bullet1 = Instantiate(projectile, gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
                    //bullet.AddForce(transform.right * bulletSpeed);
                    bullet1.velocity = RotateVector(transform.right, _tripleshotAngle) * bulletSpeed;
                    _shotTime = Time.time;
                    Rigidbody2D bullet2 = Instantiate(projectile, gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
                    //bullet.AddForce(transform.right * bulletSpeed);
                    bullet2.velocity = RotateVector(transform.right, -_tripleshotAngle) * bulletSpeed;
                }
            }
        }
        else
        {
            StartCoroutine(Reload());
        }
    }


    //knockbackDir is a normalized Vector3 representing the unit vector originating from the damaging entity and pointing towards the player
    public void TakeDamage(float damage, Vector3 knockbackDir, float knockbackStrength, float knockbackTime)
    {
        if (_hitboxActive)
        {
            _health -= damage;
            _myAudioSource.PlayOneShot(hurtNoise);
            Instantiate(hurtParticles, transform.position, Quaternion.identity);
            if (_health <= 0)
            {
                _isAlive = false;
                //GAME OVER
                FindObjectOfType<GameSession>().EndGame();
            }
            //_stunTime = Time.time;
            //_stunDuration = knockbackTime;
            //_myRigidbody.AddForce(-knockbackDir * knockbackStrength);
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
        _reloading = true;
        yield return new WaitForSeconds(reloadTime);
        _tripleshotActive = false;
        _currentAmmo = _maxAmmo;
        _reloading = false;
    }

    public void StartTripleShot()
    {
        _tripleshotActive = true;
        _currentAmmo = _maxAmmo;
        _myAudioSource.PlayOneShot(pickupNoise);
    }

    public void IncreaseMaxAmmo(int pickupValue)
    {
        _maxAmmo += pickupValue;
        _myAudioSource.PlayOneShot(pickupNoise);
    }

    public void RestoreHealth(float pickupValue)
    {
        _health += pickupValue;
        if(_health > _maxHealth)
        {
            _maxHealth += pickupValue / 4;
            _health = _maxHealth;
        }
        _myAudioSource.PlayOneShot(pickupNoise);
    }
}
