using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private RuntimeData runtimeData;
    [SerializeField] public GameObject[] weapons;
    [SerializeField] private PlayerUI _playerUI;
    [SerializeField] private CraftingUI _craftingUI;

    Weapon currWeapon;
    int currWeaponIndex;

    // Start is called before the first frame update
    void Start()
    {
        //initialize weapons
        int totalWeapons = weapons.Length;
        foreach (GameObject wp in weapons)
        {
            wp.SetActive(false);
        }

        weapons[0].SetActive(true);
        currWeaponIndex = 0;
        currWeapon = weapons[currWeaponIndex].GetComponent<Weapon>();

        //events
        GameEvents.WeaponReloaded += OnWeaponReloaded;
        GameEvents.QuickMeleeFinished += OnQuickMeleeFinished;
        GameEvents.UpdateInventory += OnUpdateInventory;
    }

    // Update is called once per frame
    void Update()
    {
        InputListener();
    }

    //listens to inputs from player
    void InputListener()
    {
        //input calls
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchWeapon(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchWeapon(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SwitchWeapon(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SwitchWeapon(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SwitchWeapon(8);
        }

        //Quick Melee: player can use melee weapon with quick bind
        if (currWeaponIndex != 0 && Input.GetKeyDown(KeyCode.F))
        {
            weapons[currWeaponIndex].SetActive(false);
            weapons[0].SetActive(true);
            weapons[0].GetComponent<Axe>().QuickMelee();

            if (currWeaponIndex == 8)
            {
                weapons[currWeaponIndex].GetComponent<PlasmaRifle>().meleeOverride(); //for plasma rifle interaction only
            }


        }
    }

    void SwitchWeapon(int newWeaponIndex)
    {
        if (newWeaponIndex == currWeaponIndex || newWeaponIndex >= runtimeData.weapons_unlocked) return; //ignore same weapon and check if unlocked

        //holster current weapon
        currWeapon.gameObject.SetActive(false);

        //equip new weapon
        Weapon newWeapon = weapons[newWeaponIndex].GetComponent<Weapon>();
        newWeapon.gameObject.SetActive(true);
        currWeapon = newWeapon;
        currWeaponIndex = newWeaponIndex;

        //pass on the weapon to update WeaponUI
        GameEvents.InvokeUpdateWeaponUI(weapons[currWeaponIndex], currWeaponIndex);
    }

    public void QuickMelee()
    {
        weapons[currWeaponIndex].SetActive(false);
        weapons[0].SetActive(true);
        weapons[0].GetComponent<Axe>().QuickMelee();

        if (currWeaponIndex == 8)
        {
            weapons[currWeaponIndex].GetComponent<PlasmaRifle>().meleeOverride(); //for plasma rifle interaction only
        }

        if (weapons[currWeaponIndex].GetComponent<Weapon>().reloading) //quick melee cancels reload
        {
            currWeapon.CallReload(_playerUI);
        }
    }

    public void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            currWeapon.CallReload(_playerUI);
        }
    }

    //getters
    public Weapon GetCurWeapon()
    {
        return currWeapon;
    }
    public int GetCurWeaponIndex()
    {
        return currWeaponIndex;
    }

    //add ammo to a weapon
    public void AddAmmo(int index, int amount)
    {
        //update weapon ammo
        weapons[index].GetComponent<Weapon>().addAmmo(amount);
    }

    void OnGameOver(object sender, GameOverEventArgs args)
    {
        GameEvents.WeaponReloaded -= OnWeaponReloaded;
        GameEvents.QuickMeleeFinished -= OnQuickMeleeFinished;
    }

    void OnWeaponReloaded(object sender, EventArgs args)
    {
        weapons[currWeaponIndex].GetComponent<Weapon>().Reload();
    }

    void OnQuickMeleeFinished(object sender, EventArgs args)
    {
        weapons[0].SetActive(false);
        weapons[currWeaponIndex].SetActive(true);
    }

    void OnUpdateInventory(object sender, EventArgs args)
    {
        GameEvents.InvokeUpdateWeaponUI(weapons[currWeaponIndex], currWeaponIndex);
    }
}
