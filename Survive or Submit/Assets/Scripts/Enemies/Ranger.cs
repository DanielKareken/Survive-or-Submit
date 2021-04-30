using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Enemy
{
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    public AudioSource fireballShootSound;
    public float aggroRange; //this needs to be the same as in the AIPath
    public float fireballForce;
    public float fireRate; //time between each attack

    Transform playerPos;
    float distanceToTarget;
    float timeTillNextAttack;
    bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = gameObject.GetComponent<CapsuleCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        dead = false;
        attacking = false;
        timeTillNextAttack = fireRate;
        timeToNextMoan = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //check if health falls to ZERO
        if (health <= 0 && !dead)
        {
            dead = true;
            sprite.SetActive(false);
            hitbox.enabled = false;
            GameObject scrap = Instantiate(scrapPrefab, gameObject.transform.position, gameObject.transform.rotation);
            ScrapCollectable collectable = scrap.GetComponent<ScrapCollectable>();
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

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        distanceToTarget = Vector3.Distance(playerPos.position, transform.position);

        if (distanceToTarget <= aggroRange && !attacking)
        {
            attacking = true;
            Attack();
        }

        //timer
        if (timeTillNextAttack < fireRate)
        {
            timeTillNextAttack += Time.deltaTime;
        }
        else
        {
            attacking = false;
        }

        Aim();
    }

    //look at player
    void Aim()
    {
        Vector2 lookDir = new Vector2(playerPos.position.x, playerPos.position.y) - rb.position;
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = lookAngle;
    }

    void Attack()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        fireball.GetComponent<Bullet>().setDamage(damage * 2);
        rb.AddForce(fireballSpawnPoint.up * fireballForce, ForceMode2D.Impulse);
        timeTillNextAttack = 0;
    }
}
