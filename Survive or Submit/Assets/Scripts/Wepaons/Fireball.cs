using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Bullet
{
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_lifetime > 0)
        {
            _lifetime -= Time.fixedDeltaTime;
        }
        else
        {
            DestroyBullet();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Water"))
        {
            //GameObject effect = Instanstiate(hitEffect, transform.position, Quaternion, identity);

            //tell enemy to take damage based on what type of ammunition hits it
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Player>().takeDamage(damage);
            }

            DestroyBullet();
        }
    }
}
