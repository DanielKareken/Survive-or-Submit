using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponFireType
{
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
    public float magSize; //max ammo allowed in one mag
    public float bulletForce = 20f;

    private bool allowFire;
    private float currMag; //current ammo in weapon magazine

    // Start is called before the first frame update
    void Start()
    {
        allowFire = true;
        currMag = magSize;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        if(allowFire && currMag != 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bulletSpawnPoint.up * bulletForce, ForceMode2D.Impulse);
            currMag--;
        }
    }

    public void Reload()
    {
        if(currMag != magSize)
        {
            //reload animation
            currMag = magSize;
        }
    }
}
