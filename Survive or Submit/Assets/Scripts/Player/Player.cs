using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] GameObject[] Weapons;
    [SerializeField] MedKitContainer medKitType;
    [SerializeField] GameObject vCamBounds;
    
    public Animator anim;
    public Rigidbody2D rb;
    public Collider2D hitbox;
    public Camera cam;
    public GameObject sprite;
    public GameObject crosshair;
    public float baseMoveSpeed;
    public int maxHealth;

    Weapon currWeapon;
    int currWeaponIndex;
    Vector2 movement;
    Vector2 mousePos;
    float currMoveSpeed;
    bool allowActions;
    bool damagedRecently;
    float damageImmunityTimer;
    int health;
    int healAmount;
    bool dead;
    bool inCraftingArea;
    bool crafting;
    bool healOnCD;
    int tempFix = 1; //temporary fix for zone triggers all entering on start

    // Start is called before the first frame update
    void Start()
    {
        //initialize weapons
        int totalWeapons = Weapons.Length;
        foreach (GameObject wp in Weapons)
        {
            wp.SetActive(false);
        }

        Weapons[0].SetActive(true);

        //init private variables
        allowActions = true;
        dead = false;
        inCraftingArea = false;
        crafting = false;
        currWeaponIndex = 0;
        health = maxHealth;
        healOnCD = false;
        currMoveSpeed = baseMoveSpeed;

        //access components
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CircleCollider2D>();
        //weapon = currWeapon[currWeaponIndex].GetComponent<Weapon>();
        anim = GetComponent<Animator>();

        Cursor.visible = false;

        //events
        GameEvents.CraftAmmo += OnAmmoCrafted;
        GameEvents.UpdateWeapon += OnWeaponSelected;
        GameEvents.WeaponReloaded += OnWeaponReloaded;
        GameEvents.HealEnded += OnHealReady;
        GameEvents.EndGame += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        //player actions
        if (allowActions && !dead)
        {                        
            Heal();
            Reload();
        }
        //player interacts
        Safehouse();

        vCamBounds.transform.position = transform.position;
    }

    void FixedUpdate()
    {
        if (allowActions && !dead)
        { 
            Movement();
            Aim();
        }  
    }

    void Movement()
    {
        //WASD or arrow keys allow for movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        rb.MovePosition(rb.position + movement * currMoveSpeed * Time.fixedDeltaTime);

        if(movement.x == 0 && movement.y == 0.0)
        {
            anim.SetBool("IsWalking", false);
        }
        else
        {
            anim.SetBool("IsWalking", true);
        }
    }

    void Aim()
    {
        //get location of mouse on screen
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - rb.position;
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = lookAngle;
    }

    void Reload()
    {
        //if r is pressed, reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            currWeapon.CallReload();
        }
    }

    void Heal()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!healOnCD && runtimeData.player_meds > 0)
            {                   
                if (health > maxHealth - medKitType.healAmount)
                {
                    health = maxHealth;
                }
                else
                {
                    health += medKitType.healAmount;
                }

                GameEvents.InvokeUpdateHealthUI(health);
                GameEvents.InvokeActivateHeal(medKitType.healCooldown);
                healOnCD = true;
                runtimeData.player_meds--;
            }
        }       
    }

    //safehouse interactions
    void Safehouse()
    {
        //if E button pressed
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)) && inCraftingArea)
        {
            if(!crafting)
            {
                //bring up crafting and halt player actions
                GameEvents.InvokeCraftingDisplay();
                allowActions = false;
                Cursor.visible = true;
                crosshair.SetActive(false);
                crafting = true;
                runtimeData.allowWeaponFire = false;
            }
            else
            {
                //hide crafting and allow player actions again
                GameEvents.InvokeCraftingHide();
                allowActions = true;
                Cursor.visible = false;
                crosshair.SetActive(true);
                crafting = false;
                runtimeData.allowWeaponFire = true;
            }
        }
    }

    //player getting hit by enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy")) {
            int amount = collision.gameObject.GetComponent<Enemy>().getDamage();
            takeDamage(amount);
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
            GameEvents.InvokeUpdateInventory();
        }

        //on enter crafting area
        if (collision.gameObject.CompareTag("Crafting"))
        {
            inCraftingArea = true;
        }

        //on zone enter
        if (collision.gameObject.CompareTag("Zone"))
        {
            if(tempFix > 6) {
                //update the zone the player is currently in to the prev zone
                int zone = collision.gameObject.GetComponent<ZoneHandler>().zone;
                runtimeData.currZone = zone;
                GameEvents.InvokeUpdateZone("Zone " + zone);
            }
            else
            {
                tempFix++;
            }
        }

        //on safezone enter
        if (collision.gameObject.CompareTag("SafeZone"))
        {
            runtimeData.playerInSpawn = true;
            GameEvents.InvokeUpdateZone("Safehouse");
        }

        //on water enter
        if (collision.gameObject.CompareTag("Water"))
        {
            currMoveSpeed *= 0.5f; //slows the player by 50% movespeed
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!runtimeData.gameOver) {
            //on exit crafting area
            if (collision.gameObject.CompareTag("Crafting"))
            {
                inCraftingArea = false;
            }

            //on zone exit
            if (collision.gameObject.CompareTag("Zone"))
            {
                //on reaching "exit", end game
                if (runtimeData.currZone == 6)
                {
                    GameEvents.InvokeEndGame("escaped");
                }
                else
                {
                    //update the zone the player is currently in to the next zone
                    int zone = collision.gameObject.GetComponent<ZoneHandler>().zone;
                    runtimeData.currZone = zone + 1;
                    GameEvents.InvokeUpdateZone("Zone " + runtimeData.currZone);
                }
            }

            //on safezone exit
            if (collision.gameObject.CompareTag("SafeZone"))
            {
                runtimeData.playerInSpawn = false;
                GameEvents.InvokeUpdateZone("Zone 1");
            }

            //on water exit
            if (collision.gameObject.CompareTag("Water"))
            {
                if (currMoveSpeed * 2 > baseMoveSpeed)
                {
                    currMoveSpeed = baseMoveSpeed;
                }
                else
                {
                    currMoveSpeed *= 2;
                }
            }
        }
    }   

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnAmmoCrafted(object sender, AmmoEventArgs args)
    {
        int amount = args.amount;

        //update weapon ammo
        Weapons[currWeaponIndex].GetComponent<Weapon>().addAmmo(args.amount);
    }

    //change player weapon to slected weapon and store previous weapon
    void OnWeaponSelected(object sender, WeaponEventArgs args)
    {
        Weapons[currWeaponIndex].SetActive(false);
        currWeaponIndex = args.index;
        Weapons[currWeaponIndex].SetActive(true);
        currWeapon = Weapons[currWeaponIndex].GetComponent<Weapon>();

        //pass on the weapon to update WeaponUI
        GameEvents.InvokeUpdateWeaponUI(Weapons[currWeaponIndex], currWeaponIndex);
    }

    //handles decreasing player health
    public void takeDamage(int amount)
    {
        health -= amount;
        anim.SetTrigger("hit");
        GameEvents.InvokeUpdateHealthUI(health); 
        
        //check if health falls to ZERO
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    void Die()
    {
        runtimeData.gameOver = true;
        GameEvents.InvokeEndGame("died");
        dead = true;
        sprite.SetActive(false);
        hitbox.enabled = false;
        Invoke("RestartGame", 2f);
    }

    void OnWeaponReloaded(object sender, EventArgs args)
    {
        Weapons[currWeaponIndex].GetComponent<Weapon>().Reload();
    }

    void OnHealReady(object sender, EventArgs args)
    {
        healOnCD = false;
    }

    void OnGameOver(object sender, GameOverEventArgs args)
    {
        GameEvents.CraftAmmo -= OnAmmoCrafted;
        GameEvents.UpdateWeapon -= OnWeaponSelected;
        GameEvents.WeaponReloaded -= OnWeaponReloaded;
        GameEvents.EndGame -= OnGameOver;
    }
}
