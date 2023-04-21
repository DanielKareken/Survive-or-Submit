using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon
{
    bool firing;

    // Start is called before the first frame update
    void Start()
    {
        currMag = magSize;
        reserveAmmo = currMag;
        nextTimeToFire = 0f;
        firing = false;
        reloading = false;

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
            if (Input.GetMouseButton(0))
            {
                if (Time.deltaTime > nextTimeToFire)
                {
                    nextTimeToFire = Time.deltaTime + 1f / fireRate;
                    //animate + sound fx
                    FireBullet();
                }

                if(!firing)
                {
                    firing = true;
                    attackSound.Play();
                }
                
            }
            else
            {
                if(firing)
                {
                    attackSound.Stop();
                    firing = false;
                }
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
}
