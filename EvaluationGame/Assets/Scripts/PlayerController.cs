using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //Cached component references
    Rigidbody2D myRigidbody;


    //
    private Vector2 moveInput;
    private Vector3 cursorPosition;
    private Vector3 difference;
    private float shotTime = 0f;


    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float shootingCooldown = .5f;
    [SerializeField] GameObject gunBarrel;
    [SerializeField] Rigidbody2D projectile;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        FaceCursor();
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Move()
    {
        myRigidbody.velocity = new Vector2((Input.GetAxis("Horizontal") * moveSpeed), (Input.GetAxis("Vertical") * moveSpeed));
    }

    private void FaceCursor()
    {
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        difference = cursorPosition - transform.position;
        difference.Normalize();
        float z_rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, z_rotation);
    }

    private void Shoot()
    {
        if (Time.time - shotTime >= shootingCooldown){
            shotTime = Time.time;
            Rigidbody2D bullet = Instantiate(projectile, gunBarrel.transform.position, Quaternion.identity) as Rigidbody2D;
            //bullet.AddForce(transform.right * bulletSpeed);
            bullet.velocity = transform.right * bulletSpeed;
        }

    }

}
