using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponFireType
{
    MELEE,
    SEMI,
    AUTOMATIC
}

public class Weapon : MonoBehaviour
{
    public WeaponFireType fireType;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public GameObject sprite;
    public float weight;
    public int magSize; //max ammo allowed in one mag
    public int reserveAmmo; //amount of ammo player has for this type of gun
    public int damagePerShot;
    public float bulletForce = 20f;
    public float fireRate; //how fast the weapon can shoot

    private bool allowFire;
    private int currMag; //current ammo in weapon magazine
    private float nextTimeToFire; //time until next shot fired for automatic weapons

    // Start is called before the first frame update
    void Start()
    {
        allowFire = true;
        currMag = magSize;
        reserveAmmo = currMag;
        nextTimeToFire = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if left mouse clicked, attack
        if (fireType == WeaponFireType.MELEE)
        {
            MeleeAttack(); //bypasses Shoot method
        }
        else if (allowFire && currMag != 0)
        {
            Shoot();
        }

        //decrement timer
        if (nextTimeToFire > 0)
        {
            nextTimeToFire -= Time.deltaTime;
        }
    }

    //determines how ot shoot based off weapon type
    public void Shoot()
    {
        //Automatic weapons
        if(fireType == WeaponFireType.AUTOMATIC)
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
            if(Input.GetMouseButtonDown(0))
            {
                //animate + sound fx
                FireBullet();
            }
        }
    }

    //handles actual shooting 
    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<Bullet>().setDamage(damagePerShot);
        rb.AddForce(bulletSpawnPoint.up * bulletForce, ForceMode2D.Impulse);
        currMag--;
        GameEvents.InvokeUpdateWeaponUI(gameObject);            
    }

    //handles melee weapon attack
    public void MeleeAttack()
    {
        //animate + sfx
        nextTimeToFire = Time.deltaTime + 1f / fireRate;
    }

    public void Reload()
    {
        if(currMag != magSize)
        {
            //reload animation

            //math
            int toAdd;
            if(reserveAmmo >= magSize)
            {
                toAdd = magSize;
                reserveAmmo -= magSize;
            }
            else
            {
                toAdd = reserveAmmo;
                reserveAmmo = 0;
            }
            
            currMag = toAdd;
            GameEvents.InvokeUpdateWeaponUI(gameObject);
        }
    }

    public void addAmmo(int amount)
    {
        reserveAmmo += amount;
        print(reserveAmmo);
    }

    public int getCurrAmmo()
    {
        return currMag;
    }
}
