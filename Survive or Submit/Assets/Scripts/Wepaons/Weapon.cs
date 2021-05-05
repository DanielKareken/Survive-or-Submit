using System;
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
    [SerializeField] RuntimeData runtimeData;
    public AudioSource attackSound;
    public AudioSource reloadSound;
    public Animator anim;
    public WeaponFireType fireType;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public GameObject weaponSprite;
    public float weight;
    public int magSize; //max ammo allowed in one mag
    public int reserveAmmo; //amount of ammo player has for this type of gun
    public int damagePerShot;
    public float bulletForce = 20f;
    public float fireRate; //how fast the weapon can shoot
    public int scrapCostScaler; //scales cost of ammo for this weapon's caliber
    public int accuracy;
    public float reloadSpeed;
    public int weaponID;

    protected int currMag; //current ammo in weapon magazine
    protected float nextTimeToFire; //time until next shot fired for automatic weapons
    protected bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        currMag = magSize;
        reserveAmmo = currMag;
        nextTimeToFire = 0f;
        reloading = false;

        anim = GetComponent<Animator>();

        GameEvents.InvokeUpdateWeaponUI(gameObject, weaponID);
    }

    // Update is called once per frame
    void Update()
    {
        //if left mouse clicked, attack
        if (runtimeData.allowWeaponFire)
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
            if(Input.GetMouseButtonDown(0) && Time.deltaTime > nextTimeToFire)
            {
                nextTimeToFire = Time.deltaTime + 1f / fireRate;
                //animate + sound fx
                FireBullet();
            }
        }
    }

    //handles actual shooting 
    protected void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<Bullet>().setDamage(damagePerShot);
        rb.AddForce(bulletSpawnPoint.up * bulletForce, ForceMode2D.Impulse);
        currMag--;
        GameEvents.InvokeUpdateWeaponUI(gameObject, weaponID);
        attackSound.Play();
    }

    public void CallReload()
    {
        if(currMag != magSize && reserveAmmo > 0)
        {
            //start reload
            GameEvents.InvokeReloadWeapon(reloadSpeed);

            if(!reloading)
            {
                reloadSound.Play();
                reloading = true;
            }
            else
            {
                reloadSound.Stop();
                reloading = false;
            }
        }
    }

    public void Reload()
    {
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
        GameEvents.InvokeUpdateWeaponUI(gameObject, weaponID);
    }

    public void addAmmo(int amount)
    {
        reserveAmmo += amount;
    }

    public int getCurrAmmo()
    {
        return currMag;
    }

    public int getAmmoCost()
    {
        return scrapCostScaler;
    }

    public RuntimeData getRuntimeData()
    {
        return runtimeData;
    }

    public void OnWeaponReloaded(object sender, EventArgs args)
    {
        reloading = false;
    }
}
