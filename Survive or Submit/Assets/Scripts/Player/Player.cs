using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] GameObject vCamBounds;
    
    public Animator anim;
    public Rigidbody2D rb;
    public Collider2D hitbox;
    public Camera cam;
    public GameObject sprite;
    public GameObject crosshair;
    public AudioSource levelUpSound;
    public AudioSource walkingSound;
    public AudioSource pickupCollectable;
    public float baseMoveSpeed;
    public int maxHealth;

    Vector2 movement;
    Vector2 mousePos;
    float currMoveSpeed;
    bool allowActions; //checks whether player can perform actions
    bool allowMovement; //checks whether player can move and aim
    float damageImmunityTimer;
    int health;
    int healAmount;
    bool dead;
    bool crafting;
    bool healOnCD;
    int tempFix = 1; //temporary fix for zone triggers all entering on start
    

    // Start is called before the first frame update
    void Start()
    {
        //init private variables
        allowActions = true;
        allowMovement = true;
        dead = false;
        crafting = false;
        health = maxHealth;
        healOnCD = false;
        currMoveSpeed = baseMoveSpeed;

        runtimeData.player_max_health = maxHealth;
        runtimeData.weapons_unlocked = 1;

        //access components
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CircleCollider2D>();
        //weapon = currWeapon[currWeaponIndex].GetComponent<Weapon>();
        anim = GetComponent<Animator>();

        Cursor.visible = false;

        GameEvents.InvokeUpdateInventory();

        //events
        GameEvents.HealEnded += OnHealReady;
        GameEvents.QuickMeleeFinished += OnQuickMeleeFinished;
        GameEvents.EndGame += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        //player actions
        if (allowActions && !dead && !runtimeData.game_paused)
        {
            Heal();
            weaponHolder.Reload();
            QuickMelee();
        }

        //pause menu
        if (!dead)
        {
            Pause();
        }

        //player menus
        Crafting();

        //check player experience
        if (runtimeData.player_experience >= runtimeData.player_exp_to_lvl_up)
        {
            runtimeData.player_experience -= runtimeData.player_exp_to_lvl_up;
            runtimeData.player_exp_to_lvl_up += runtimeData.player_exp_to_lvl_up / 5;
            runtimeData.player_level++;
            levelUpSound.Play();
            GameEvents.InvokeDisplayLevelUpUI(false);
        }

        //testing
        if (Input.GetKeyDown(KeyCode.I))
        {
            runtimeData.player_level++;
        }
    }

    void FixedUpdate()
    {
        if (allowMovement && !dead && !runtimeData.game_paused)
        { 
            Movement();
            Aim();
        }

        //update vCamBounds object to stick on player
        vCamBounds.transform.position = transform.position;
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
            walkingSound.mute = true;
        }
        else
        {
            anim.SetBool("IsWalking", true);
            walkingSound.mute = false;
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

    void Heal()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!healOnCD && runtimeData.player_meds > 0)
            {
                int healAmount = (int)(25 * runtimeData.player_heal_multi);
                if (health > maxHealth - healAmount)
                {
                    health = maxHealth;
                }
                else
                {
                    health += healAmount;
                }

                runtimeData.player_meds--;
                healOnCD = true;
                GameEvents.InvokeUpdateHealthUI(health);
                GameEvents.InvokeActivateHeal(10); //heal cooldown is 10 seconds  
            }
        }       
    }

    //player can use melee weapon with quick bind
    void QuickMelee()
    {
        if(Input.GetKeyDown(KeyCode.F) && weaponHolder.GetCurWeaponIndex() != 0)
        {
            allowActions = false;
            weaponHolder.QuickMelee();
        }           
    }

    //safehouse interactions
    void Crafting()
    {
        //if E button pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!crafting)
            {
                //bring up crafting and halt player actions
                GameEvents.InvokeCraftingDisplay();
                allowActions = false;
                allowMovement = false;
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
                allowMovement = true;
                Cursor.visible = false;
                crosshair.SetActive(true);
                crafting = false;
                runtimeData.allowWeaponFire = true;
            }
        }
    }

    void Pause()
    {
        //pause game
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameEvents.InvokePauseGame();
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
        //pick ups
        if (collision.gameObject.CompareTag("Collectable"))
        {
            pickupCollectable.Play();
            ScrapCollectable scrap = collision.gameObject.GetComponent<ScrapCollectable>();
            scrap.OnCollected();
            int amount = scrap.getQuantity();
            runtimeData.player_scrap += amount;
            GameEvents.InvokeUpdateInventory();
        }
        else if(collision.gameObject.CompareTag("ItemCrate"))
        {
            ItemCrate crate = collision.gameObject.GetComponent<ItemCrate>();

            if(crate.crateType == CrateType.WeaponCache)
            {
                runtimeData.weapons_unlocked++;
                GameEvents.InvokeDisplayLevelUpUI(true);
            }
            else if(crate.crateType == CrateType.MedBag)
            {
                runtimeData.player_meds += crate.GetItemAmount();
                GameEvents.InvokeUpdateInventory();
            }
            
            Destroy(collision.gameObject);
        }

        //on enter crafting area
        if (collision.gameObject.CompareTag("Crafting"))
        {
            //inCraftingArea = true;
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
                //inCraftingArea = false;
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

    /*change player weapon to slected weapon and store previous weapon
    void OnWeaponSelected(object sender, WeaponEventArgs args)
    {
        weapons[currWeaponIndex].SetActive(false);
        currWeaponIndex = args.index;
        weapons[currWeaponIndex].SetActive(true);
        currWeapon = weapons[currWeaponIndex].GetComponent<Weapon>();

    }*/

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
    }

    void OnHealReady(object sender, EventArgs args)
    {
        healOnCD = false;
    }

    void OnQuickMeleeFinished(object sender, EventArgs args)
    {
        allowActions = true;
    }

    void OnGameOver(object sender, GameOverEventArgs args)
    {
        GameEvents.EndGame -= OnGameOver;
    }
}
