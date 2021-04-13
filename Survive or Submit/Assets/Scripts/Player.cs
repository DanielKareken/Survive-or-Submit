using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] WeaponContainer weaponContainer;
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] GameObject currWeapon;

    public Rigidbody2D rb;
    public Collider2D hitbox;
    public Camera cam;
    public GameObject sprite;
    public GameObject crosshair;
    public float moveSpeed;
    public float health;

    Weapon weapon;
    Vector2 movement;
    Vector2 mousePos;
    bool allowMovement;
    bool allowAiming;
    bool weaponEquiped;
    bool damagedRecently;
    float damageImmunityTimer;
    bool dead;
    bool inCraftingArea;
    bool crafting;
    int currMeds;

    // Start is called before the first frame update
    void Start()
    {
        //initialize RuntimeData vars
        runtimeData.player_scrap = 0;
        runtimeData.player_weapon = currWeapon;

        //init private variables
        allowMovement = true;
        allowAiming = true;
        weaponEquiped = true;
        dead = false;
        inCraftingArea = false;
        crafting = false;
        currMeds = 0;

        //access components
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CircleCollider2D>();
        currWeapon = runtimeData.player_weapon;
        weapon = currWeapon.GetComponent<Weapon>();

        Cursor.visible = false;

        //events
        GameEvents.CraftThis += OnThisCrafted;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowAiming)
        {
            Aim();
        }

        Safehouse();

        //check if health falls to ZERO
        if (health <= 0 && !dead)
        {
            dead = true;
            sprite.SetActive(false);
            hitbox.enabled = false;
            Invoke("Die", 2f);
        }
    }

    void FixedUpdate()
    {
        if(allowMovement && !dead)
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
            runtimeData.player_weapon.SetActive(true);

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

    //safehouse interactions
    void Safehouse()
    {
        //if E button pressed
        if (Input.GetKeyDown(KeyCode.E) && inCraftingArea)
        {
            if(!crafting)
            {
                //bring up crafting and halt player actions
                GameEvents.InvokeCraftingDisplay();
                allowAiming = false;
                allowMovement = false;
                Cursor.visible = true;
                crosshair.SetActive(false);
                crafting = true;
            }
            else
            {
                //hide crafting and allow player actions again
                GameEvents.InvokeCraftingHide();
                allowAiming = true;
                allowMovement = true;
                Cursor.visible = false;
                crosshair.SetActive(true);
                crafting = false;
            }
        }
    }

    //player getting hit by enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy")) {
            health -= 10f;
            print(health);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //picking up scrap
        if (collision.gameObject.CompareTag("Collectable"))
        {
            ScrapCollectable scrap = collision.gameObject.GetComponent<ScrapCollectable>();
            scrap.OnCollected();
            int amount = scrap.getQuantity();
            runtimeData.player_scrap += amount;
            print(runtimeData.player_scrap);
        }

        //on enter crafting area
        if (collision.gameObject.CompareTag("Crafting"))
        {
            inCraftingArea = true;
        }

        //on zone enter
        if (collision.gameObject.CompareTag("Zone"))
        {
            //update the zone the player is currently in to the prev zone
            int zone = collision.gameObject.GetComponent<ZoneHandler>().getZone();
            runtimeData.currZone = zone - 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //on exit crafting area
        if (collision.gameObject.CompareTag("Crafting"))
        {
            inCraftingArea = false;
        }

        //on zone exit
        if (collision.gameObject.CompareTag("Zone"))
        {
            //update the zone the player is currently in to the next zone
            int zone = collision.gameObject.GetComponent<ZoneHandler>().getZone();
            runtimeData.currZone = zone + 1;
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnThisCrafted(object sender, CraftEventArgs args)
    {
        //update weapon ammo
        currWeapon.GetComponent<Weapon>().addAmmo(args.ammoAmount);
        //update player meds
        currMeds += args.medAmount;
        print(currMeds);
    }

    //allow access to weapon by other objects
    public Weapon getWeapon()
    {
        return weapon;
    }

    //update weapon whenever player switches
    public void updateWeapon()
    {
        currWeapon = runtimeData.player_weapon;
        weapon = currWeapon.GetComponent<Weapon>();
    }

    
}
