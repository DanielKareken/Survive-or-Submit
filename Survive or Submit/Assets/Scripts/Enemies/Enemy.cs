using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject sprite;
    public GameObject scrapPrefab;
    public Collider2D hitbox;
    public AudioSource idleSound;
    public AudioSource hitSound;
    public AudioSource dieSound;

    public int health;
    
    public int baseDamage;
    public Rigidbody2D rb; //refernce to self rigidbody
    public Animator anim;

    public int minDrop;
    public int maxDrop;

    /*public float speed;
    public float chaseAfterAttackDistance;
    public float chaseDistance;
    public float waitBeforeAttack;              } handled by AIPath script
    float currentChaseDistance;*/

    protected bool dead;
    protected int damage;
    protected float timeToNextMoan = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dead = false;
        damage = baseDamage;
    }

    // Update is called once per frame
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
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("hit");
        hitSound.Play();
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    public int getDamage()
    {
        return damage;
    }
}
