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

    void Update()
    {
        MakeIdleSound();

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
    }

    void FixedUpdate()
    {
        //get player position
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        distanceToTarget = Vector3.Distance(playerPos.position, transform.position);
    }

    void Patrol()
    {
        anim.SetBool("charging", false);
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
        anim.SetBool("charging", true);
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
        anim.SetBool("charging", false);
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
