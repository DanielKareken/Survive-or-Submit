using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] GameObject ammo;

    Weapon weapon;
    Text ammoText;
    int curr;
    int reserve;

    // Start is called before the first frame update
    void Start()
    {
        ammoText = ammo.GetComponent<Text>();
        ammoText.text = curr + "/" + reserve;

        //events
        GameEvents.UpdateWeaponUI += OnWeaponUIUpdated;
    }
    void Update()
    {
        
    }

    public void OnWeaponUIUpdated(object sender, WeaponEventArgs args)
    {
        weapon = args.weapon.GetComponent<Weapon>();
        curr = weapon.getCurrAmmo();
        reserve = weapon.reserveAmmo;
        ammoText.text = curr + "/" + reserve;
    }


}
