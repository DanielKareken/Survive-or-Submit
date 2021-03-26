using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public GameObject hitEffect;
    public float damage;

    private float lifetimeTimer;

    private void Start()
    {
        lifetimeTimer = 3f;
    }

    private void FixedUpdate()
    {
        if(lifetimeTimer > 0)
        {
            lifetimeTimer -= Time.fixedDeltaTime;
        }
        else
        {
            DestroyBullet();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) {
            //GameObject effect = Instanstiate(hitEffect, transform.position, Quaternion, identity);

            //tell enemy to take damage based on what type of ammunition hits it
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().takeDamage(damage);
            }
            
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        //Destroy(effect, 2f);
        Destroy(gameObject);
    }
}
