using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public int spreadCount;

    // Start is called before the first frame update
    void Start()
    {
        currMag = magSize;
        reserveAmmo = currMag;
        nextTimeToFire = 0f;

        anim = GetComponent<Animator>();

        GameEvents.InvokeUpdateWeaponUI(gameObject, weaponID);
    }

    // Update is called once per frame
    void Update()
    {
        //if left mouse clicked, attack
        if (getRuntimeData().allowWeaponFire)
        {
            if (currMag != 0)
            {
                Shoot();
            }
        }

        //decrement timers
        if (nextTimeToFire > 0)
        {
            nextTimeToFire -= Time.deltaTime;
        }
    }

    //determines how ot shoot based off weapon type
    new public void Shoot()
    {
        //Automatic weapons
        if (fireType == WeaponFireType.AUTOMATIC)
        {
            //if left mouse button held down and Time > nextTimeToFire
            if (Input.GetMouseButton(0) && Time.deltaTime > nextTimeToFire)
            {
                nextTimeToFire = Time.deltaTime + 1f / fireRate;
                //animate + sound fx
                FireBullet();
            }
        }

        //Semiautomatic weapons
        else if (fireType == WeaponFireType.SEMI)
        {
            if (Input.GetMouseButtonDown(0) && Time.deltaTime > nextTimeToFire)
            {
                nextTimeToFire = Time.deltaTime + 1f / fireRate;
                //animate + sound fx
                FireBullet();
            }
        }
    }

    //handles shooting multiple bullets
    new void FireBullet()
    {
        for (int i = 0; i < spreadCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Bullet>().setDamage(damagePerShot);
            float spread = Random.Range(-(accuracy / 2), accuracy / 2); //bullet spread angle
            bullet.transform.Rotate(0, 0, spread);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
        }
        
        currMag--;
        GameEvents.InvokeUpdateWeaponUI(gameObject, weaponID);
    }
}
