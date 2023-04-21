using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaRifle : Weapon
{
    public GameObject laser;

    bool firing;  

    // Start is called before the first frame update
    void Start()
    {
        laser.GetComponent<Bullet>().setDamage(damagePerShot);
        anim = GetComponent<Animator>();

        currMag = magSize;
        reserveAmmo = currMag;
        nextTimeToFire = 0f;
        laser.SetActive(false);
        firing = true;        
        reloading = false;

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
            laser.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if(firing)
        {
            laser.GetComponent<BoxCollider2D>().enabled = false;
            //reset fireRate
            nextTimeToFire = Time.deltaTime + 1f / fireRate;
        } 
    }

    //determines how ot shoot based off weapon type
    new public void Shoot()
    {
        //Automatic weapons
        if (fireType == WeaponFireType.AUTOMATIC)
        {           
            //on button down
            if (Input.GetMouseButtonDown(0))
            {
                firing = true;
                attackSound.Play();
                laser.SetActive(true);
            }
            //on button release
            else if (Input.GetMouseButtonUp(0))
            {
                firing = false;
                attackSound.Stop();
                laser.SetActive(false);
            }

            //while firing
            if (firing)
            {
                laser.transform.position = bulletSpawnPoint.transform.position;
                laser.transform.rotation = bulletSpawnPoint.transform.rotation;
            }                        
        }

        //Semiautomatic weapons
        else if (fireType == WeaponFireType.SEMI)
        {
            if (Input.GetMouseButtonDown(0) && Time.deltaTime > nextTimeToFire)
            {
                nextTimeToFire = Time.deltaTime + 1f / fireRate;
            }
        }
    }

    //turns off laser on melee
    public void meleeOverride()
    {
        laser.SetActive(false);
        firing = false;
    }
}
