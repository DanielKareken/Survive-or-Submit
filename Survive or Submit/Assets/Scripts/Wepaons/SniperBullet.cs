using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : Bullet
{
    void Update()
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        //tell enemy to take damage based on what type of ammunition hits it
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().takeDamage(damage);
        }
    }
}
