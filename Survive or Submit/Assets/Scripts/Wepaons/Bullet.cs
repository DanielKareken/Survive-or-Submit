using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    NORMAL,
    PIERCING,
    AOE,
    LASER
}

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public BulletType type;
    protected float lifetimeTimer;
    protected int damage;

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

    public void setDamage(int dmg)
    {
        damage = dmg;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) {
            //GameObject effect = Instanstiate(hitEffect, transform.position, Quaternion, identity);

            //tell enemy to take damage based on what type of ammunition hits it
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().takeDamage(damage);

                if(type != BulletType.PIERCING)
                {
                    DestroyBullet();
                }
            }
            else
            {
                DestroyBullet();
            }
        }
    }

    protected void DestroyBullet()
    {
        if(type == BulletType.AOE)
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        
        Destroy(gameObject);
    }
}
