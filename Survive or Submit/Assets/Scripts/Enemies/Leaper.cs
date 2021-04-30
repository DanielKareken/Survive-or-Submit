using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    PATROL,
    CHANNEL,
    ATTACK
}

public class Leaper : Enemy
{
    public float aggroRange; //this needs to be the same as in the AIPath
    public float dashSpeed;
    public float startDashTime;
    public float startWindupTime;
    public State currentState;

    Transform playerPos;
    AIPath aiScript;
    float distanceToTarget;
    bool attacking;
    bool channeling;
    float dashTime;
    float windupTime;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        aiScript = GetComponent<AIPath>();
        anim = GetComponent<Animator>();

        dead = false;
        damage = baseDamage;
        attacking = false;
        dashTime = 0;
        windupTime = 0;
        currentState = State.PATROL;
        timeToNextMoan = 0;
    }

    void FixedUpdate()
    {
        //check if health falls to ZERO
        if (health <= 0 && !dead)
        {
            dead = true;
            sprite.SetActive(false);
            hitbox.enabled = false;
            GameObject scrap = Instantiate(scrapPrefab, gameObject.transform.position, gameObject.transform.rotation);
            ScrapCollectable collectable = scrap.GetComponent<ScrapCollectable>();
            collectable.setQuantity(Random.Range(minDrop, maxDrop + 1));
            dieSound.Play();
            Invoke("Die", dieSound.time);
        }

        //idle noise timer
        if (timeToNextMoan <= 0)
        {
            idleSound.Play();
        }
        else
        {
            timeToNextMoan -= Time.deltaTime;
        }

        //get player position
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        distanceToTarget = Vector3.Distance(playerPos.position, transform.position);
        
        //states
        if (currentState == State.PATROL)
        {
            Patrol();
        }
        else if (currentState == State.CHANNEL)
        {
            Channel();
        }
        else if (currentState == State.ATTACK)
        {
            DashAttack();
        }

        if (!attacking)
        {
            Aim();
        }
        
    }

    void Patrol()
    {
        aiScript.enabled = true;
        if (distanceToTarget <= aggroRange)
        {
            currentState = State.CHANNEL;
            windupTime = startWindupTime;
        }
    }

    //simulates windup before dashing
    void Channel()
    {
        aiScript.enabled = false;
        //check if in range again (for after first attack)
        if (!attacking && distanceToTarget > aggroRange)
        {
            currentState = State.PATROL;
        }
        else
        {
            if (windupTime > 0)
            {
                windupTime -= Time.deltaTime;
            }
            else
            {
                windupTime = startWindupTime;
                attacking = true;
                dashTime = startDashTime;
                currentState = State.ATTACK;
            }
        }       
    }

    //simulates leap attack
    void DashAttack()
    {
        rb.velocity = transform.up * dashSpeed;
        damage = baseDamage * 2;
        
        //timer
        if (dashTime > 0)
        {
            dashTime -= Time.deltaTime;
            DashAttack();
        }
        else
        {
            attacking = false;
            dashTime = 0;
            damage = baseDamage;
            currentState = State.CHANNEL;
        }
    }

    //look at player
    void Aim()
    {
        Vector2 lookDir = new Vector2(playerPos.position.x, playerPos.position.y) - rb.position;
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = lookAngle;
    }
}
