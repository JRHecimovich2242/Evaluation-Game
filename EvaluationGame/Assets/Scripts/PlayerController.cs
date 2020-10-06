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
    private bool _dashing = false;
    public bool InShop = false;
    //private float _dashStartTime = 0f;
    [SerializeField] int _maxAmmo = 30;
    private int _currentAmmo = 0;
    [SerializeField] bool _tripleshotActive = false;
    [SerializeField] int _tripleshotAngle = 30;
    [SerializeField] float reloadTime = 2f;
    [SerializeField] bool _reloading = false;
    

    [SerializeField] float _maxHealth = 100f;
    private float _health = 0f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float moveLimiter = 0.7f;
    //[SerializeField] float dashSpeed = 2f;
    //[SerializeField] float dashDuration = .25f;
    //[SerializeField] float dashCooldown = 5f;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float shootingCooldown = .5f;
    [SerializeField] float damageCooldown = 1f;
    [SerializeField] GameObject gunBarrel;
    [SerializeField] Rigidbody2D projectile;
    [SerializeField] ParticleSystem hurtParticles;
    [SerializeField] AudioClip gunSound;
    [SerializeField] AudioClip hurtNoise;
    [SerializeField] AudioClip pickupNoise;
    [SerializeField] AudioClip reloadSound;
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
            if (Time.time - _stunTime >= _stunDuration && !InShop)
            {
                Move();
            }
            else if (InShop)
            {
                _myRigidbody.velocity = Vector2.zero;
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
                StartCoroutine(Reload());
            }
        }
    }

    private void Move()
    {
        float yInput = Input.GetAxisRaw("Vertical");
        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0 && yInput != 0)
        {
            xInput *= moveLimiter;
            yInput *= moveLimiter;
        }
        _myRigidbody.velocity = new Vector2((xInput * moveSpeed), (yInput * moveSpeed));
        
    }
    /*
    IEnumerator Dash()
    {
        _dashing = true;
        _dashStartTime = Time.time;
        _myRigidbody.velocity = new Vector2((Input.GetAxis("Horizontal") * dashSpeed), (Input.GetAxis("Vertical") *  dashSpeed));
        yield return new WaitForSeconds(dashDuration);
        _dashing = false;
    }
    */

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
        if (_hitboxActive && !_dashing)
        {
            _health -= damage;
            _myAudioSource.PlayOneShot(hurtNoise);
            Instantiate(hurtParticles, transform.position, Quaternion.identity);
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
        _hitboxActive = false;
        yield return new WaitForSeconds(damageCooldown);
        _hitboxActive = true;
    }

    public void UpdateFireRate()
    {
        shootingCooldown *= .9f;
        if(shootingCooldown <= 0)
        {
            shootingCooldown = .1f;
        }
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
        _myAudioSource.PlayOneShot(reloadSound);
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

    //Increase maxAmmo by pickupValue
    public void IncreaseMaxAmmo(int pickupValue)
    {
        _maxAmmo += pickupValue;
        _myAudioSource.PlayOneShot(pickupNoise);
    }

    //Restore health by pickupValue
    public void RestoreHealth(float pickupValue)
    {
        _health += pickupValue;
        if(_health > _maxHealth)
        {
            _health = _maxHealth;
        }
        _myAudioSource.PlayOneShot(pickupNoise);
    }

    public void IncreaseMaxHealth(float upgradeValue)
    {
        _maxHealth += upgradeValue;
        _myAudioSource.PlayOneShot(pickupNoise);
    }

    public void PlayPickupNoise()
    {
        _myAudioSource.PlayOneShot(pickupNoise);
        _myAudioSource.PlayOneShot(pickupNoise);
    }
}
