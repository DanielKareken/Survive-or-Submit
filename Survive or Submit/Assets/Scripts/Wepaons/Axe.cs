using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{
    //public GameObject meleeHitbox;
    public GameObject hitboxPos;
    public LayerMask enemyLayers;
    public float attackRadius;

    bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        currMag = magSize;
        reserveAmmo = currMag;
        nextTimeToFire = 0f;
        isAttacking = false;
        //.SetActive(false);

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //meleeHitbox.transform.position = hitboxPos.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if left mouse clicked, attack
        if (getRuntimeData().allowWeaponFire)
        {
            if (currMag != 0)
            {
                Shoot();
            }
        }

        //decrement timer
        if (nextTimeToFire > 0)
        {
            nextTimeToFire -= Time.deltaTime;
        }
    }

    //handles melee weapon attack
    public new void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.deltaTime > nextTimeToFire)
        {
            //animate + sfx
            anim.SetTrigger("Attack");
            nextTimeToFire = Time.deltaTime + 1f / fireRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (hitboxPos == null)
            return;

        //Gizmos.DrawWireSphere(hitboxPos.transform.position, attackRadius);
    }

    public void toggleHitbox()
    {
        if (isAttacking)
        {
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(hitboxPos.transform.position, attackRadius, enemyLayers);

            foreach (Collider2D enemy in enemiesHit)
            {
                enemy.GetComponent<Enemy>().takeDamage(damagePerShot);
            }

            isAttacking = false;
            //meleeHitbox.SetActive(false);
        }
        else
        {
            isAttacking = true;
            //meleeHitbox.SetActive(true);
        }
    }
}
