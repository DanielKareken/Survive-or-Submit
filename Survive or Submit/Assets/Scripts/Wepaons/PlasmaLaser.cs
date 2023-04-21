using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaLaser : Bullet
{
    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //tell enemy to take damage based on what type of ammunition hits it
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().takeDamage(damage);
        }
    }

    public int getDamage()
    {
        return damage;
    }
}
