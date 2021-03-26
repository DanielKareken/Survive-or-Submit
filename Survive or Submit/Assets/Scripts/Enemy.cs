using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject Player;

    public GameObject sprite;
    public BoxCollider2D hitbox;
    public float health;
    public float speed;
    public float aggroRange;
    public EnemyState enemyState;

    public GameObject player;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb; //refernce to self rigidbody
    //public Animator anim;

    public float chaseAfterAttackDistance;
    public float chaseDistance;

    public float patrolRadiusMin, patrolRadiusMax;
    public float patrolForThisTime;

    public float waitBeforeAttack;
    public GameObject attackHitbox;

    float currentChaseDistance;
    float patrolTimer;
    Vector2 randDir;
    private bool dead;
    float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = gameObject.GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        currentChaseDistance = chaseDistance;
        patrolTimer = patrolForThisTime;
        enemyState = EnemyState.PATROL;
        dead = false;
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
            Invoke("Die", 2f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (enemyState == EnemyState.PATROL)
        {
            Patrol();
        }

        if (enemyState == EnemyState.CHASE)
        {
            Chase();
        }

        if (enemyState == EnemyState.ATTACK)
        {
            Attack();
        }
    }

    public void Patrol()
    {
        patrolTimer += Time.deltaTime;
        //print(patrolTimer);

        if (patrolTimer > patrolForThisTime)
        {
            rb.velocity = Vector2.zero;

            //Set random destination
            float randRadius = Random.Range(patrolRadiusMin, patrolRadiusMax);
            randDir = Random.insideUnitSphere * randRadius;
            randDir.x += transform.position.x;
            randDir.y += transform.position.y;

            patrolTimer = 0f;
        }

        randDir = randDir.normalized;

        if (randDir.x < 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        else
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        rb.AddForce(randDir * speed);

        if (Vector2.Distance(transform.position, player.transform.position) <= chaseDistance)
        {
            enemyState = EnemyState.CHASE;
        }
    }

    public void Chase()
    {
        Vector2 dirToPlayer = player.transform.position - transform.position; //get direction between this enemy and player

        if (dirToPlayer.x < 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        else
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        dirToPlayer = dirToPlayer.normalized;
        rb.AddForce(dirToPlayer * speed * 2); //moves enemy

        if (Vector2.Distance(transform.position, player.transform.position) <= aggroRange)
        {
            enemyState = EnemyState.ATTACK;
            rb.velocity = Vector2.zero;
            //play audio?

            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
        }

        //if player runs away from enemy
        else if (Vector2.Distance(transform.position, player.transform.position) > chaseDistance)
        {
            enemyState = EnemyState.PATROL;

            patrolTimer = patrolForThisTime;

            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
        }
    }

    public void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > waitBeforeAttack)
        {
            //anim.SetTrigger("attack");
            attackTimer = 0f;
        }

        if (Vector2.Distance(transform.position, player.transform.position) > aggroRange + chaseAfterAttackDistance)
        {
            enemyState = EnemyState.CHASE;
            attackTimer = waitBeforeAttack - 0.1f;
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
