using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Weapon weapon;
    public Rigidbody2D rb;
    public Camera cam;
    public float moveSpeed;
    public float health;

    private Vector2 movement;
    private Vector2 mousePos;
    private bool allowMovement;
    private bool weaponEquiped;
    private bool damagedRecently;
    private float damageImmunityTimer;
    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        allowMovement = true;
        weaponEquiped = true;
        dead = false;

        //access components
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Aim();

        //check if health falls to ZERO
        if (health <= 0 && !dead)
        {
            dead = true;
            Invoke("Die", 2f);
        }
    }

    void FixedUpdate()
    {
        if(allowMovement)
        {
            Movement();
        }
    }

    void Movement()
    {
        //WASD or arrow keys allow for movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Aim()
    {
        //get location of mouse on screen
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //check if weapon is equipped to show weapon and attach it to player
        if (weaponEquiped)
        {
            weapon.gameObject.SetActive(true);
            
            //if left mouse clicked, shoot
            if(Input.GetMouseButtonDown(0))
            {
                weapon.Shoot();
            }

            //if r is pressed, reload
            if(Input.GetKeyDown(KeyCode.R))
            {
                weapon.Reload();
            }
        }
        

        Vector2 lookDir = mousePos - rb.position;
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = lookAngle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy")) {
            health -= 10f;
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
