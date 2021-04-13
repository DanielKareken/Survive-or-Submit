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
    public GameObject scrapPrefab;
    public Collider2D hitbox;
    public float health;
    public float speed;
    public float aggroRange;
    public EnemyState enemyState;

    public GameObject player;
    //public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb; //refernce to self rigidbody
    //public Animator anim;

    public int minDrop;
    public int maxDrop;

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
        hitbox = gameObject.GetComponent<CapsuleCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();

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
            GameObject scrap = Instantiate(scrapPrefab, gameObject.transform.position, gameObject.transform.rotation);
            ScrapCollectable collectable = scrap.GetComponent<ScrapCollectable>();
            collectable.setQuantity(Random.Range(minDrop, maxDrop + 1));
            Invoke("Die", 2f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
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
