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

    protected float _lifetime; //how long bullet lasts in world before despawning
    protected int damage;

    void Update()
    {
        if(_lifetime > 0)
        {
            _lifetime -= Time.deltaTime;
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

    public void setLifetime(float lifetime)
    {
        _lifetime = lifetime;
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
                print(collision.gameObject.tag);
            }
        }
    }

    protected void DestroyBullet()
    {
        if(type != BulletType.LASER)
        {
            if(type == BulletType.AOE)
            {
                Instantiate(hitEffect, transform.localPosition, transform.rotation);
            }
        
            Destroy(gameObject);
        }
    }
}
