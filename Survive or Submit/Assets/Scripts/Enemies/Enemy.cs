using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    public GameObject sprite;
    public GameObject cratePrefab;
    public float crateDropChance;
    public GameObject scrapPrefab;
    public Collider2D hitbox;
    public AudioSource[] idleSound;
    public AudioSource[] hitSound;
    public AudioSource dieSound;
    public int health;
    public int baseDamage;
    public Rigidbody2D rb; //refernce to self rigidbody
    public Animator anim;
    public int minDrop;
    public int maxDrop;
    public int expOnKill;
    /*public float speed;
    public float chaseAfterAttackDistance;
    public float chaseDistance;
    public float waitBeforeAttack;              } handled by AIPath script
    float currentChaseDistance;*/

    protected bool dead;
    protected int damage;
    protected float timeToNextMoan = 0;
    protected int nextIdleSound;
    
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
    void Update()
    {
        MakeIdleSound();
    }

    //idle noise timer
    public void MakeIdleSound()
    {
        if (timeToNextMoan <= 0)
        {
            nextIdleSound = Random.Range(0, idleSound.Length);
            idleSound[nextIdleSound].Play();
            timeToNextMoan = Random.Range(5, 10);
        }
        else
        {
            timeToNextMoan -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //tell enemy to take damage based on what type of ammunition hits it
        if (collision.gameObject.CompareTag("Laser"))
        {
            takeDamage(collision.gameObject.GetComponent<PlasmaLaser>().getDamage());
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("hit");

        //check if health falls to ZERO
        if (health <= 0 && !dead)
        {
            dead = true;
            runtimeData.numActiveEnemies--; //decrease count of enemies in world
            sprite.SetActive(false);
            hitbox.enabled = false;
            dieSound.Play();
            runtimeData.player_experience += expOnKill;

            //drop scrap
            GameObject scrap = Instantiate(scrapPrefab, gameObject.transform.position, gameObject.transform.rotation);
            ScrapCollectable collectable = scrap.GetComponent<ScrapCollectable>();
            collectable.setQuantity(Random.Range(minDrop, maxDrop + 1));
            if (Random.Range(0, 100) <= crateDropChance)
            {
                GameObject crate = Instantiate(cratePrefab, transform.position, transform.rotation);
            }
            

            Invoke("Die", dieSound.clip.length);
        }
        else if (isSoundPlaying(hitSound))
        {
            int i = Random.Range(0, hitSound.Length); //get random sound
            hitSound[i].Play();
        }
    }

    //check if sound is playing
    bool isSoundPlaying(AudioSource[] sounds)
    {
        foreach(AudioSource sound in sounds)
        {
            if(sound.isPlaying)
            {
                return true;
            }
        }

        return false;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public int getDamage()
    {
        return damage;
    }

    protected RuntimeData getRuntimeData()
    {
        return runtimeData;
    }
}
